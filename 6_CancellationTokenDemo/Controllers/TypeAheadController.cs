using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace _6_CancellationTokenDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TypeAheadController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<string>> Get(string param, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine($"Searching for '{param}'...");
                var text = await System.IO.File.ReadAllTextAsync("names.json", cancellationToken);

                var jsonNames = JsonConvert.DeserializeObject<List<string>>(text);
                var items = jsonNames.Where(x => string.IsNullOrEmpty(param) || x.ToLower().StartsWith(param.ToLower())).ToList();

                var delay = (double)items.Count / 3;
                if(delay > 5)
                    delay = 5;

                Console.WriteLine($"Waiting server query {param}... {delay}s");
                await Task.Delay(TimeSpan.FromSeconds(delay), cancellationToken);
                Console.WriteLine($"Returning {param}\n");
                return items;

            } catch(OperationCanceledException)
            {
                Console.WriteLine($"CANCELLED! =)\n");
            }
            return new List<string>();
        }
    }
}
