using System;

namespace _8_LogicalPatternsHandling
{
    class Program
    {
        static void Main(string[] args)
        {
            //Logical Patterns handling
            object result = args switch {
                not null => 
                            args[0] switch{
                                "invalid" => throw new ArgumentException($"Invalid value for {nameof(args)}"),
                                null => throw new ArgumentNullException(nameof(args)),
                                _ => "valid!"
                            },
                _ => throw new ArgumentNullException(nameof(args)),
            };
            Console.WriteLine(result);
        }
    }
}
