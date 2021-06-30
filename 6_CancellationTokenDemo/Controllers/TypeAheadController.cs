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
        public async Task<IEnumerable<string>> Get(string param, CancellationToken ct)
        {
            try{
                Console.WriteLine($"Searching for '{param}'...");
                //Ponto de cancelamento 1
                var text = await System.IO.File.ReadAllTextAsync("names.json");

                var jsonNames = JsonConvert.DeserializeObject<List<string>>(text);
                var items = jsonNames.Where(x => string.IsNullOrEmpty(param) || x.ToLower().StartsWith(param.ToLower())).ToList();
                
                var delay = (double)items.Count / 3;
                if(delay > 3)
                    delay = 3;
                
                //Ponto de cancelamento 2
                Console.WriteLine($"Waiting server query {param}... {delay + 1 }s");
                await Task.Delay(TimeSpan.FromSeconds(1));
                ct.ThrowIfCancellationRequested();

                //Ponto de não retorno
                Console.WriteLine("Atingido ponto de não retorno");
                await Task.Delay(TimeSpan.FromSeconds(delay), CancellationToken.None);
                Console.WriteLine($"Returning {param}\n");
                return items;
            }catch(OperationCanceledException ex)
            {
                Console.WriteLine($"Cancelled! {param}\n");
            }

            return null;
        }
    }
}
