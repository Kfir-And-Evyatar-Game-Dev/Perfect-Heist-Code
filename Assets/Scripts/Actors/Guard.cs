using UnityEngine;

/// <summary>
/// A guard walking back and forth between two cells on a fixed schedule.
/// Sees characters inside a cone in front of it, but not through walls.
/// </summary>
public class Guard : MonoBehaviour
{
    [Tooltip("How far the guard sees, in cells.")]
    [SerializeField] float viewDistance = 3f;

    [Tooltip("Total width of the view cone, in degrees.")]
    [SerializeField] float viewAngle = 90f;

    [Tooltip("Optional child object that is rotated to face the walking direction.")]
    [SerializeField] Transform facingVisual;

    [Tooltip("Tick if the image shows the guard looking toward the bottom of the picture.")]
    [SerializeField] bool imageFacesDown = true;

    public Vector2 DocPos { get; private set; }
    /// <summary>Walking direction in document coordinates (y down).</summary>
    public Vector2 Facing { get; private set; } = Vector2.up;

    MapBuilder map;
    Vector2Int pointA, pointB;
    GuardRoute route;

    public Vector2Int PatrolStart => pointA;
    public Vector2Int PatrolEnd => pointB;
    public Vector2Int? StorageStop => route.visitStorage ? route.storageCell : null;

    [Tooltip("Colour of the guard's field of view.")]
    [SerializeField] Color viewColor = new Color(0.2f, 0.45f, 1f, 0.3f);

    public void Init(MapBuilder map, Vector2Int start, GuardRoute route)
    {
        this.map = map;
        pointA = start;
        pointB = route.patrolEnd;
        this.route = route;
        Facing = route.stationaryFacing.normalized;
        ShowAt(0f);

        // Blue field of view that follows the guard, turns with him and stops at walls.
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            VisionCone cone = VisionCone.Create(name + " view", map, () => DocPos, () => Facing,
                                                viewDistance, viewAngle, viewColor, sr.sharedMaterial, 2);
            cone.transform.SetParent(transform.parent, true);
        }
    }

    /// <summary>Patrol position at run time t: A to B in the first half of the period, back in the second.</summary>
    public void ShowAt(float t)
    {
        if (map == null) return;

        float period = Mathf.Max(0.1f, route.period);
        float phase = Mathf.Repeat(t, period);
        float dwell = route.visitStorage ? Mathf.Clamp(route.storageDwell, 0f, period - 0.1f) : 0f;
        float mainDistance = Vector2.Distance(pointA, pointB);
        float sideDistance = route.visitStorage ? Vector2.Distance(pointB, route.storageCell) : 0f;
        float half = (period - dwell) / 2f;
        float mainTime = half * mainDistance / Mathf.Max(0.01f, mainDistance + sideDistance);

        if (phase < mainTime)
            FollowLeg(pointA, pointB, phase, mainTime);
        else if (phase < half && route.visitStorage)
            FollowLeg(pointB, route.storageCell, phase - mainTime, half - mainTime);
        else if (phase < half + dwell)
            FollowLeg(pointB, route.storageCell, 1f, 1f);
        else if (phase < period - mainTime && route.visitStorage)
            FollowLeg(route.storageCell, pointB, phase - half - dwell, half - mainTime);
        else
            FollowLeg(pointB, pointA, phase - period + mainTime, mainTime);

        transform.position = map.DocToWorld(DocPos);
        if (facingVisual != null)
        {
            var worldFacing = new Vector3(Facing.x, -Facing.y, 0f); // document y points down
            facingVisual.up = imageFacesDown ? -worldFacing : worldFacing;
        }
    }

    void FollowLeg(Vector2 from, Vector2 to, float elapsed, float duration)
    {
        DocPos = Vector2.Lerp(from, to, duration > 0f ? elapsed / duration : 0f);
        Vector2 direction = to - from;
        Facing = direction == Vector2.zero ? route.stationaryFacing.normalized : direction.normalized;
    }

    public bool CanSee(Vector2 target)
    {
        Vector2 toTarget = target - DocPos;
        if (toTarget.magnitude > viewDistance) return false;
        if (toTarget.sqrMagnitude > 0.0001f && Vector2.Angle(Facing, toTarget) > viewAngle / 2f) return false;
        return map.HasLineOfSight(DocPos, target);
    }

    void OnDrawGizmos()
    {
        // Draws the view cone in the Scene view.
        Gizmos.color = new Color(1f, 0.6f, 0f, 0.8f);
        Vector3 forward = new Vector3(Facing.x, -Facing.y, 0f);
        Vector3 left = Quaternion.Euler(0, 0, viewAngle / 2f) * forward * viewDistance;
        Vector3 right = Quaternion.Euler(0, 0, -viewAngle / 2f) * forward * viewDistance;
        Vector3 p = transform.position;
        Gizmos.DrawLine(p, p + left);
        Gizmos.DrawLine(p, p + right);
        Gizmos.DrawLine(p + left, p + right);
    }
}
