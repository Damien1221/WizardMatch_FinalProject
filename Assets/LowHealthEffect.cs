using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class LowHealthEffect : MonoBehaviour
{
    public CanvasGroup warningImage;
    public float totalDuration = 5f;
    public float minBlinkSpeed = 0.3f;
    public float maxBlinkSpeed = 0.6f;

    public float warningFadeOutDuration = 1f;

    private Coroutine blinkRoutine;

    public void TriggerLowHealthEffect()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        warningImage.alpha = 0;
        blinkRoutine = StartCoroutine(BlinkIncreasingSpeed());
    }

    private IEnumerator BlinkIncreasingSpeed()
    {
        float elapsed = 0f;

        while (elapsed < totalDuration)
        {
            float progress = elapsed / totalDuration;
            float blinkInterval = Mathf.Lerp(maxBlinkSpeed, minBlinkSpeed, progress); // faster over time

            // Toggle blink using fade
            warningImage.DOFade(1, blinkInterval / 2f);
            yield return new WaitForSeconds(blinkInterval / 2f);
            warningImage.DOFade(0, blinkInterval / 2f);
            yield return new WaitForSeconds(blinkInterval / 2f);

            elapsed += blinkInterval;
        }

        // After blinking is done, make the image stay fully visible
        warningImage.DOFade(1f, 0.2f);
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1.5f);
        warningImage.DOFade(0f, warningFadeOutDuration);
    }
}
