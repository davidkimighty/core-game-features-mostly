using BroCollie.SceneLoad;
using BroCollie.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-10000)]
public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private ScreenFader _screenFader;

    private void Awake()
    {
        ServiceLocator.Register(new SceneLoader());
        ServiceLocator.Register(_screenFader);

    }

    private void Start()
    {
        if (SceneManager.loadedSceneCount == 1)
            _ = ServiceLocator.Get<SceneLoader>().LoadSceneAdditiveAsync(_sceneName);
    }
}
