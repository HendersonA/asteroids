public class ScoreManager : Subject<int>
{
    public static ScoreManager Instance;
    private int currentScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddScore(int points)
    {
        currentScore += points;
        NotifyObservers(currentScore);
    }
    protected override int GetState()
    {
        return currentScore;
    }
}