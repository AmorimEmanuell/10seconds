using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Button _playButton;

    private void Start()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
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
}
