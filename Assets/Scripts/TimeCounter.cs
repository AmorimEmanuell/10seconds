using System;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    private const float _desiredTime = 10f;
    private static float _sectionTime;

    public static Action OnTimeReached;

    public static float SectionPercentage => _sectionTime / _desiredTime;
    public static float TotalElapsedTime { get; private set; }

    private void Update()
    {
        TotalElapsedTime += Time.deltaTime;

        _sectionTime += Time.deltaTime;
        _sectionTime = Mathf.Clamp(_sectionTime, 0, _desiredTime);

        if (_sectionTime >= _desiredTime)
        {
            _sectionTime = 0;
            OnTimeReached?.Invoke();
        }
    }
}
