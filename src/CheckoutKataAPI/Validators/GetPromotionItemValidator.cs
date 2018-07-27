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
    public class GetPromotionItemValidator : AbstractValidator<GetPromotionItem> {

        public GetPromotionItemValidator()
        {
            RuleFor(p => p.IdProduct)
                .Must(p => p != 0);

            RuleFor(p => p.QTY)
                .Must(p => p > 0 && p<ValidationConstants.DEFAULT_MAX_PRICE);

            RuleFor(p => p.PercentDiscount)
                .Must(p => p > 0 && p<=100);
        }
    }
}
