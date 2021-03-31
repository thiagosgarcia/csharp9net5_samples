public class Program{
    public static void Main(string[] args){
        var name = args[0];
        var age = int.Parse(args[1]);
        var person = new Person()
        {
            Name = name,
            Age = age
        };

        // person.Age = 31;

        System.Console.WriteLine($"Olá {person.Name} - {person.Age}!");
    }
}

public class Person{
    public string Name { get; set; }
    public int Age { get; set; }
}