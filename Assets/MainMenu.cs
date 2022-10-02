using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Button _playButton;
    [SerializeField] private GameObject _finalScorePanel;
    [SerializeField] private TextMeshProUGUI _finalScoreText;

    private void Start()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        GameEvents.OnGameEnded += EndGame;
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveAllListeners();
    }

    private void OnPlayButtonClicked()
    {
        _mainPanel.SetActive(false);
        GameEvents.RaiseOnGameStart();
    }

    private void EndGame()
    {
        _mainPanel.SetActive(true);

        _finalScorePanel.SetActive(true);
        _finalScoreText.SetText("{0}", ScoreCounter.CurrentScore);
    }
}
