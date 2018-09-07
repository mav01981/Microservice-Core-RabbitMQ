using System.Diagnostics;

namespace MicroserviceTwoConsumer.Services
{
    public class Service:IService
    {
        public void RemoveStock<T>(T message)
        {
            Debug.WriteLine($"{message}");
        }
    }
}
