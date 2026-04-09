namespace ChurrosApp;

public class Churros
{
    public string Name { get; }
    public decimal Price { get; }

    public Churros(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Name}: {Price:C2}";
    }
}
