using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BroCollie.SceneLoad
{
    public class SceneLoader
    {
        public event Action OnSceneLoadStart;
        public event Action<float> OnSceneLoadProgress;
        public event Action OnSceneLoadComplete;

        public async Awaitable LoadNewSceneAsync(string sceneName, CancellationToken cancellationToken = default)
        {
            OnSceneLoadStart?.Invoke();
            try
            {
                if (cancellationToken != default)
                    cancellationToken.ThrowIfCancellationRequested();

                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
                while (!loadOperation.isDone)
                {
                    if (cancellationToken != default && cancellationToken.IsCancellationRequested) break;

                    float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                    OnSceneLoadProgress?.Invoke(progress);

                    await Awaitable.NextFrameAsync(cancellationToken);
                }
                OnSceneLoadComplete?.Invoke();
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Debug.LogError($"Scene load operation failed: {ex.Message}");
            }
        }

        public async Awaitable LoadNewSceneAdditiveAsync(string sceneName, CancellationToken cancellationToken = default)
        {
            OnSceneLoadStart?.Invoke();
            string activeScene = SceneManager.GetActiveScene().name;
            await UnloadSceneAsync(activeScene, cancellationToken);
            OnSceneLoadProgress?.Invoke(0.5f);

            await LoadSceneAdditiveAsync(sceneName, cancellationToken);
            OnSceneLoadProgress?.Invoke(1f);
            OnSceneLoadComplete?.Invoke();
        }

        public async Awaitable UnloadSceneAsync(string sceneName, CancellationToken cancellationToken = default)
        {
            try
            {
                if (cancellationToken != default)
                    cancellationToken.ThrowIfCancellationRequested();

                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneName);
                while (!unloadOperation.isDone)
                {
                    if (cancellationToken != default && cancellationToken.IsCancellationRequested) break;

                    await Awaitable.NextFrameAsync(cancellationToken);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Debug.LogError($"Scene unload operation failed: {ex.Message}");
            }
        }

        public async Awaitable LoadSceneAdditiveAsync(string sceneName, CancellationToken cancellationToken = default)
        {
            try
            {
                if (cancellationToken != default)
                    cancellationToken.ThrowIfCancellationRequested();

                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                loadOperation.allowSceneActivation = true;

                while (!loadOperation.isDone)
                {
                    if (cancellationToken != default && cancellationToken.IsCancellationRequested) break;

                    await Awaitable.NextFrameAsync(cancellationToken);
                }

                Scene newScene = SceneManager.GetSceneByName(sceneName);
                if (newScene.IsValid())
                    SceneManager.SetActiveScene(newScene);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Debug.LogError($"Scene load operation failed: {ex.Message}");
            }
        }
    }
}