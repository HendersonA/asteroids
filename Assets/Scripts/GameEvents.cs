using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    [System.Serializable]
    public class GameEventType
    {
        [HideInInspector] public string Name;
        public GameEvent EventType;
        public UnityEvent Events;
    }
    [SerializeField] private GameEventType[] gameEvents;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            foreach (GameEventType gameEvent in gameEvents)
                GameManager.GameEvents[gameEvent.EventType.GetHashCode()] += gameEvent.Events.Invoke;
        }
    }
    private void OnDestroy()
    {
        foreach (GameEventType gameEvent in gameEvents)
            GameManager.GameEvents[gameEvent.EventType.GetHashCode()] -= gameEvent.Events.Invoke;
    }
    public void GameStartEvent()
    {
        GameManager.GameEvents[GameEvent.GameStart.GetHashCode()]?.Invoke();
    }
    private void OnValidate()
    {
        foreach (GameEventType gameEventType in gameEvents)
            gameEventType.Name = gameEventType.EventType.ToString();
    }
}