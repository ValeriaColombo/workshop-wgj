using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float minXPosition = 0;
    [SerializeField] private float minYPosition = 0;
    [SerializeField] private float maxXPosition = 0;
    [SerializeField] private float maxYPosition = 0;

    private void Update()
    {
        Vector2 targetPosition = _target.position;
        float xPosition = CheckLimits(targetPosition.x, minXPosition, maxXPosition);
        float yPosition = CheckLimits(targetPosition.y, minYPosition, maxYPosition);
        transform.position = new Vector3(xPosition, yPosition, -10);
    }

    private float CheckLimits(float targetPosition, float min, float max)
    {
        return Mathf.Max(min, Mathf.Min(max, targetPosition));
    }
}
