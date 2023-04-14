using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.ValidationRules
{
    internal class OwnerValidationRules
    {
    }

    public class StringToIntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                bool check = int.TryParse(s, out int checkOut);

                if(check == true)
                {
                    return new ValidationResult(true, null);
                }

                return new ValidationResult(false, "Maximum number of guests must be an integer.");
            }
            catch
            {
                return new ValidationResult(false, "Unknow error occured.");
            }
        }
    }

    public class RangeOfConvertedValue : ValidationRule
    {
        public double Min
        {
            get;
            set;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                int a = (int)value;

                if(a < Min)
                {
                    return new ValidationResult(false, "Maximum number of guests must be greater than 0.");
                }

                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "Unknown error occured.");
            }
        }
    }
}
