using UnityEngine;

public class CircleGizmo : MonoBehaviour
{
    [SerializeField] private EnemyStateHandler enemy;
    public Vector3 position; // Centro del círculo
    public float radius; // Radio máximo
    public int segments = 50; // Número de segmentos para el círculo (más segmentos = círculo más suave)
    public Color gizmoColor = Color.red; // Color del Gizmo

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
        float angleStep = 360f / segments; // Ángulo entre puntos

        Vector3 prevPoint = center + new Vector3(radius, 0, 0); // Primer punto del círculo

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad; // Convertir a radianes
            Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

            Gizmos.DrawLine(prevPoint, newPoint); // Dibujar línea entre puntos
            prevPoint = newPoint; // Actualizar el punto anterior
        }
    }
}
