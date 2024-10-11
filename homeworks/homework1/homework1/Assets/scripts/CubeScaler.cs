using UnityEngine;

public class CubeScaler : MonoBehaviour
{
    public float scaleFactor = 0.1f;  // The amount to scale the cube each time

    void Update()
    {
        // Scale the cube up when "H" is pressed
        if (Input.GetKeyDown(KeyCode.H))
        {
            transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }

        // Scale the cube down when "J" is pressed
        if (Input.GetKeyDown(KeyCode.J))
        {
            transform.localScale -= new Vector3(scaleFactor, scaleFactor, scaleFactor);

            // Ensure the scale does not become negative or too small
            if (transform.localScale.x < 0.1f)
            {
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
        }
    }
}
