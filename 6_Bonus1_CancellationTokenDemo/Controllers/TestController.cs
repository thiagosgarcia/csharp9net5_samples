using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _6_Bonus1_CancellationTokenDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("SyncMethod")]
        public void SyncMethod(){
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"{i + 1} seconds passed...");
            }
            Console.WriteLine($"Finished. Returning...");
        }

        [HttpGet("AsyncMethod")]
        public async Task AsyncMethod(){
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                Console.WriteLine($"{i + 1} seconds passed...");
            }
            Console.WriteLine($"Finished. Returning...");
        }

        [HttpGet("Example1")]
        public async Task Example1(CancellationToken cancellationToken){
            for (int i = 0; i < 10; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(1000);
                Console.WriteLine($"{i + 1} seconds passed...");
            }
            Console.WriteLine($"Finished. Returning...");
        }

        [HttpGet("Example2/{param1}")]
        public async Task Example2(string param1, [FromQuery]int param2 = 10, CancellationToken cancellationToken = default){
            for (int i = 0; i < param2; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(1000);
                Console.WriteLine($"{i + 1} seconds passed... Params: {param1}");
            }
            Console.WriteLine($"Finished. Returning...");
        }

        [HttpGet("Example3")]
        public async Task<string> Example3(CancellationToken cancellationToken){
            
            var client = new HttpClient();
            Console.WriteLine("Calling a slow service...");
            var result = await client.GetStringAsync("https://localhost:5001/Slow/", cancellationToken);
            return result;
        }

        [HttpGet("Example4")]
        public async Task<string> Example4(CancellationToken cancellationToken){
            
            var client = new HttpClient();
            Console.WriteLine("Calling a slow service...");
            var result = await client.GetStringAsync("https://localhost:5001/Slow/Fixed", cancellationToken);
            return result;
        }

        [HttpGet("Example5")]
        public async Task<string> TimeoutPropagation(CancellationToken cancellationToken){ //Inherited CT
            
            var client = new HttpClient();
            Console.WriteLine("Calling a slow service...");
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(6)); //Timeout CT
            var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken); //Linking both
            var resultTask = client.GetStringAsync("https://localhost:5001/Slow/Fixed", linkedToken.Token);
            
            if(!Task.WaitAll(new Task[]{ resultTask }, 4000, linkedToken.Token)) //Example of how to cancel both tokens regardless other conditions
            {
                Console.WriteLine("Timeout!");
                linkedToken.Cancel();
            }

            return await resultTask;
        }

        [HandleTimeout]
        [HttpGet("Example6")]
        public async Task<string> ExceptionHandlerAndTimeoutPropagation(CancellationToken cancellationToken){ //Inherited CT
            
            var client = new HttpClient();
            Console.WriteLine("Calling a slow service...");
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(6)); //Timeout CT
            var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken); //Linking both
            var resultTask = client.GetStringAsync("https://localhost:5001/Slow/Fixed", linkedToken.Token);
            
            // if(!Task.WaitAll(new Task[]{ resultTask }, 1000, linkedToken.Token)) //Example of how to cancel both tokens regardless other conditions
            // {
            //     Console.WriteLine("Timeout!");
            //     linkedToken.Cancel();
            // }

            return await resultTask;
        }
    }
    [ApiController]
    [Route("[controller]")]
    public class SlowController : ControllerBase
    {
        [HttpGet]
        public async Task<string> SlowMethod(){
            try{
                Console.WriteLine("processing data...");
                await Task.Delay(10000);
                Console.WriteLine("ok!");
                return "ok!";
            }
            catch(Exception ex)
            {
                //This will never happen, because we don't have a Cancellation Token
                Console.WriteLine($"Slow method response: {ex.Message}");
                return "nok";
            }
        }
        [HttpGet("Fixed")]
        public async Task<string> FixedSlowMethod(CancellationToken cancellationToken){
            try{
                Console.WriteLine("processing data...");
                await Task.Delay(10000, cancellationToken);
                Console.WriteLine("ok!");
                return "ok!";
            }
            catch(OperationCanceledException ex){
                //If the browser is abandoned, page is changed, timeout is raised, or anything else goes worng with the caller, this will happen
                Console.WriteLine($"Slow method response: {ex.Message}");
                //This is where you can rollback, dispose or do anything to handle a cancellation
                Console.WriteLine($"If needed, I can rollback anything from here");
                return "nok";
            }catch{
                return "nok";
            }
        }
    }
    
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