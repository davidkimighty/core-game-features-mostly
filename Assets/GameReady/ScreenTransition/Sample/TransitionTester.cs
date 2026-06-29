using GameReady;
using UnityEngine;

public class TransitionTester : MonoBehaviour
{
    [SerializeField] private PunchHoleTransition _transition;

    private void Start()
    {
        Open();
    }

    [ContextMenu("Open")]
    public void Open() => _transition.OpenAsync();

    [ContextMenu("Close")]
    public void Close() => _transition.CloseAsync();
}
