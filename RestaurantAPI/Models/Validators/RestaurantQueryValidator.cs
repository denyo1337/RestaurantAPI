using FluentValidation;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Models.Validators
{
    public class RestaurantQueryValidator:AbstractValidator<RestaurantQuery> // 
    {
        private int[] allowedPagesize = new int[] { 5, 10, 15 };
        private string[] allowedSortByColumnNames = { nameof(Restaurant.Name), nameof(Restaurant.Category), nameof(Restaurant.Description) };

        public RestaurantQueryValidator()
        {
           
            RuleFor(x => x.pageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.pageSize).Custom((value, context) =>
              {
                  if(!allowedPagesize.Contains(value))
                  {
                      context.AddFailure("PageSize", $"Page size must be in [{string.Join(",",allowedPagesize)}]");
                  }
              });
            RuleFor(x => x.SortBy).Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value)).WithMessage($"Sort by is optional or must be in {string.Join(",", allowedSortByColumnNames)}");
        }
    }
}
