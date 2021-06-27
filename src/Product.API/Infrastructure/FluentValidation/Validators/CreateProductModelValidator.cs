using FluentValidation;
using Product.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API.Infrastructure.FluentValidation.Validators
{
    public class CreateProductModelValidator : AbstractValidator<CreateProductModel>
    {
        public CreateProductModelValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Description).NotEmpty();
            RuleFor(p => p.NormalPrice).NotEmpty();
            RuleFor(p => p.DiscountedPrice).NotEmpty();
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Description).NotEmpty();


        }
    }
}
