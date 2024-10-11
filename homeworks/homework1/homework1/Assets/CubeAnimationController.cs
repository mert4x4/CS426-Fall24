using UnityEngine;

public class CubeAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to the cube
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // When "H" is pressed, trigger the scale-up animation
        if (Input.GetKeyDown(KeyCode.H))
        {
            animator.SetTrigger("ScaleUpTrigger");
        }

        // When "J" is pressed, trigger the scale-down animation
        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("ScaleDownTrigger");
        }
    }
}
