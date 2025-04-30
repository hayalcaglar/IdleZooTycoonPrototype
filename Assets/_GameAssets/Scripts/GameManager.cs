using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI infoText;
    public Slider upgradeProgressBar;




    public GameObject upgradePanel;
    public Animal selectedAnimal;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void OpenUpgradePanel(Animal animal)
    {
        selectedAnimal = animal;
        upgradePanel.SetActive(true);
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

public void UpdateUpgradeProgress(int level)
{
    float fill = Mathf.Clamp01((float)level / 10f); // max seviye 10 kabul ediyoruz
    upgradeProgressBar.value = fill;
}


    
}
