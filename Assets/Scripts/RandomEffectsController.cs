using System.Collections;
using UnityEngine;

public class RandomEffectsController : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _angle;
    [SerializeField] private float _rotationDuration;

    private float _cameraAngle = 0f;

    private void Start()
    {
        GameEvents.OnGameEnded += EndGame;
        TimeCounter.OnTimeReached += RandomlyApplyEffect;
    }

    private void EndGame()
    {
        _camera.transform.rotation = Quaternion.identity;
    }

    private void RandomlyApplyEffect()
    {
        // This inverts the buttons being pressed
        // if ((Random.Range(0f, 1f) * 10) > 5)
        // {
        //     _isVerticalInverted = !_isVerticalInverted;
        // } 
        // else
        // { 
        //     _isHorizontalInverted = !_isHorizontalInverted;
        // }

        // This rotates the camera
        _cameraAngle -= _angle;
        StartCoroutine(RotateCameraRoutine(_rotationDuration));
    }

    private IEnumerator RotateCameraRoutine(float duration)
    {
        var finalRotation = Quaternion.Euler(new Vector3(0, 0, _cameraAngle));
        var originalRotation = _camera.rotation;
        var totalRotated = 0f;
        var timeElapsed = 0f;
        while (totalRotated < 1)
        {
            totalRotated = timeElapsed / duration;
            var next = Quaternion.Lerp(originalRotation, finalRotation, totalRotated);
            _camera.rotation = next;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
