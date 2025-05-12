using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCollisionDetector : MonoBehaviour
{
    [Tooltip("Reference to the FallSaver component")]
    public FallSaver fallSaver;

    [Tooltip("Reference to the character's head transform")]
    public Transform headTransform;

    [Tooltip("Debug visualization")]
    public bool showDebug = true;

    [Tooltip("Collision check frequency (seconds)")]
    public float checkFrequency = 0.05f;

    [Tooltip("Minimum downward speed to consider as falling (units/second)")]
    public float minFallingSpeed = 0.1f;

    [Tooltip("Cooldown time after collision to prevent multiple triggers (seconds)")]
    public float collisionCooldown = 1.0f;

    private BoxCollider boxCollider;
    private bool isCheckingCollision = false;
    private Vector3 previousHeadPosition;
    private bool isInCooldown = false;
    private bool wasInCollider = false;

    private void Start()
    {
        // Get the box collider or add one if it doesn't exist
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
        }

        // Initialize previous position
        if (headTransform != null)
        {
            previousHeadPosition = headTransform.position;
        }

        // Start checking for collisions
        StartCoroutine(CheckCollisionRoutine());
    }

    private IEnumerator CheckCollisionRoutine()
    {
        isCheckingCollision = true;

        while (isCheckingCollision && headTransform != null)
        {
            // Calculate movement direction and speed
            Vector3 currentHeadPosition = headTransform.position;
            float verticalMovement = previousHeadPosition.y - currentHeadPosition.y;
            float verticalSpeed = verticalMovement / checkFrequency;

            // Store current position for next frame
            previousHeadPosition = currentHeadPosition;

            // Check if head is inside the collider
            bool isInCollider = boxCollider.bounds.Contains(currentHeadPosition);

            // Only trigger when:
            // 1. The head is moving downward faster than minFallingSpeed
            // 2. The head is inside the collider
            // 3. We're not in cooldown
            // 4. Either this is the first time entering the collider OR we've exited and re-entered
            if (verticalSpeed >= minFallingSpeed &&
                isInCollider &&
                !isInCooldown &&
                (!wasInCollider || (wasInCollider && !isInCollider)))
            {
                Debug.Log($"Downward collision detected! Vertical speed: {verticalSpeed} units/sec");

                // Manually trigger the FallSaver's OnTriggerEnter
                if (fallSaver != null)
                {
                    Collider headCollider = headTransform.GetComponent<Collider>();
                    if (headCollider != null)
                    {
                        fallSaver.OnTriggerEnter(headCollider);
                    }
                    else
                    {
                        // If no collider on head, still trigger fall saver
                        fallSaver.SendMessage("OnTriggerEnter", boxCollider);
                    }
                }

                // Start cooldown to prevent multiple triggers
                StartCoroutine(CollisionCooldown());
            }

            // Update was in collider state
            wasInCollider = isInCollider;

            yield return new WaitForSeconds(checkFrequency);
        }
    }

    private IEnumerator CollisionCooldown()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(collisionCooldown);
        isInCooldown = false;
    }

    private void OnDrawGizmos()
    {
        if (showDebug && boxCollider != null)
        {
            // Draw the collision box
            Gizmos.color = isInCooldown ? Color.red : Color.green;
            Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);

            if (headTransform != null)
            {
                // Draw line to head
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, headTransform.position);
                Gizmos.DrawSphere(headTransform.position, 0.1f);

                // Draw current movement direction (in play mode)
                if (Application.isPlaying)
                {
                    Vector3 direction = headTransform.position - previousHeadPosition;
                    if (direction.magnitude > 0.001f)
                    {
                        Gizmos.color = direction.y < 0 ? Color.red : Color.green;
                        Gizmos.DrawRay(headTransform.position, direction.normalized * 0.5f);
                    }
                }
            }
        }
    }

    // Call this to stop checking for collisions
    public void StopCollisionChecking()
    {
        isCheckingCollision = false;
    }
}