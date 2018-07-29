# Checkout
The following checkout API allows to create and get orders, add and remove products from an order and get the correct pre-tax total of an order. It also allows to setup products, price promotions(markdowns) and Bux X Get Y promotions. To reflect correct pre-tax total API recalculates an order for any order related operation based on the current setup and the current date.

## Use Cases:
1. Setup product of 2 types: with price per each item and with price per lb. Each product has unique SKU.

2. Setup Buy X Get Y promotions. Allow to specify a list of product which should be in an order with the needed quantities and a list of product which will be added based on a promotion with percent discount based on regular prices.
Allow to specify date range when a promotion is active.

3. Setup price promotions. Allow to get reduction of price for a specific list of products. Allow to specify date range when a promotion is active. If multiple price promotions are active for a specific product, the one with biggest discount will be applied. 
Price promotions don't work for products which were added by Buy X Get Y promotions.

4. Start a new order.

5. Get an order by id with correct pre-tax total based on the current setup and the current date.

6. Add a product into an order based on SKU and needed quantity and get current order's total. Allow to use fractional quantity(weight) for products with price per lb. 

7. Remove a product from an order based on SKU and get current order's total.

## API Endpoints:
- `/api/products` `Method: POST` - Setup a product. Example of JSON: `{"SKU": "Product1", "PriceType": 2, "Price": 1.5}`.
PriceType possible values: 1 - price per each item, 2 - price per lb. Returns a product with assigned Id.


- `/api/pricepromotions` `Method: POST` - Setup a price promotion(markdown price). Example of JSON: `{"Name": "Test1PricePromo", "PriceDiscount": 0.7, "AssignedProductIds": [1], "StartDate": "2018-07-25T00:00:00.000Z", "EndDate": "2019-07-25T00:00:00.000Z"}`.
AssignedProductIds - product ids for which a promotion is applied. StartDate and EndDate specifies when a promotion is active, dates can be null. Returns a promotion with an assigned Id.


- `/api/buygetpromotions` `Method: POST` - Setup a buy x get y promotion. Example of JSON: `{"Name": "Test1BuyGetPromo", "BuyItems": [{ IdProduct: 1, QTY: 1}],"GetItems": [{ IdProduct: 2, QTY: 1, PercentDiscount: 80}], "StartDate": "2018-07-25T00:00:00.000Z", "EndDate": "2019-07-25T00:00:00.000Z" }`. BuyItems describes the list of products which should be bought with which quantities, all specified products should be in an order. GetItems describes the list of products with quantities which will be added to an order, PercentDiscount - percent discount from a basic product price, should be more than 0 and equal or less than 100. StartDate and EndDate specifies when a promotion is active, dates can be null. Returns a promotion with an assigned Id.


- `/api/orders` `Method: POST` - Create a new empty order. Returns a order with an assigned Id.



- `/api/orders/{idorder}` `Method: GET` - Get an order by `{idorder}`. Returns a calculated order with the correct total.


- `/api/orders/{idorder}/products` `Method: POST` - Add a product into an order with the given `{idorder}`. Example of JSON: `{"SKU": "Product1", "QTY": 2.5}`. SKU is a code of a product in the app, QTY is quantity for adding. Returns a calculated order with the correct total.



- `/api/orders/{idorder}/products/{sku}` `Method: DELETE` - Delete a product with the given `{sku}` from an order with the given `{idorder}`. Returns a calculated order with the correct total.


## Required installation:
.Net Core SDK 2.1 - https://www.microsoft.com/net/download/dotnet-core/2.1

## Cmd commands:
```
api-run
```
```
tests-run
```
