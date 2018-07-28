using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Constants;
using CheckoutKataAPI.Entities.Products;
using CheckoutKataAPI.Models;

namespace CheckoutKataAPI.Validators
{
    public class AddOrderToProductModelValidator : AbstractValidator<AddOrderToProductModel> {

        public AddOrderToProductModelValidator() {
            RuleFor(p => p.QTY)
                .Must(p => p > 0 && p < ValidationConstants.DEFAULT_MAX_QTY);

            RuleFor(p => p.SKU)
                .Must(p => !string.IsNullOrEmpty(p));
        }
    }
}
