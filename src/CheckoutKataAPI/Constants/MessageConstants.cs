using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Constants
{
    /// <summary>
    /// Constants for storing messages used in the system. For a real word solution it will be better to create a glossary service
    /// which will deal with resolving labels and resolve business valiation messages through it
    /// </summary>
    public class MessageConstants
    {
        public const string DEFAULT_ERROR_MESSAGE = "Oops something went wrong!";

        public const string INVALID_SETUP_MULTIPLE_PRODUCTS_WITH_SAME_SKU = "Multiple products with the same SKU";
        public const string PROMOTION_TYPE_IS_UNKNOWN = "The following promotion type can't be processed";
        public const string LIST_BASED_WORKFLOW_PROCESSOR_NOT_ALLOW_SAME_ACTION_MULTIPLE_TIME = "The same action is specified more than one time";

        // business validation messages
        public const string ADD_PRODUCT_MODEL_IS_EMPTY = "Add product model isn't specififed";
        public const string FRACTIONAL_QTY_NOT_AVALIABLE_IN_ORDER_FOR_PRODUCT_WITH_LB_PRICE =
            "Fractional QTY isn't avaliable for a product with price per each item";
        public const string PRODUCT_NOT_EXIST_IN_ORDER = "The given product doesn't exist in the order";
        public const string DELETE_PROMO_PRODUCT_NOT_PERMITTED_IN_ORDER = "Deleting promotion product isn't permitted";
        public const string NOT_FOUND_ORDER_ID = "Invalid order id";
        public const string NOT_FOUND_PRODUCT_BY_SKU_CODE = "Invalid product SKU";
        public const string NOT_FOUND_PRODUCT_ID = "Invalid product id";
        public const string PRODUCT_SKU_DUPLICATE = "Exist SKU";
        public const string NOT_VALID_PRICE_TYPE_IN_PRODUCT = "Invalid price type";
        public const string MISSED_BUY_PART_IN_GET_BUY_PROMOTION = "Buy part isn't specified";
        public const string MISSED_GET_PART_IN_GET_BUY_PROMOTION = "Get part isn't specified";
    }
}
