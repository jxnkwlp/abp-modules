#!/usr/bin/env bash
# Detect a newer ABP MINOR or MAJOR on NuGet and bump project versions.
# Ignores PATCH-only updates. Writes common.props + host/common.props when applying.
#
# Usage:
#   ./scripts/abp-version-bump.sh              # detect + apply
#   ./scripts/abp-version-bump.sh --dry-run    # detect only, no file writes
#   ./scripts/abp-version-bump.sh --check      # exit 0 if update available, 1 if not
#
# On apply/dry-run with an update, prints KEY=VALUE lines for CI:
#   HAS_UPDATE, CURRENT_ABP, TARGET_ABP, BRANCH, CURRENT_MODULE_VERSION,
#   NEW_MODULE_VERSION, BUMP_KIND (minor|major)

set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
COMMON_PROPS="${ROOT_DIR}/common.props"
HOST_COMMON_PROPS="${ROOT_DIR}/host/common.props"
NUGET_INDEX_URL="https://api.nuget.org/v3-flatcontainer/volo.abp.core/index.json"

DRY_RUN=0
CHECK_ONLY=0

for arg in "$@"; do
  case "$arg" in
    --dry-run) DRY_RUN=1 ;;
    --check) CHECK_ONLY=1 ;;
    -h|--help)
      sed -n '2,16p' "$0"
      exit 0
      ;;
    *)
      echo "Unknown argument: $arg" >&2
      exit 2
      ;;
  esac
done

if [[ ! -f "$COMMON_PROPS" ]]; then
  echo "Missing ${COMMON_PROPS}" >&2
  exit 1
fi

if [[ ! -f "$HOST_COMMON_PROPS" ]]; then
  echo "Missing ${HOST_COMMON_PROPS}" >&2
  exit 1
fi

read_prop() {
  local file="$1"
  local name="$2"
  local value
  value="$(grep -oE "<${name}>[^<]+</${name}>" "$file" | head -n1 | sed -E "s|</?${name}>||g")"
  if [[ -z "$value" ]]; then
    echo "Property ${name} not found in ${file}" >&2
    exit 1
  fi
  printf '%s' "$value"
}

CURRENT_ABP="$(read_prop "$COMMON_PROPS" AbpVersion)"
CURRENT_MODULE="$(read_prop "$COMMON_PROPS" ModuleVersion)"

# Strip any floating range / wildcard for comparison (e.g. 9.0.*)
CURRENT_ABP_BASE="${CURRENT_ABP%%\**}"
CURRENT_ABP_BASE="${CURRENT_ABP_BASE%.}"

parse_semver() {
  # stdin: version string -> stdout: major minor patch (integers); fails on prerelease
  python3 - "$1" <<'PY'
import re, sys
v = sys.argv[1].strip()
# reject prerelease / build metadata for "current" parse of project props? allow plain x.y.z
m = re.fullmatch(r"(\d+)\.(\d+)\.(\d+)", v)
if not m:
    # allow major.minor only
    m = re.fullmatch(r"(\d+)\.(\d+)", v)
    if not m:
        print(f"unsupported version: {v}", file=sys.stderr)
        sys.exit(1)
    print(m.group(1), m.group(2), 0)
else:
    print(m.group(1), m.group(2), m.group(3))
PY
}

read -r CUR_MAJOR CUR_MINOR CUR_PATCH <<<"$(parse_semver "$CURRENT_ABP_BASE")"
read -r MOD_MAJOR MOD_MINOR MOD_PATCH <<<"$(parse_semver "$CURRENT_MODULE")"

TMP_JSON="$(mktemp)"
trap 'rm -f "$TMP_JSON"' EXIT

curl -fsSL "$NUGET_INDEX_URL" -o "$TMP_JSON"

TARGET_INFO="$(
  python3 - "$TMP_JSON" "$CUR_MAJOR" "$CUR_MINOR" <<'PY'
import json, re, sys

path, cur_major, cur_minor = sys.argv[1], int(sys.argv[2]), int(sys.argv[3])
with open(path, encoding="utf-8") as f:
    versions = json.load(f)["versions"]

stable = []
for v in versions:
    # SemVer core only: no -rc / -preview / +build
    if re.fullmatch(r"\d+\.\d+\.\d+", v):
        maj, mino, pat = map(int, v.split("."))
        stable.append((maj, mino, pat, v))

