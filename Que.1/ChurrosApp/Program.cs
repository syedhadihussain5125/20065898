using ChurrosApp;
using System.Collections.Generic;

Dictionary<int, Churros> menu = new()
{
	{ 1, new Churros("Churros with plain sugar", 6.00m) },
	{ 2, new Churros("Churros with cinnamon sugar", 6.00m) },
	{ 3, new Churros("Churros with chocolate sauce", 8.00m) },
	{ 4, new Churros("Churros with Nutella", 8.00m) }
};

Queue<Order> orderQueue = new();
int nextOrderNo = 1;
bool running = true;

while (running)
{
	DisplayMainMenu();
	Console.Write("Choose your option: ");
	string? choice = Console.ReadLine();

	switch (choice)
	{
		case "1":
			PlaceOrder(menu, orderQueue, ref nextOrderNo);
			break;
		case "2":
			DeliverOrder(orderQueue);
			break;
		case "0":
			running = false;
			Console.WriteLine("System closed. Goodbye.");
			break;
		default:
			Console.WriteLine("Invalid option. Please choose 0, 1, or 2.");
			break;
	}

	Console.WriteLine();
}

static void DisplayMainMenu()
{
	Console.WriteLine("------------------------------------------------------");
	Console.WriteLine("Delicious Churros:");
	Console.WriteLine("1. Churros with plain sugar: EUR 6.00");
	Console.WriteLine("2. Churros with cinnamon sugar: EUR 6.00");
	Console.WriteLine("3. Churros with chocolate sauce: EUR 8.00");
	Console.WriteLine("4. Churros with Nutella: EUR 8.00");
	Console.WriteLine("------------------------------------------------------");
	Console.WriteLine("1. Place order");
	Console.WriteLine("2. Deliver order");
	Console.WriteLine("0. Exit");
	Console.WriteLine("------------------------------------------------------");
}

static void PlaceOrder(Dictionary<int, Churros> menu, Queue<Order> orderQueue, ref int nextOrderNo)
{
	Console.Write("Select churros type (1-4): ");
	if (!int.TryParse(Console.ReadLine(), out int itemChoice) || !menu.ContainsKey(itemChoice))
	{
		Console.WriteLine("Invalid churros selection.");
		return;
	}

	Console.Write("Enter quantity: ");
	if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
	{
		Console.WriteLine("Invalid quantity. Quantity must be greater than 0.");
		return;
	}

	Order order = new(nextOrderNo++, menu[itemChoice], quantity);
	Console.WriteLine(order.PlaceOrder());
	Console.WriteLine("Success: your order has been placed.");

	decimal total = order.PayBill();
	Console.WriteLine($"Payment successful. Total bill: {FormatEur(total)}");

	orderQueue.Enqueue(order);
	Console.WriteLine($"Order #{order.OrderNo} added to queue.");
	Console.WriteLine($"Orders waiting: {orderQueue.Count}");
}

static void DeliverOrder(Queue<Order> orderQueue)
{
	if (orderQueue.Count == 0)
	{
		Console.WriteLine("No pending orders in queue.");
		return;
	}

	Order next = orderQueue.Dequeue();
	Console.WriteLine(next.CollectOrder());
	Console.WriteLine($"Delivered item: {next.OrderDetails.Name} x {next.Quantity}");
	Console.WriteLine($"Remaining orders: {orderQueue.Count}");
}

static string FormatEur(decimal amount)
{
	return $"EUR {amount:0.00}";
}
