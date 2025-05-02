using System.Collections;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    private int money; // SADECE BUNU KULLANACAĞIZ
    
    public TextMeshProUGUI moneyText;
    private float autoSaveInterval = 30f;
private float autoSaveTimer = 0f;


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

    private void Start()
    {
         money = PlayerPrefs.GetInt("Money", 0);
         UpdateMoneyUI();
    }
    private void OnApplicationQuit()
{
   PlayerPrefs.SetInt("Money", money);
}

    private void Update()
    {
        HandleIdleIncome();
        HandleAutoSave();
    }

    private void HandleAutoSave()
{
    autoSaveTimer += Time.deltaTime;
    if (autoSaveTimer >= autoSaveInterval)
    {
        SaveData();
        autoSaveTimer = 0f;
    }
}
public void SaveData()
{
    // Parayı kaydet
    PlayerPrefs.SetInt("Money", money);

    // Görevleri kaydet
    QuestManager.Instance.SaveData();

    // Hayvan seviyelerini kaydet
    Animal[] animals = FindObjectsByType<Animal>(FindObjectsSortMode.None);
    foreach (var animal in animals)
    {
        animal.SaveData();
    }

    Debug.Log("💾 Auto-save tamamlandı!");
}


    public int CurrentMoney => money; // ARTIK sadece money döndürüyor

    public void SpendMoney(int amount)
    {
        money -= amount;
        if (money < 0) money = 0;
        UpdateMoneyUI();
    }

    public void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: " + money.ToString();
        }
    }
public void AddMoney(int amount)
{
    if (GameManager.Instance != null && GameManager.Instance.isBoostActive)
        amount *= 2;

    money += amount;
    UpdateMoneyUI();
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
