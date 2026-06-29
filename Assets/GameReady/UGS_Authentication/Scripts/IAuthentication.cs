using System.Threading.Tasks;

namespace GameReady.UGS
{
    public interface IAuthentication
    {
        Task InitializeUnityServicesAsync(string envName);
        Task SignInAsync();
    }
}