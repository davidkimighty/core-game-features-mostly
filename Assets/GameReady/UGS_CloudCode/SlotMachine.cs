using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private async void Awake()
    {
        try
        {
            var options = new InitializationOptions();
            options.SetEnvironmentName("development");
            await UnityServices.InitializeAsync(options);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            SpinResult result = await CloudCodeService.Instance.CallModuleEndpointAsync<SpinResult>("SlotMachineModule", "Spin");
            _text.text = $"[ {string.Join(", ", result.spins)} ]";
        }
        catch (CloudCodeException exception)
        {
            Debug.LogException(exception);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Authenticator] Sign-in failed: {ex.Message}");
        }
    }
}

public class SpinResult
{
    public List<string> spins;
}