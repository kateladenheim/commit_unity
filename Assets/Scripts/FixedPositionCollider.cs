using UnityEngine;

public class FixedPositionCollider : MonoBehaviour
{
    [Tooltip("The collider to keep at a fixed position")]
    public BoxCollider targetCollider;

    [Tooltip("The position where the collider should stay")]
    public Vector3 fixedPosition;

    [Tooltip("Whether to use the starting position as the fixed position")]
    public bool useStartingPosition = true;

    [Tooltip("Whether to preserve the collider's rotation")]
    public bool preserveRotation = false;

    // Reference to the original parent transform
    private Transform originalParent;

    // Original local position within the parent
    private Vector3 originalLocalPosition;

    // Original local rotation within the parent
    private Quaternion originalLocalRotation;

    // Whether we've set up the collider
    private bool initialized = false;

    private void Start()
    {
        // Find the collider if not assigned
        if (targetCollider == null)
        {
            targetCollider = GetComponent<BoxCollider>();

            if (targetCollider == null)
            {
                Debug.LogError("No BoxCollider found! Please assign one in the inspector or add one to this GameObject.");
                return;
            }
        }

        // Create a gameObject to hold the collider
        GameObject holderObject = new GameObject(targetCollider.gameObject.name + "_FixedCollider");

        // Store original parent and position info before reparenting
        originalParent = targetCollider.transform.parent;
        originalLocalPosition = targetCollider.transform.localPosition;
        originalLocalRotation = targetCollider.transform.localRotation;

        if (useStartingPosition)
        {
            // Use the current world position as the fixed position
            fixedPosition = targetCollider.transform.position;
        }

        // Position the holder at the fixed position
        holderObject.transform.position = fixedPosition;

        // Set initial rotation if preserving it
        if (preserveRotation)
        {
            holderObject.transform.rotation = targetCollider.transform.rotation;
        }

        // Store collider properties before reparenting
        Vector3 colliderCenter = targetCollider.center;
        Vector3 colliderSize = targetCollider.size;

        // Move the collider to the fixed holder
        targetCollider.transform.parent = holderObject.transform;
        targetCollider.transform.localPosition = Vector3.zero;

        // Reset collider properties that might have changed with reparenting
        targetCollider.center = colliderCenter;
        targetCollider.size = colliderSize;

        if (!preserveRotation)
        {
            targetCollider.transform.rotation = Quaternion.identity;
        }

        initialized = true;

        Debug.Log($"Collider fixed at position: {fixedPosition}");
    }

    public void ResetCollider()
    {
        if (!initialized || targetCollider == null || originalParent == null) return;

        // Return collider to original parent and position
        targetCollider.transform.parent = originalParent;
        targetCollider.transform.localPosition = originalLocalPosition;
        targetCollider.transform.localRotation = originalLocalRotation;

        // Destroy the holder object
        if (targetCollider.transform.parent != null &&
            targetCollider.transform.parent.name.EndsWith("_FixedCollider"))
        {
            Destroy(targetCollider.transform.parent.gameObject);
        }

        initialized = false;

        Debug.Log("Collider reset to original state");
    }

    // Helper method to update the fixed position at runtime
    public void SetFixedPosition(Vector3 newPosition)
    {
        fixedPosition = newPosition;

        if (initialized && targetCollider != null)
        {
            // Update the holder object position
            targetCollider.transform.parent.position = fixedPosition;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            // Draw the fixed position in the editor
            Gizmos.color = Color.yellow;

            Vector3 gizmoPosition = useStartingPosition ?
                (targetCollider != null ? targetCollider.transform.position : transform.position) :
                fixedPosition;

            Gizmos.DrawWireCube(gizmoPosition, targetCollider != null ? targetCollider.size : Vector3.one);
            Gizmos.DrawSphere(gizmoPosition, 0.1f);
        }
    }
}
