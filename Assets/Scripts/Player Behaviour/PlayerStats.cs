using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private List<Heart> hearts;
    [SerializeField] TMP_Text timeTMP;
    [SerializeField] TMP_Text coins;
    public int maxHealth = 6;
    public int currentHealth;
    public int coinsCollected = 0;

    private float timePlayed = 0f;

    public UnityEvent onPlayerDied;

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

        timeTMP.text = $"{minutes:D2}:{seconds:D2}";
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        hearts[currentHealth].EmptyHeart();

        if (currentHealth <= 0)
        {
            onPlayerDied.Invoke();
        }
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
