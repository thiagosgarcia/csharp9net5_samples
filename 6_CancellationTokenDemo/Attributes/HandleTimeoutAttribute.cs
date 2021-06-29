using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _6_CancellationTokenDemo.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HandleTimeoutAttribute : TypeFilterAttribute
    {
        public HandleTimeoutAttribute() : base(typeof(HandleTimeoutAttributeImpl))
        {}
        public HandleTimeoutAttribute(Type t) : base(t)
        {}

        public class HandleTimeoutAttributeImpl : ExceptionFilterAttribute
        {
            //Using this TypeFilter pattern, you can inject dependencies on attributes...
            // private readonly ILogger _logger;

            public HandleTimeoutAttributeImpl()
            {}

            //...and handle whatever you need.
            public override void OnException(ExceptionContext context)
            {
                Console.WriteLine("Handling exception...");
                if(context.Exception is OperationCanceledException)
                    context.Result = new ObjectResult(null)
                            {
                                StatusCode = 408,
                                Value = "Request cancelled. Please, try again"
                            };
                
            }
        }
    }
}