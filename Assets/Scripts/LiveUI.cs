using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LiveUI : MonoBehaviour
{
    [SerializeField] private UnityEvent _onGameOver;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _contentTransform;
    [SerializeField] private Color _deathColor;
    private List<GameObject> _liveIcons;
    private Health _playerHealth;
    private void Start()
    {
        _liveIcons = new List<GameObject>();
        _playerHealth = GameManager.PlayerTransform.GetComponent<Health>();
        SetLiveUI(_playerHealth.MaxLives);
        _playerHealth.OnTakeDamage.AddListener(UpdateLiveUI);
    }
    private void SetLiveUI(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var liveIcon = Instantiate(_prefab, _contentTransform);
            _liveIcons.Add(liveIcon);
        }
    }
    private void UpdateLiveUI()
    {
        var iconRenderer = _liveIcons[_playerHealth._CurrentLive].GetComponent<Image>();
        iconRenderer.color = _deathColor;
        if (_playerHealth.IsDead)
        {
            _onGameOver?.Invoke();
            return;
        }
    }
}