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
    public class BuyXGetYPromotionValidator : AbstractValidator<BuyXGetYPromotion> {

        public BuyXGetYPromotionValidator() {
            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(ValidationConstants.DEFAULT_TEXT_FIELD_MAX_SIZE);

            RuleFor(p => p.ApplyLimit)
                .Must(p => !p.HasValue || p.Value > 0);

            RuleFor(p => p.BuyItems)
                .SetCollectionValidator(new BuyPromotionItemValidator());

            RuleFor(p => p.GetItems)
                .SetCollectionValidator(new GetPromotionItemValidator());
        }
    }
}
