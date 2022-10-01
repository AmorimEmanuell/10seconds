using System;
using System.Collections;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float _rotateDuration;

    private Vector2 _currentGridPosition = Vector2.zero;
    private Vector2 Grid = new Vector2(5, 5);
    private float _moveDuration = 0.5f;
    private Coroutine _moveRoutine, _rotateRoutine, _stabilizeRoutine;
    private bool _isMoving = false, _isStabilizing;

    private void Update()
    {
        var a = Input.GetKeyDown(KeyCode.A);
        var d = Input.GetKeyDown(KeyCode.D);
        var w = Input.GetKeyDown(KeyCode.W);
        var s = Input.GetKeyDown(KeyCode.S);

        var horizontal = 0;
        if (a)
        {
            horizontal--;
        }

        if (d)
        {
            horizontal++;
        }

        var vertical = 0;
        if (w)
        {
            vertical++;
        }

        if (s)
        {
            vertical--;
        }

        if (horizontal == 0 && vertical == 0)
        {
            return;
        }

        _isStabilizing = false;
        _isMoving = false;
        StopAllCoroutines();

        var deltaMovement = new Vector2(horizontal, vertical);
        _currentGridPosition += deltaMovement;

        _moveRoutine = StartCoroutine(MoveRoutine(_currentGridPosition));
        _rotateRoutine = StartCoroutine(RotateRoutine(deltaMovement, _rotateDuration));
    }

    private IEnumerator MoveRoutine(Vector2 gridPosition)
    {
        _isMoving = true;
        var finalPosition = new Vector2(gridPosition.x * 1.5f, gridPosition.y * 1.5f);
        var originalPosition = transform.position;
        var totalTraveled = 0f;
        var timeElapsed = 0f;
        while (totalTraveled < 1)
        {
            totalTraveled = timeElapsed / _moveDuration;
            var next = Vector3.Lerp(originalPosition, finalPosition, totalTraveled);
            transform.position = next;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _isMoving = false;
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
            var next = Quaternion.Lerp(originalRotation, finalRotation, totalRotated);
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
