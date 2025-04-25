using UnityEngine;
using UnityEngine.Events;

public class InvisibleEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent _onInvisible;
    [SerializeField] private UnityEvent _onVisible;

    private void OnBecameInvisible()
    {
        Debug.Log($"{transform.parent.name} saiu da tela");
        _onInvisible?.Invoke();
    }
    private void OnBecameVisible()
    {
        _onVisible?.Invoke();
    }
}
