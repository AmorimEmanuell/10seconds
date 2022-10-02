using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private GameObject _scorePanel;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private readonly string _scoreFormat = "{0}";

    public static int CurrentScore { get; private set; }

    private void Start()
    {
        GameEvents.OnGameStart += StartGame;
        GameEvents.OnGameEnded += EndGame;
        TimeCounter.OnTimeReached += IncreaseScore;
    }

    private void StartGame()
    {
        CurrentScore = 0;
        _scoreText.SetText(_scoreFormat, CurrentScore);

        _scorePanel.SetActive(true);
    }

    private void EndGame()
    {
        _scorePanel.SetActive(false);
    }

    private void IncreaseScore()
    {
        CurrentScore += 100;
        _scoreText.SetText(_scoreFormat, CurrentScore);
    }
}
