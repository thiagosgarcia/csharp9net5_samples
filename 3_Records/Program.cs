using System;
public class Program{
    public static void Main(string[] args){
        var name = args[0];
        var age = int.Parse(args[1]);
        // var person = new Person(name, age);
        var person = new Person(name, age);
        var person2 = new Person("Pessoa2", age);

        // System.Console.WriteLine($"Olá {person.Name} - {person.Age}!");
        Console.Write("Class -> ");
        Console.WriteLine(person);
        Console.WriteLine(person == person2);

        var recordPerson = new RecordPerson(name, age);
        var recordPerson2 = recordPerson with {};
        Console.Write("Record -> ");
        Console.WriteLine(recordPerson);
        Console.WriteLine(recordPerson2);
        Console.WriteLine(Equals(recordPerson, recordPerson2));
        Console.WriteLine(ReferenceEquals(recordPerson, recordPerson2));


    }
}

public record RecordPerson (string Name, int Age);


public class Person {
    public string Name { get; private set; }
    public int Age { get; private set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public override string ToString(){
        return $"Person {{ Name = {Name}, Age = {Age} }}";
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        if(obj is Person person){
            return person.Name == this.Name && person.Age == this.Age;
        }

        return base.Equals (obj);
    }

    public static bool operator ==(Person x, Person y){
        return x.Equals(y);
    }
    public static bool operator !=(Person x, Person y){
        return !x.Equals(y);
    }
    
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}