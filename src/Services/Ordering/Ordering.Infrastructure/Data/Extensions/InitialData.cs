namespace Ordering.Infrastructure.Data.Extensions;

internal class InitialData
{
    public static IEnumerable<Customer> Customers =>
        [
            Customer.Create(CustomerId.Of(new Guid("2629cf76-2a44-49eb-845d-2d63a1abc5eb")), "mehmet", "mehmet@gmail.com"),
            Customer.Create(CustomerId.Of(new Guid("badf7e2a-cd63-46a2-bebe-8a93efbd7b50")), "john", "john@gmail.com")
        ];

    public static IEnumerable<Product> Products =>
        [
            Product.Create(ProductId.Of(new Guid("e0cfa8d0-0da6-4c47-9b29-c687b10394fa")), "IPhone X", 500),
            Product.Create(ProductId.Of(new Guid("86eca081-bfcc-4ae8-996a-1ee9b724e875")), "Samsung 10", 400),
            Product.Create(ProductId.Of(new Guid("569dd787-218d-4cc7-ad56-6d54d6beecf9")), "Huawei Plus", 650),
            Product.Create(ProductId.Of(new Guid("6e5c6b10-94c1-45d4-8265-335c9e2875ea")), "Xiaomi Mi", 450)
        ];

    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            var address1 = Address.Of("mehmet", "ozkaya", "mehmet@gmail.com", "Bahcelievler No:4", "Turkey", "Istanbul", "38050");
            var address2 = Address.Of("john", "doe", "john@gmail.com", "Broadway No:1", "England", "Nottingham", "08050");

            var payment1 = Payment.Of("mehmet", "123456789012", "12/28", "355", 1);
            var payment2 = Payment.Of("john", "210987654321", "06/30", "222", 2);

            var order1 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("2629cf76-2a44-49eb-845d-2d63a1abc5eb")),
                OrderName.Of("Order 1"),
                address1,
                address1,
                payment1);

            order1.Add(ProductId.Of(new Guid("e0cfa8d0-0da6-4c47-9b29-c687b10394fa")), 2, 500);
            order1.Add(ProductId.Of(new Guid("86eca081-bfcc-4ae8-996a-1ee9b724e875")), 1, 400);

            var order2 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("badf7e2a-cd63-46a2-bebe-8a93efbd7b50")),
                OrderName.Of("Order 2"),
                address2,
                address2,
                payment2);

            order2.Add(ProductId.Of(new Guid("569dd787-218d-4cc7-ad56-6d54d6beecf9")), 1, 650);
            order2.Add(ProductId.Of(new Guid("6e5c6b10-94c1-45d4-8265-335c9e2875ea")), 2, 450);

            return [order1, order2];
        }
    }
}