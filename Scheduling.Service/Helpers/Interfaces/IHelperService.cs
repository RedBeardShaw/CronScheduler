using System.Threading.Tasks;

namespace Scheduling.Service.Helpers.Interfaces
{
    public interface IHelperService
    {
        Task PerformTimerActivity(string schedule);
    }
}
