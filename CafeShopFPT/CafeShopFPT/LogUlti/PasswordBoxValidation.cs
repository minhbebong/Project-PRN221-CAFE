using System.Globalization;
using System.Windows.Controls;

namespace CafeShopFPT.LogUlti {
    public class PasswordBoxValidation :ValidationRule {


        public override ValidationResult Validate(object value,CultureInfo cultureInfo) {
            if (!string.IsNullOrEmpty(value.ToString())) {

                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false,"Field is required!");
        }
    }
}
