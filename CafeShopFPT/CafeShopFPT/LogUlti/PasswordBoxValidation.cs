﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CafeShopFPT.LogUlti
{
    public class PasswordBoxValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(!string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Field is required!");
        }
    }
}