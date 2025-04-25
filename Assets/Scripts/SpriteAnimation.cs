using System;
using System.Collections;
using UnityEngine;

namespace Asteroids
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        public event Action OnComplete;
        [SerializeField] private Sprite[] _sprites;
        private SpriteRenderer _spriteRenderer;
        private Sprite _cachedSprite = null;
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void StartAnimation() => StartCoroutine(SpriteAnimationCoroutine());
        private IEnumerator SpriteAnimationCoroutine()
        {
            _cachedSprite = _spriteRenderer.sprite;
            for (int i = 0; i < _sprites.Length; i++)
            {
                var sprite = _sprites[i];
                _spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(.1f);
            }
            _spriteRenderer.sprite = _cachedSprite;
            yield return new WaitForEndOfFrame();
            OnComplete?.Invoke();
        }
    }
}