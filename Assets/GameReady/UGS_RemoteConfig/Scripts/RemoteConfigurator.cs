using System;
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace GameReady.UGS
{
    public class RemoteConfigurator : MonoBehaviour, IRemoteConfig
    {
        public async Task FetchRemoteConfigAsync(userAttributes userAttributes = default,
            appAttributes appAttributes = default, filterAttributes filterAttributes = default)
        {
            await RemoteConfigService.Instance.FetchConfigsAsync(userAttributes, appAttributes, filterAttributes);
        }

        public string GetString(string key)
        {
            try
            {
                return RemoteConfigService.Instance.appConfig.GetString(key);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            return null;
        }

        public T GetDataFromJson<T>(string key)
        {
            try
            {
                string json = RemoteConfigService.Instance.appConfig.GetJson(key);
                if (string.IsNullOrEmpty(json)) return default;

                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            return default;
        }
    }

    public struct userAttributes
    {

    }

    public struct appAttributes
    {

    }

    public struct filterAttributes
    {
        public string[] key;
        public string[] type;
        public string[] schemaId;
    }
}
