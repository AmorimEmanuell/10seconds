using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private GameObject _scorePanel;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private readonly string _scoreFormat = "{0}";

    private float _elapsedTime = 0;

    public static int CurrentScore { get; private set; }

    private void Start()
    {
        GameEvents.OnGameStart += StartGame;
        GameEvents.OnGameEnded += EndGame;
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

    private void Update()
    {
        if (_scorePanel.activeInHierarchy)
        {
            _elapsedTime -= Time.deltaTime;

            if (_elapsedTime <= 0) {
                IncreaseScore();
                _elapsedTime = 0.1f;
            }
        }
    }

    private void IncreaseScore()
    {
        CurrentScore += 1;
        _scoreText.SetText(_scoreFormat, CurrentScore);
    }
}
