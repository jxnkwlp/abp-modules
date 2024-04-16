using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class AccountUserDelegationCreateDto : IValidatableObject
{
    public Guid UserId { get; set; }

    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartTime > EndTime)
        {
            yield return new ValidationResult(
                "The start time cannot be greater than the end time",
                new[] { nameof(StartTime), nameof(EndTime) });
        }
    }
}
