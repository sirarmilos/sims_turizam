using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.ValidationRules
{
    public class IntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int result;
            bool isValid = int.TryParse(value.ToString(), out result);

            if (!isValid)
            {
                return new ValidationResult(false, "Entry must be an integer.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
