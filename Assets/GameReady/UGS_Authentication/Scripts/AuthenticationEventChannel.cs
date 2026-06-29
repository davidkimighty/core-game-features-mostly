using System;
using Unity.Services.Authentication;
using UnityEngine;

namespace GameReady.UGS
{
    [CreateAssetMenu(fileName = "AuthenticationEventChannel", menuName = "UGS/EventChannel/Authentication")]
    public class AuthenticationEventChannel : ScriptableObject
    {
        public event Action<PlayerInfo> OnSignInSuccess;
        public event Action OnSignInFail;

        public void RaiseOnSignInSuccess(PlayerInfo playerInfo)
        {
            OnSignInSuccess?.Invoke(playerInfo);
        }

        public void RaiseOnSignInFail()
        {
            OnSignInFail?.Invoke();
        }
    }
}
