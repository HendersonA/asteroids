using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text scoreText;
    private void Start()
    {
        ScoreManager.Instance.OnScore += UpdateScoreDisplay;
    }
    private void OnDestroy()
    {
        ScoreManager.Instance.OnScore -= UpdateScoreDisplay;
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