using UnityEngine;
using System.Collections;  // Required for IEnumerator

public class PlatformBehavior : MonoBehaviour
{
    private bool isShrinking = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isShrinking)
        {
            TriggerShrinkAndDestroy();
        }
    }

    public void TriggerShrinkAndDestroy()
    {
        StartCoroutine(DelayedShrink());
    }

    private IEnumerator DelayedShrink()
    {
        yield return new WaitForSeconds(0.5f);  // Wait for 0.5 seconds before shrinking
        StartCoroutine(ShrinkAndDestroy());
    }

    private IEnumerator ShrinkAndDestroy()
    {
        isShrinking = true;
        Vector3 initialScale = transform.localScale;
        float shrinkDuration = 0.5f;  // Duration of shrink animation
        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 0f, elapsedTime / shrinkDuration);
            transform.localScale = initialScale * scale;
            yield return null;
        }

        Destroy(gameObject);
    }
}
