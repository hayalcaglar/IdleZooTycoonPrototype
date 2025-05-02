using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI infoText;
    public Slider upgradeProgressBar;
    public bool isBoostActive = false;
    public float boostDuration = 30f;
    private float boostTimer = 0f;
    public GameObject shopPanel;





    public GameObject upgradePanel;
    public Animal selectedAnimal;

    private void Start()
{

    upgradePanel.SetActive(false);
     if (PlayerPrefs.GetInt("BoostUsed", 0) == 1)
    {
        if (boostButton != null)
            boostButton.SetActive(false);
    }
}


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public GameObject boostButton;  // Inspector’dan atayacağız
    public void ActivateBoost()
{
    if (!isBoostActive)
    {
        isBoostActive = true;
        boostTimer = boostDuration;

        // 🔥 Butonu devre dışı bırak
        if (boostButton != null)
            boostButton.SetActive(false);

        StartCoroutine(HandleBoost());
        ShowInfoText("🔥 Boost Aktif! 30 saniye boyunca 2x para!", 2f);
    }
}


    public void OpenUpgradePanel(Animal animal)
    {
        selectedAnimal = animal;
        upgradePanel.SetActive(true);

         // 🔥 Panel açıldığında hemen barı güncelle
         UpdateUpgradeProgress(selectedAnimal.level);
    }

    public void UpgradeSelectedAnimal()
    {
        if (selectedAnimal != null)
        {
            selectedAnimal.Upgrade();
             upgradePanel.SetActive(false); // Paneli kapat
        }
    }
public void SellSelectedAnimal()
{
    if (selectedAnimal != null)
    {
        int sellValue = Mathf.RoundToInt(selectedAnimal.moneyPerClick * 5);
        MoneyManager.Instance.AddMoney(sellValue);

        ShowInfoText($"+{sellValue} para: {selectedAnimal.animalName} satıldı!", 3f);

        Destroy(selectedAnimal.gameObject);
        upgradePanel.SetActive(false);
        selectedAnimal = null; // seçili hayvanı sıfırla
    }
}



public void ShowInfoText(string message, float duration = 2f)
{
    infoText.gameObject.SetActive(true);
    infoText.text = message;
    StartCoroutine(HideInfoTextAfterDelay(duration));
}

private IEnumerator HideInfoTextAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    infoText.gameObject.SetActive(false);
}

private IEnumerator HandleBoost()
{
    while (boostTimer > 0)
    {
        boostTimer -= Time.deltaTime;
        // İstersen burada UI sayacı güncelle
        yield return null;
    }

    isBoostActive = false;
    ShowInfoText("⏰ Boost Bitti!", 2f);
     PlayerPrefs.SetInt("BoostUsed", 1); // Kullanım kaydını güncelle

// // 🔥 Butonu tekrar aç
//     if (boostButton != null)
//         boostButton.SetActive(true);

}


public void UpdateUpgradeProgress(int level)
{
    float fill = Mathf.Clamp01((float)level / 10f); // max seviye 10 kabul ediyoruz
    upgradeProgressBar.value = fill;
}

public void AutoSaveAll()
{
    MoneyManager.Instance.SaveData();
    QuestManager.Instance.SaveData();
    
    Animal[] animals = FindObjectsByType<Animal>(FindObjectsSortMode.None);
    foreach (var animal in animals)
    {
        animal.SaveData();
    }

    Debug.Log("💾 Tüm sistemler kaydedildi!");
}

public void ResetGame()
{
    PlayerPrefs.DeleteAll();
    Debug.Log("Oyun sıfırlandı!"); 

    // İstersen Unity sahnesini yeniden yükle:
    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
}



public void OpenShop()
{
    shopPanel.SetActive(true);
}

public void CloseShop()
{
    shopPanel.SetActive(false);
}

public void BuyAnimal()
{
    int cost = 500;
    if (MoneyManager.Instance.CurrentMoney >= cost)
    {
        MoneyManager.Instance.SpendMoney(cost);
        Debug.Log("🐾 Yeni hayvan satın alındı!");

        // Buraya hayvan spawn veya açma kodunu ekleyeceğiz
    }
    else
    {
        ShowInfoText("Yeterli paran yok!", 2f);
    }
}

public void BuyNewAnimal()
{
    int cost = 500;
    if (MoneyManager.Instance.CurrentMoney >= cost)
    {
        MoneyManager.Instance.SpendMoney(cost);
        ShowInfoText("🐾 Yeni hayvan satın alındı!", 2f);
        Debug.Log("Yeni hayvan satın alındı!");
        // Buraya prefab instantiate kodu eklenecek (istersen sonra)
    }
    else
    {
        GameManager.Instance.ShowInfoText("Yeterli paran yok!", 2f);
    }
}

public void BuyDecoration()
{
    int cost = 300;
    if (MoneyManager.Instance.CurrentMoney >= cost)
    {
        MoneyManager.Instance.SpendMoney(cost);
         ShowInfoText("🎨 Dekorasyon satın alındı!", 2f);
        Debug.Log("🎨 Dekorasyon satın alındı!");

        // Buraya dekor ekleme kodunu ekleyeceğiz
    }
    else
    {
        ShowInfoText("Yeterli paran yok!", 2f);
    }
}




    
}
