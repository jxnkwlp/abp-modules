Get-ChildItem  -Path .\ -Recurse -Depth 5 -Directory -Exclude "node_modules/*" -Include "bin", "obj" | ForEach-Object ($_) { Write-Output $_.FullName; Remove-Item $_ -Force -Recurse }
