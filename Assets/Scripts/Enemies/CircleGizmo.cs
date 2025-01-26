using UnityEngine;
/// <summary>
/// A component that draws a circle gizmo in the Scene view for visualization purposes.
/// Author: ChatGPT
/// </summary>
public class CircleGizmo : MonoBehaviour
{
    [SerializeField] private EnemyStateHandler enemy;
    public Vector3 position;
    public float radius; 
    public int segments = 50;
    public Color gizmoColor = Color.red; 

    private void OnDrawGizmos()
    {
        radius = enemy.maxRadius;
        position = enemy.mainPosition;
        Gizmos.color = gizmoColor;
        DrawCircle(position, radius, segments);
    }

    /// <summary>
    /// Dibuja un círculo en el plano XZ con Gizmos.
    /// </summary>
    /// <param name="center">El centro del círculo.</param>
    /// <param name="radius">El radio del círculo.</param>
    /// <param name="segments">El número de segmentos para el círculo.</param>
    private void DrawCircle(Vector3 center, float radius, int segments)
    {
        float angleStep = 360f / segments; 

        Vector3 prevPoint = center + new Vector3(radius, 0, 0); 

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad; 
            Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

            Gizmos.DrawLine(prevPoint, newPoint); 
            prevPoint = newPoint; 
        }
    }
}
