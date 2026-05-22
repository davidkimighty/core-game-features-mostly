using BroCollie.SceneLoad;
using BroCollie.Util;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class GameInitializer : MonoBehaviour
{
    [SerializeField] private ScreenFader _screenFaderPrefab;
    
    private void Awake()
    {
        if (!ServiceLocator.Contains<SceneLoader>())
            ServiceLocator.Register(new SceneLoader());

        if (!ServiceLocator.Contains<ScreenFader>())
        {
            ScreenFader fader = Instantiate(_screenFaderPrefab);
            DontDestroyOnLoad(fader);
            ServiceLocator.Register(fader);
        }
    }
}
