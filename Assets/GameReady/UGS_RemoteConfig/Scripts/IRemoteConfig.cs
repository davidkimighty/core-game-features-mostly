using System.Threading.Tasks;

namespace GameReady.UGS
{
    public interface IRemoteConfig
    {
        Task FetchRemoteConfigAsync(userAttributes userAttributes = default,
            appAttributes appAttributes = default, filterAttributes filterAttributes = default);
        string GetString(string key);
        T GetDataFromJson<T>(string key);
    }
}
