using Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press a key to Create Order");
            Console.ReadLine();

            MainAsync(args).GetAwaiter().GetResult();

            Console.ReadKey();
        }

        public static async Task MainAsync(string[] args)
        {
            var order = new OrderDto()
            {
                OrderId = 2323231,
                ProductId = 7679977,
                ProductDescription = "Audi A6",
                Qty = 1,
                Price = 19000
                
            };

            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(order));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                await client.PostAsync("http://localhost:41173/api/publish", httpContent);
            }
        }
    }
}
