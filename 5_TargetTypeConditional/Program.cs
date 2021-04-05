using System;
public class Program{
    public static void Main(string[] args){
        var isStudent = !bool.Parse(args[0]);

        // Same for overrides (covariant returns)
        Person person = isStudent ? new Student{
            name = "Student",
            course = "Computer Science"
        } : new Teacher{
            name = "Teacher",
            subject = "Maths"
        };

        Console.WriteLine(person);

    }
}
public record Person (){
    //DataMember
    public string name { get; init; }
}
public record Student () : Person{    
    public string course { get; init; }
}
public record Teacher () : Person{
    public string subject { get; init; }
}

// abstract class Animal
// {
//     public abstract Food GetFood();
//     ...
// }
// class Tiger : Animal
// {
//     public override Meat GetFood() => ...;
// }
// https://devblogs.microsoft.com/dotnet/welcome-to-c-9-0/
