using System;

namespace _7_SwitchAndPatternMatching
{
    // Single-File applications
    // dotnet publish -r win-x64 --self-contained true /p:PublishSingleFile=true

    // Examples:
    // dotnet run Product BasePrice BuyerSalary
    // dotnet run  Phone      10        22
    // dotnet run  Phone      10        2
    // dotnet run  Phone      10        5
    // dotnet run  Phone      10       dois
    // dotnet run  Phone      10   Client: [Name]
    // dotnet run  Phone      10     Developer
    class Program
    {
        static Product product;
        static Product blackProduct;
        static Product greenProduct;
        static Product redProduct;
        static void Main(string[] args)
        {
            

            Setup(args);

            if(Double.TryParse(args[2], out var d))
                Console.WriteLine($"Result: {After(d)}");
            else
                Console.WriteLine($"Result: {After(args[2])}");
        }

        static void Setup(string[] args){
            var name = args[0];
            var price = Double.Parse(args[1]);
            product = new Product(name, price, "White");
            blackProduct = product with { Color = "Black", Price = product.Price + 5 };
            greenProduct = product with { Color = "Green", Price = product.Price - 2 };
            redProduct = product with { Color = "Red", Price = product.Price + 10 };

        }

        static string After(object obj)
        {
            switch(obj)
            {
                case string s when s.StartsWith("Client"):
                    return "Please, inform the salary";

                case string s when s.StartsWith("Developer"):
                    return "Wow! Everything is free for you :)";

                case double d:
                    return GetProduct(d);

                // case int or double:
                //     return $"a number";

                case "dois":
                    return "You said 'dois'";

                default:
                    return "default";
            }
        }

        //Relational and Logical Patterns
        static string GetProduct(double d){
            return d switch
            {
                0 => product.ToString(),
                < 0 => greenProduct.ToString(),
                < 5 => blackProduct.ToString(),
                > 5 and < 10 => redProduct.ToString(),
                _ => product.ToString()
            };
        }
    }

public record Product (
        string Name,
        double Price,
        string Color
    );
}
