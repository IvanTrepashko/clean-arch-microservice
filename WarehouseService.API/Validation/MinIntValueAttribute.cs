using System.ComponentModel.DataAnnotations;

namespace WarehouseService.API.Validation
{
    public class MinIntValueAttribute : ValidationAttribute
    {
        public int MinValue { get; init; }

        public MinIntValueAttribute(int minValue)
        {
            MinValue = minValue;
        }

        public string GetErrorMessage()
        {
            return $"Value should be more than {MinValue}.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            int intValue = (int)value;

            if (intValue < MinValue)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
