namespace ChurrosApp;

public class Order
{
    private bool _isPaid;

    public int OrderNo { get; }
    public Churros OrderDetails { get; }
    public int Quantity { get; }
    public decimal Bill { get; private set; }

    public Order(int orderNo, Churros orderDetails, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }

        OrderNo = orderNo;
        OrderDetails = orderDetails;
        Quantity = quantity;
    }

    public string PlaceOrder()
    {
        return $"Order #{OrderNo} placed: {Quantity} x {OrderDetails.Name}";
    }

    public decimal PayBill()
    {
        Bill = Quantity * OrderDetails.Price;
        _isPaid = true;
        return Bill;
    }

    public string CollectOrder()
    {
        if (!_isPaid)
        {
            return $"Order #{OrderNo} cannot be collected because payment is pending.";
        }

        return $"Order #{OrderNo} collected successfully.";
    }
}
