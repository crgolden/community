using System.Threading.Tasks;

namespace Community.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
