using UnityEngine;
using UnityEngine.UI;

public class TimeCounterFeedback : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private void Update()
    {
        _slider.value = TimeCounter.ElapsedPercentage;
    }
}
