using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M3LMS.ViewModels
{
    public class ValidJoinDate : ValidationAttribute
    {
       
        protected override ValidationResult
                IsValid(object value, ValidationContext validationContext)
        {
            DateTime _dateJoin = Convert.ToDateTime(value);
            if (_dateJoin < DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult
                    ("Join date can not be greater than current date.");
            }
        }

       
    }
}