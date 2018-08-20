using System.Threading.Tasks;

namespace MicroServiceOne
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
