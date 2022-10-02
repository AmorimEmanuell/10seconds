using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _currentScore;
    private string _scoreFormat = "{0}";

    private void Start()
    {
        TimeCounter.OnTimeReached += IncreaseScore;
    }

    private void IncreaseScore()
    {
        _currentScore += 100;
        _scoreText.SetText(_scoreFormat, _currentScore);
    }
}
