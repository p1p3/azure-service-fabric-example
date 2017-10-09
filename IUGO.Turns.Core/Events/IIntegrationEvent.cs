using System.Threading.Tasks;

namespace IUGO.Turns.Core.Events
{
    public interface IIntegrationEvent
    {
        Task Notify();
    }
}