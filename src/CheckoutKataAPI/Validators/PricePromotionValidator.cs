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
    public class PricePromotionValidator : AbstractValidator<PricePromotion> {

        public PricePromotionValidator() {
            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(ValidationConstants.DEFAULT_TEXT_FIELD_MAX_SIZE);

            RuleFor(p => p.PriceDiscount)
                .Must(p => p > 0 && p < ValidationConstants.DEFAULT_MAX_PRICE);
        }
    }
}
