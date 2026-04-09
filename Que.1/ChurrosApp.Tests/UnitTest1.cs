namespace ChurrosApp.Tests;

[TestClass]
public class OrderTests
{
    [TestMethod]
    public void PayBill_ReturnsTotalAmount_WhenQuantityAndPriceAreValid()
    {
        Churros churros = new("Churros with chocolate sauce", 8.00m);
        Order order = new(1, churros, 3);

        decimal result = order.PayBill();

        Assert.AreEqual(24.00m, result);
        Assert.AreEqual(24.00m, order.Bill);
    }
}