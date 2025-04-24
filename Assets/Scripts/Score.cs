using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private int _score = 10;

    public void SetScore()
    {
        ScoreManager.Instance.AddScore(_score);
    }
}