using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Constants;
using CheckoutKataAPI.Entities.Products;

namespace CheckoutKataAPI.Validators
{
    public class ProductValidator : AbstractValidator<Product> {

        public ProductValidator() {
            RuleFor(p => p.SKU)
                .NotEmpty()
                .MaximumLength(ValidationConstants.DEFAULT_TEXT_FIELD_MAX_SIZE);

            RuleFor(p => p.Price)
                .Must(p => p > 0 && p < ValidationConstants.DEFAULT_MAX_PRICE);

            RuleFor(p => p.PriceType)
                .Must(p => Enum.IsDefined(typeof(PriceType), p));
        }
    }
}
