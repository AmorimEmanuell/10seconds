using System.Collections;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float _rotateDuration;
    [SerializeField] private AnimationCurve _moveCurve;
    [SerializeField] private AnimationCurve _rotationCurve;

    private Vector2Int _currentGridPosition = Vector2Int.zero;
    private float _moveDuration = 0.2f;
    private Coroutine _moveRoutine, _rotateRoutine, _stabilizeRoutine;

    private void Update()
    {
        var leftKey = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        var rightKey = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        var upKey = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        var downKey = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);

        var horizontal = 0;
        if (leftKey)
        {
            horizontal--;
        }

        if (rightKey)
        {
            horizontal++;
        }

        var vertical = 0;
        if (upKey)
        {
            vertical++;
        }

        if (downKey)
        {
            vertical--;
        }

        if (horizontal == 0 && vertical == 0)
        {
            return;
        }

        var nextX = Mathf.Clamp(_currentGridPosition.x + horizontal, -2, 2);
        var nextY = Mathf.Clamp(_currentGridPosition.y + vertical, -2, 2);
        var nextGridPosition = new Vector2Int(nextX, nextY);
        if (_currentGridPosition == nextGridPosition)
        {
            return;
        }

        var delta = nextGridPosition - _currentGridPosition;
        _currentGridPosition = nextGridPosition;

        StopAllCoroutines();

        _moveRoutine = StartCoroutine(MoveRoutine(_currentGridPosition));
        _rotateRoutine = StartCoroutine(RotateRoutine(delta, _rotateDuration));
    }

    private IEnumerator MoveRoutine(Vector2 gridPosition)
    {
        var finalPosition = new Vector2(gridPosition.x * 1.5f, gridPosition.y * 1.5f);
        var originalPosition = transform.position;
        var totalTraveled = 0f;
        var timeElapsed = 0f;
        while (totalTraveled < 1)
        {
            totalTraveled = timeElapsed / _moveDuration;
            var next = Vector3.Lerp(originalPosition, finalPosition, _moveCurve.Evaluate(totalTraveled));
            transform.position = next;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RotateRoutine(Vector2 deltaMovement, float duration)
    {
        var zAngle = deltaMovement.x * 45f;
        var xAngle = deltaMovement.y * 45f;
        var finalRotation = Quaternion.Euler(new Vector3(-xAngle, 0, -zAngle));
        var originalRotation = transform.rotation;
        var totalRotated = 0f;
        var timeElapsed = 0f;
        while (totalRotated < 1)
        {
            totalRotated = timeElapsed / duration;
            var next = Quaternion.Lerp(originalRotation, finalRotation, _rotationCurve.Evaluate(totalRotated));
            transform.rotation = next;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        finalRotation = Quaternion.identity;
        originalRotation = transform.rotation;
        timeElapsed = 0f;
        totalRotated = 0f;
        while (totalRotated < 1)
        {
            totalRotated = timeElapsed / duration;
            var next = Quaternion.Lerp(originalRotation, finalRotation, totalRotated);
            transform.rotation = next;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
