using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace GameReady.UGS
{
    public class Authenticator : MonoBehaviour, IAuthentication
    {
        [SerializeField] private AuthenticationEventChannel _authenticationEventChannel;

        private InitializationOptions _initializationOptions;
        private IAuthenticationService _authenticationService;

        public async Task InitializeUnityServicesAsync(string envName)
        {
            if (UnityServices.State == ServicesInitializationState.Initialized) return;

            _initializationOptions = new InitializationOptions();
            _initializationOptions.SetEnvironmentName(envName);
            await UnityServices.InitializeAsync(_initializationOptions);

            _authenticationService = AuthenticationService.Instance;
        }

        public async Task SignInAsync()
        {
            try
            {
                if (!_authenticationService.IsSignedIn)
                {
                    await _authenticationService.SignInAnonymouslyAsync();

                    if (_authenticationService.IsSignedIn)
                    {
                        _authenticationEventChannel.RaiseOnSignInSuccess(_authenticationService.PlayerInfo);
                        //Debug.Log($"PlayerID: {_authenticationService.PlayerId}");
                    }
                    else
                    {
                        _authenticationEventChannel.RaiseOnSignInFail();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Authenticator] Sign-in failed: {ex.Message}");
            }
        }
    }
}