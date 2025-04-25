using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void StartAnimation() => StartCoroutine(SpriteAnimationCoroutine());
    private IEnumerator SpriteAnimationCoroutine()
    {
        for (int i = 0; i < _sprites.Length; i++)
        {
            var sprite = _sprites[i];
            _spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(.1f);
        }
        _spriteRenderer.sprite = null;
        yield return new WaitForEndOfFrame();
    }
}