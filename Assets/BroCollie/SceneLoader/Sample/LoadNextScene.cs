using BroCollie.SceneLoad;
using BroCollie.Util;
using UnityEngine;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] private string _nextScene;

    private void Start()
    {
        _ = ServiceLocator.Get<ScreenFader>().FadeAsync(0);
    }

    public async void LoadScene()
    {
        await ServiceLocator.Get<ScreenFader>().FadeAsync(1);
        await ServiceLocator.Get<SceneLoader>().LoadNewSceneAsync(_nextScene);
    }

    public async void LoadSeneAdditive()
    {
        await ServiceLocator.Get<ScreenFader>().FadeAsync(1);
        await ServiceLocator.Get<SceneLoader>().LoadNewSceneAdditiveAsync(_nextScene);
    }
}
