using UnityEngine;
using TMPro;


public class MoneyManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    public static MoneyManager Instance { get; private set; }

    public int currentMoney = 0;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

   public void AddMoney(int amount)
{
    currentMoney += amount;
    UpdateMoneyUI();
}

private void UpdateMoneyUI()
{
    if (moneyText != null)
    {
        moneyText.text = "Money: " + currentMoney.ToString();
    }
}

}
