using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Constants;
using CheckoutKataAPI.Entities.Promotions;

namespace CheckoutKataAPI.Validators
{
    public class BuyPromotionItemValidator : AbstractValidator<BuyPromotionItem> {

        public BuyPromotionItemValidator()
        {
            RuleFor(p => p.IdProduct)
                .Must(p => p != 0);

            RuleFor(p => p.Amount)
                .Must(p => p > 0 && p<ValidationConstants.DEFAULT_MAX_PRICE);
        }
    }
}
