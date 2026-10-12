using System.Collections;
using UnityEngine;

/// <summary>
/// Snaps the XR Origin's Y position to the assigned floor collider on startup.
/// Use with XR Origin Tracking Origin Mode = Floor and Camera Y Offset = 0.
/// Assign a collider covering the intended starting area, not the whole store's walls.
/// </summary>
public class XRStartAtFloor : MonoBehaviour
{
    [Header("Floor Detection")]
    [Tooltip("Assign a collider covering the walkable floor at the starting location.")]
    public Collider floorCollider;

    [Tooltip("How far above the starting position to begin searching (metres).")]
    [Min(0.1f)] public float searchHeight = 20f;

    [Tooltip("How far below the starting position to search (metres).")]
    [Min(0.1f)] public float searchDistance = 100f;

    [Header("Height Adjustment")]
    [Tooltip("Small optional adjustment above the detected floor (metres). Usually 0.")]
    public float heightAdjustment = 0f;

    [Header("Debug")]
    public bool showDebugLogs = true;

    private IEnumerator Start()
    {
        // Allow the XR tracking system to initialize before snapping the XR Origin.
        yield return null;
        yield return null;
        SnapToFloor();
    }

    [ContextMenu("Snap To Floor")]
    public void SnapToFloor()
    {
        if (floorCollider == null)
        {
            Debug.LogError("XRStartAtFloor: Assign a Floor Collider in the Inspector.", this);
            return;
        }

        // The root determines the selected start location. Its X/Z and rotation
        // remain unchanged; only Y is adjusted to the floor height.
        Vector3 rayOrigin = new Vector3(
            transform.position.x,
            Mathf.Max(transform.position.y + searchHeight,
                      floorCollider.bounds.max.y + 1f),
            transform.position.z);

        Physics.SyncTransforms();
        float rayDistance = Mathf.Max(
            searchHeight + searchDistance,
            rayOrigin.y - floorCollider.bounds.min.y + 1f);

        RaycastHit hit;
        if (floorCollider.Raycast(new Ray(rayOrigin, Vector3.down), out hit, rayDistance))
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y + heightAdjustment;
            transform.position = newPosition;

            if (showDebugLogs)
                Debug.Log("XRStartAtFloor: XR Origin Y set to " + newPosition.y, this);
        }
        else
        {
            Debug.LogWarning(
                "XRStartAtFloor: No floor hit at this start location. " +
                "Check Floor Collider coverage and search distances.", this);
        }
    }
}
