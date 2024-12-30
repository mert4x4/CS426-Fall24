using System.Collections;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public Vector3 pointA; // Patrol start point
    public Vector3 pointB; // Patrol end point
    public float speed = 2f; // Guard's movement speed
    public float detectionRadius = 1.0f; // Detection radius
    public int circleSegments = 50; // Number of segments to draw the circle
    public LayerMask playerLayer; // Layer to detect the player

    private bool movingToPointB = true; // Flag to toggle movement direction
    private LineRenderer lineRenderer; // LineRenderer for visualizing the detection radius
    private Coroutine patrolCoroutine; // Reference to the patrol coroutine

    void Start()
    {
        // Initialize LineRenderer for drawing the detection radius
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.useWorldSpace = true;
        lineRenderer.loop = true;
        lineRenderer.positionCount = circleSegments;

        StartPatrol();
    }

    void Update()
    {
        DrawCircle(); // Update the circle position during movement
        DetectPlayer();
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            Vector3 target = movingToPointB ? pointB : pointA;
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }
            movingToPointB = !movingToPointB; // Toggle direction
            yield return new WaitForSeconds(1f); // Pause at each point
        }
    }

    void DetectPlayer()
    {
        // Check for all colliders within the detection radius
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player")) // Check the tag instead of the layer
            {
                Debug.Log("Player detected! Game Over.");
                PlayerMovement player = hit.GetComponent<PlayerMovement>();
                if (player != null)
                {
                    player.Die(); // Call the player's Die method
                }
            }
        }
    }

    public void SetPatrolPoints(Vector3 startPoint, Vector3 endPoint)
    {
        pointA = startPoint;
        pointB = endPoint;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void DrawCircle()
    {
        float angleStep = 360f / circleSegments;
        float angle = 0f;

        for (int i = 0; i < circleSegments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * detectionRadius;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * detectionRadius;

            lineRenderer.SetPosition(i, new Vector3(transform.position.x + x, transform.position.y + 0.01f, transform.position.z + z)); // Match guard's position
            angle += angleStep;
        }
    }

    public void ResetGuard()
    {
        // Stop the current patrol coroutine
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
        }

        // Reset the guard's position to the starting point
        transform.position = pointA;

        // Reset movement direction
        movingToPointB = true;

        // Restart the patrol coroutine
        StartPatrol();

        Debug.Log("Guard reset to initial state.");
    }

    private void StartPatrol()
    {
        patrolCoroutine = StartCoroutine(Patrol());
    }
}
