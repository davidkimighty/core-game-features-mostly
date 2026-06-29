using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace GameReady
{
    public class PunchHoleTransition : Transition
    {
        private static readonly int HoleSizeId = Shader.PropertyToID("_HoleSize");

        [SerializeField] private Image _holeImage;

        private Material _mat;

        private void Awake()
        {
            _mat = new Material(_holeImage.material);
            _mat.SetColor("_Color", _holeImage.color);
            _holeImage.material = _mat;
            _canvas.enabled = true;
        }

        public override async Awaitable OpenAsync(CancellationToken ct = default)
        {
            await AnimateAsync(0f, 1f, ct);
        }

        public override async Awaitable CloseAsync(CancellationToken ct = default)
        {
            await AnimateAsync(1f, 0f, ct);
        }

        private async Awaitable AnimateAsync(float from, float to, CancellationToken ct)
        {
            float elapsed = 0f;

            while (elapsed < _duration)
            {
                ct.ThrowIfCancellationRequested();

                float t = _curve.Evaluate(elapsed / _duration);
                float size = Mathf.LerpUnclamped(from, to, t);
                _mat.SetFloat(HoleSizeId, size);

                await Awaitable.NextFrameAsync(ct);
                elapsed += Time.deltaTime;
            }
            _mat.SetFloat(HoleSizeId, to);
        }
    }
}
