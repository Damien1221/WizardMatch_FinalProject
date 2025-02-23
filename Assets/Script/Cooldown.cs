using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CoolDown
{
    public enum Progress
    {
        Ready,
        Started,
        InProgress,
        Finished
    }

    public float Duration = 1.0f;

    public Progress CurrentProgress = Progress.Ready;

    public bool IsOnCooldown
    {
        get { return _isOnCooldown; }
    }

    private float _currentDuration = 0f;

    private bool _isOnCooldown = false;

    private Coroutine _coroutine;

    public void StartCooldown()
    {
        if (CurrentProgress is Progress.Started or Progress.InProgress)
            return;

        _coroutine = CoroutineHost.Instance.StartCoroutine(DoCooldown());
    }
    public void StopCooldown()
    {
        if (_coroutine != null)
        {
            CoroutineHost.Instance.StopCoroutine(_coroutine);
        }
        _currentDuration = 0f;
        _isOnCooldown = false;
        CurrentProgress = Progress.Ready;
    }
    private IEnumerator DoCooldown()
    {
        CurrentProgress = Progress.Started;
        _currentDuration = Duration;
        _isOnCooldown = true;

        while (_currentDuration > 0)
        {
            _currentDuration -= Time.deltaTime;
            CurrentProgress = Progress.InProgress;

            yield return null;
        }

        _currentDuration = 0f;
        _isOnCooldown = false;
        CurrentProgress = Progress.Finished;
    }
}
