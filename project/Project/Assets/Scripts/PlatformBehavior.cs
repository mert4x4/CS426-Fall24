using UnityEngine;
using System.Collections;

public class PlatformBehavior : MonoBehaviour
{
    private bool isShrinking = false;
    private Vector3 initialScale = new Vector3(2.698692f, 0.44969f, 2.811703f); // Hardcoded initial scale

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isShrinking)
        {
            TriggerShrinkAndReturnToPool();
        }
    }

    public void TriggerShrinkAndReturnToPool()
    {
        if (!isShrinking)
        {
            StartCoroutine(DelayedShrink());
        }
    }

    private IEnumerator DelayedShrink()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShrinkAndReturnToPool());
    }

    private IEnumerator ShrinkAndReturnToPool()
    {
        isShrinking = true;
        Vector3 currentScale = transform.localScale;
        float shrinkDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 0f, elapsedTime / shrinkDuration);
            transform.localScale = currentScale * scale;
            yield return null;
        }

        ResetPlatformState();
    }

    public void ResetPlatformState()
    {
        isShrinking = false;
        transform.localScale = initialScale; // Reset to the hardcoded initial scale
        gameObject.SetActive(false);
    }
}
