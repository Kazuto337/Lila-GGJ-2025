using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private List<Heart> hearts;
    public int maxHealth = 6;
    public int currentHealth;
    public int coinsCollected = 0;

    private float timePlayed = 0f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        timePlayed += Time.deltaTime;
        DisplayTime();
    }

    void DisplayTime()
    {
        int minutes = Mathf.FloorToInt(timePlayed / 60);
        int seconds = Mathf.FloorToInt(timePlayed % 60);

        //Debug.Log($"Tiempo jugado: {minutes:D2}:{seconds:D2}");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        hearts[currentHealth + 1].EmptyHeart();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        hearts[currentHealth].FillHeart();
    }

    public void CollectCoin(int amount)
    {
        coinsCollected += amount;
        Debug.Log($"Monedas recolectadas: {coinsCollected}");
    }
}
