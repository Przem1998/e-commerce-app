using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ILogRepository
    {
        Task InitLog(string log);
    }
}