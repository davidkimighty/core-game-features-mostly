using System;
using System.Threading;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration = 1f;

    public async Awaitable FadeAsync(float alpha, CancellationToken cancellationToken = default)
    {
        if (cancellationToken == default)
            cancellationToken = destroyCancellationToken;

        float elapsedTime = 0f;
        float startAlpha = _canvasGroup.alpha;
        try
        {
            while (elapsedTime < _fadeDuration)
            {
                cancellationToken.ThrowIfCancellationRequested();

                elapsedTime += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, alpha, Mathf.Clamp01(elapsedTime / _fadeDuration));

                await Awaitable.NextFrameAsync(cancellationToken);
            }
            _canvasGroup.alpha = alpha;
        }
        catch (OperationCanceledException)
        {
            Debug.Log($"[ScreenFader] Screen fade canceled.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[ScreenFader] Screen fade failed. Message: {ex.Message}");
        }
    }
}
