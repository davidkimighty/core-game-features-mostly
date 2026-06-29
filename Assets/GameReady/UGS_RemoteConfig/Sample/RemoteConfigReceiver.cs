using GameReady.UGS;
using System;
using TMPro;
using UnityEngine;

public enum UnityServiceEnv { production, development }

public class RemoteConfigReceiver : MonoBehaviour
{
    [SerializeField] private UnityServiceEnv _serviceEnv = UnityServiceEnv.production;
    [SerializeField] private Authenticator _authenticator;
    [SerializeField] private RemoteConfigurator _remoteConfigurator;

    [SerializeField] private TextMeshProUGUI _versionText;

    private async void Awake()
    {
        try
        {
            await _authenticator.InitializeUnityServicesAsync(_serviceEnv.ToString());
            await _authenticator.SignInAsync();

            await _remoteConfigurator.FetchRemoteConfigAsync();

            string version = _remoteConfigurator.GetString("version");
            _versionText.text = $"v{version}";
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to initialize Unity Services: {ex.Message}");
        }
    }
}
