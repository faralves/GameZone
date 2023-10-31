using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.Core.DomainObjects
{
    public static class GetValidationErrors
    {
        public static List<string> GetListValidationErrors(object instance)
        {
            var context = new ValidationContext(instance, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(instance, context, results, true);

            if (!isValid)
            {
                return results.Select(result => result.ErrorMessage).ToList();
            }

            return new List<string>();
        }

    }
}
