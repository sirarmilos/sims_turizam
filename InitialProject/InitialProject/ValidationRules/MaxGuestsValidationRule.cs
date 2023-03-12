using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.ValidationRules
{
    public class MaxGuestsValidationRule : ValidationRule
    {
        public int GreatherThenZero
        {
            get; set;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (Convert.ToInt32(value) <= 0)
            {
                return new ValidationResult(false, "Max Numbers must be greather then zero.");
            }

            return new ValidationResult(true, null);
        }
    }
}
