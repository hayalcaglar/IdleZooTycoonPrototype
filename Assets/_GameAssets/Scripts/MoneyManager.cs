using System.Collections;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    public int currentMoney = 0;
    public TextMeshProUGUI moneyText;

    [Header("Idle Income Settings")]
    public float idleInterval = 10f; // Kaç saniyede bir idle para eklenecek
    private float idleTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        HandleIdleIncome();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
    }

    public void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: " + currentMoney.ToString();
        }
    }

    private void HandleIdleIncome()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleInterval)
        {
            int idleIncome = CalculateIdleIncome();
            AddMoney(idleIncome);
            idleTimer = 0f; // Sıfırla
        }
    }

    private int CalculateIdleIncome()
    {
        int totalIncome = 0;
        Animal[] animals = FindObjectsByType<Animal>(FindObjectsSortMode.None);


        foreach (Animal animal in animals)
        {
            totalIncome += animal.moneyPerClick;
        }

       return Mathf.RoundToInt(totalIncome * 0.2f); // %20'si kadar idle gelir

    }
}