if not stable:
    print("no stable versions found", file=sys.stderr)
    sys.exit(1)

# highest (major, minor), then highest patch on that line
stable.sort()
best_line = max((maj, mino) for maj, mino, _, _ in stable)
line_versions = [t for t in stable if (t[0], t[1]) == best_line]
target = max(line_versions, key=lambda t: t[2])
t_maj, t_min, t_pat, t_ver = target

if (t_maj, t_min) <= (cur_major, cur_minor):
    print("NONE")
    sys.exit(0)

if t_maj > cur_major:
    kind = "major"
elif t_min > cur_minor:
    kind = "minor"
else:
    kind = "none"

print(f"{t_ver}|{t_maj}|{t_min}|{t_pat}|{kind}")
PY
)"

if [[ "$TARGET_INFO" == "NONE" ]]; then
  if [[ "$CHECK_ONLY" -eq 1 ]]; then
    exit 1
  fi
  echo "HAS_UPDATE=false"
  echo "CURRENT_ABP=${CURRENT_ABP}"
  echo "CURRENT_MODULE_VERSION=${CURRENT_MODULE}"
  echo "No ABP MINOR/MAJOR update available (current ${CURRENT_ABP})." >&2
  exit 0
fi

IFS='|' read -r TARGET_ABP T_MAJOR T_MINOR T_PATCH BUMP_KIND <<<"$TARGET_INFO"
BRANCH="abp/${T_MAJOR}.${T_MINOR}.x"

if [[ "$BUMP_KIND" == "major" ]]; then
  NEW_MODULE="$((MOD_MAJOR + 1)).0.0"
elif [[ "$BUMP_KIND" == "minor" ]]; then
  NEW_MODULE="${MOD_MAJOR}.$((MOD_MINOR + 1)).0"
else
  echo "Unexpected bump kind: ${BUMP_KIND}" >&2
  exit 1
fi

if [[ "$CHECK_ONLY" -eq 1 ]]; then
  exit 0
fi

echo "HAS_UPDATE=true"
echo "CURRENT_ABP=${CURRENT_ABP}"
echo "TARGET_ABP=${TARGET_ABP}"
echo "BRANCH=${BRANCH}"
echo "CURRENT_MODULE_VERSION=${CURRENT_MODULE}"
echo "NEW_MODULE_VERSION=${NEW_MODULE}"
echo "BUMP_KIND=${BUMP_KIND}"

if [[ "$DRY_RUN" -eq 1 ]]; then
  echo "Dry run: would set AbpVersion=${TARGET_ABP}, ModuleVersion=${NEW_MODULE}, host AbpVersion=${T_MAJOR}.${T_MINOR}.*" >&2
  exit 0
fi

# Update root common.props
python3 - "$COMMON_PROPS" "$TARGET_ABP" "$NEW_MODULE" <<'PY'
from pathlib import Path
import re, sys
path, abp, mod = Path(sys.argv[1]), sys.argv[2], sys.argv[3]
text = path.read_text(encoding="utf-8")
text2, n1 = re.subn(r"(<AbpVersion>)[^<]+(</AbpVersion>)", rf"\g<1>{abp}\2", text, count=1)
text3, n2 = re.subn(r"(<ModuleVersion>)[^<]+(</ModuleVersion>)", rf"\g<1>{mod}\2", text2, count=1)
if n1 != 1 or n2 != 1:
    raise SystemExit(f"failed to update common.props (AbpVersion={n1}, ModuleVersion={n2})")
path.write_text(text3, encoding="utf-8")
PY

# Update host wildcard AbpVersion (first AbpVersion in host/common.props)
python3 - "$HOST_COMMON_PROPS" "${T_MAJOR}.${T_MINOR}.*" <<'PY'
from pathlib import Path
import re, sys
path, abp = Path(sys.argv[1]), sys.argv[2]
text = path.read_text(encoding="utf-8")
text2, n = re.subn(r"(<AbpVersion>)[^<]+(</AbpVersion>)", rf"\g<1>{abp}\2", text, count=1)
if n != 1:
    raise SystemExit(f"failed to update host/common.props AbpVersion (count={n})")
path.write_text(text2, encoding="utf-8")
PY

echo "Updated ${COMMON_PROPS} and ${HOST_COMMON_PROPS}." >&2
