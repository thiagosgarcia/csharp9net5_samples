using System;
public class Program{
    public static void Main(string[] args){
        var name = args[0];
        var age = int.Parse(args[1]);
        // var person = new Person(name, age);
        var person = new Person(){
            Name = name,
            Age = age
        };

        // System.Console.WriteLine($"Olá {person.Name} - {person.Age}!");
        Console.Write("Class -> ");
        Console.WriteLine(person);

        var recordPerson = new RecordPerson(name, age);
        Console.Write("Record -> ");
        Console.WriteLine(recordPerson);
    }
}

public record RecordPerson (
        string Name,
        int Age
    );


public class Person{
    public string Name { get; set; }
    public int Age { get; set; }

    // public Person(string name, int age)
    // {
    //     Name = name;
    //     Age = age;
    // }
    // public string Name { get; private set; }
    // public int Age { get; private set; }

    // public override string ToString(){
    //     return $"Person {{ Name = {Name}, Age = {Age} }}";
    // }

    // public override bool Equals(object obj)
    // {
    //     if (obj == null || GetType() != obj.GetType())
    //     {
    //         return false;
    //     }
    //     return base.Equals (obj);
    // }
    
    // // override object.GetHashCode
    // public override int GetHashCode()
    // {
    //     return base.GetHashCode();
    // }
}