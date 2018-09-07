namespace MicroserviceTwoConsumer.Services
{
    public interface IService
    {
        void RemoveStock<T>(T message);
    }
}