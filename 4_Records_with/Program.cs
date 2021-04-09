using System;
public class Program{
    public static void Main(string[] args){
        var name = args[0];
        var price = double.Parse(args[1]);

        Product product = new (name, price, "White"); //Positional Construction
        Console.Write(" -> ");
        Console.WriteLine(product);

        var blackProduct = product with { Color = "Black" };
        Console.Write(" -> ");
        Console.WriteLine(blackProduct);

        var greenProduct = product with { Color = "Green" };
        Console.Write(" -> ");
        Console.WriteLine(greenProduct);

        var redProduct = product with { Color = "Red" };
        Console.Write(" -> ");
        Console.WriteLine(redProduct);

        var (productName, productPrice, productColor) = product; //Positional Deconstruction
        Console.WriteLine($"Name: {productName}, Price: {productPrice}, Color:{productColor}");

    }
}

public record Product (
        string Name,
        double Price,
        string Color
    );
