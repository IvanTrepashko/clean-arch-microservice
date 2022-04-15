using System.ComponentModel.DataAnnotations;

namespace WarehouseService.API.Validation
{
    public class MinUIntValueAttribute : ValidationAttribute
    {
        public uint MinValue { get; init; }

        public MinUIntValueAttribute(uint minValue)
        {
            MinValue = minValue;
        }

        public string GetErrorMessage()
        {
            return $"Value should be more than {MinValue}.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            uint uintValue = (uint)value;
           
            if (uintValue < MinValue)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
