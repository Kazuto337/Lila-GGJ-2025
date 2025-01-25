using System;
using UnityEngine;

public class PatrolSystem
{
    /// <summary>
    /// Genera un Vector3 aleatorio dentro de un radio máximo desde un vector principal.
    /// </summary>
    /// <param name="mainPosition">La posición base (vector principal) desde donde se calcula el movimiento.</param>
    /// <param name="currentPosition">La posición actual del objeto.</param>
    /// <param name="maxRadius">El radio máximo que define los límites del movimiento.</param>
    /// <returns>Un Vector3 aleatorio dentro del radio permitido.</returns>
    public static Vector3 GenerateRandomPatrolPoint(Vector3 mainPosition, Vector3 currentPosition, float maxRadius)
    {
        // Generar un punto aleatorio dentro de una esfera unitaria
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere;
        randomDirection.y = 0; // Opcional: Mantener el movimiento en el plano XZ si no quieres variación en Y

        // Escalar la dirección aleatoria por una distancia aleatoria dentro del radio
        float randomDistance = UnityEngine.Random.Range(0f, maxRadius);
        Vector3 offset = randomDirection.normalized * randomDistance;

        // Calcular el nuevo punto a partir del vector principal
        Vector3 newPatrolPoint = mainPosition + offset;

        // Si necesitas asegurarte de que el nuevo punto esté dentro del radio máximo del objeto actual
        if (Vector3.Distance(newPatrolPoint, currentPosition) > maxRadius)
        {
            newPatrolPoint = currentPosition + (newPatrolPoint - currentPosition).normalized * maxRadius;
        }

        return newPatrolPoint;
    }
}
