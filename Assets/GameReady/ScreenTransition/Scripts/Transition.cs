using System.Threading;
using UnityEngine;

namespace GameReady
{
    public abstract class Transition : MonoBehaviour
    {
        [SerializeField] protected Canvas _canvas;
        [SerializeField] protected float _duration = 1f;
        [SerializeField] protected AnimationCurve _curve;

        public virtual async Awaitable OpenAsync(CancellationToken ct = default)
        {
            _canvas.enabled = true;
        }

        public virtual async Awaitable CloseAsync(CancellationToken ct = default)
        {
            _canvas.enabled = false;
        }
    }
}
