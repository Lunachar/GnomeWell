using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The target to follow.
    public Transform target;

    // Top limit for camera movement.
    public float topLimit = 10.0f;

    // Bottom limit for camera movement.
    public float bottomLimit = -10.0f;

    // Speed of camera follow.
    public float followSpeed = 0.5f;

    // LateUpdate is called after all Update functions have been called.
    private void LateUpdate()
    {
        if (target != null)
        {
            // Get the current position of the camera.
            Vector3 newPosition = this.transform.position;

            // Interpolate the camera's y position towards the target's y position.
            newPosition.y = Mathf.Lerp(newPosition.y, target.position.y, followSpeed);

            // Clamp the camera's y position within the defined limits.
            newPosition.y = Mathf.Min(newPosition.y, topLimit);
            newPosition.y = Mathf.Max(newPosition.y, bottomLimit);

            // Set the new camera position.
            transform.position = newPosition;
        }
    }

    // Draw gizmos to visualize camera limits.
    void OnDrawGizmosSelected()
    {
        // Set gizmos color to yellow.
        Gizmos.color = Color.yellow;

        // Define top and bottom points for camera limits.
        Vector3 topPoint = new Vector3(this.transform.position.x, topLimit, this.transform.position.z);
        Vector3 bottomPoint = new Vector3(this.transform.position.x, bottomLimit, this.transform.position.z);
        
        // Draw a line between the top and bottom points to represent camera limits.
        Gizmos.DrawLine(topPoint, bottomPoint);
    }
}