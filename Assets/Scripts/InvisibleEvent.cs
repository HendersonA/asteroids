using UnityEngine;
using UnityEngine.Events;

namespace Asteroids
{
    public class InvisibleEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onInvisible;
        [SerializeField] private UnityEvent _onVisible;

        private void OnBecameInvisible()
        {
            if(GameManager.MainCamera)
            _onInvisible?.Invoke();
        }
        private void OnBecameVisible()
        {
            if (GameManager.MainCamera)
                _onVisible?.Invoke();
        }
    }
}