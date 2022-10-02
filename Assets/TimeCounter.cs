using System;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    private const float _desiredTime = 10f;
    private static float _elapsedTime;

    public static Action OnTimeReached;

    public static float ElapsedPercentage => _elapsedTime / _desiredTime;

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        _elapsedTime = Mathf.Clamp(_elapsedTime, 0, _desiredTime);

        if (_elapsedTime >= _desiredTime)
        {
            _elapsedTime = 0;
            OnTimeReached?.Invoke();
        }
    }
}
