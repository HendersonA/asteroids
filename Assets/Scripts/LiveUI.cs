using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiveUI : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _contentTransform;
    [SerializeField] private Color _deathColor;
    private List<GameObject> _liveIcons = new List<GameObject>();
    private Health _playerHealth;
    private void Start()
    {
        _playerHealth = GameManager.PlayerTransform.GetComponent<Health>();
        SetLiveUI(_playerHealth.MaxLives);
        _playerHealth.OnTakeDamage.AddListener((value)=>UpdateLiveUI());
    }
    private void SetLiveUI(int amount)
    {
        for (int i = 1; i < amount; i++)
        {
            var liveIcon = Instantiate(_prefab, _contentTransform);
            _liveIcons.Add(liveIcon);
        }
    }
    private void UpdateLiveUI()
    {
        var iconRenderer = _liveIcons[_playerHealth._CurrentLive].GetComponent<Image>();
        iconRenderer.color = _deathColor;
    }
}