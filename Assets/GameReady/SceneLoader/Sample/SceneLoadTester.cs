using GameReady.SceneLoad;
using UnityEngine;

public class SceneLoadTester : MonoBehaviour
{
    [SerializeField] private string _loadSceneName;

    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _sceneLoader = new SceneLoader();
    }

    [ContextMenu("Load Next")]
    public void LoadNext()
    {
        _ = _sceneLoader.LoadNewSceneAsync(_loadSceneName);
    }
}
