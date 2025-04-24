using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour, IObserver<int>
{
    public TMP_Text scoreText;
    private void Start()
    {
        ScoreManager.Instance.Subscribe(this);
    }
    private void OnDestroy()
    {
        ScoreManager.Instance.Unsubscribe(this);
    }
    public void OnNotify(int newScore)
    {
        UpdateScoreDisplay(newScore);
    }
    private void UpdateScoreDisplay(int currentScore)
    {
        if (scoreText != null)
        {
            scoreText.text = "Pontos: " + currentScore;
        }
        else
        {
            Debug.LogWarning("Texto de pontuação não atribuído no ScoreManager!");
        }
    }
}