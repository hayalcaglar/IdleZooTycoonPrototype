using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public TextMeshProUGUI questText;
    public GameObject interactionTextObject;

    private int upgradeGoal = 3;
    private int currentUpgradeCount = 0;
    private HashSet<Animal> upgradedAnimals = new HashSet<Animal>(); 
    private int unlockedAreaCount = 0;
    private int requiredUnlockedAreas = 1;

    private bool quest1Completed = false;
    private bool quest2Completed = false;
    private bool quest3Started = false;
    private bool quest3Completed = false;
    private int moneyGoal = 300; // 300 para biriktirilecek
    private int moneyAtQuest3Start = 0;



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        questText.gameObject.SetActive(true);
        questText.text = "Görev: 3 farklı hayvanı upgrade et (0/3)";
        StartCoroutine(HideQuestText());
    }

   public void OnAnimalUpgraded(Animal animal)
{
    if (quest1Completed) return; // ✅ Görev 1 zaten tamamlandıysa hiçbir şey yapma

    if (!upgradedAnimals.Contains(animal))
    {
        upgradedAnimals.Add(animal);
        currentUpgradeCount++;

        questText.text = $"Görev: 3 farklı hayvanı upgrade et ({currentUpgradeCount}/3)";

        if (currentUpgradeCount >= 3)
        {
            quest1Completed = true;
            CompleteQuest(); // 🎉 Ödül ver ve yazıyı göster
        }
    }
    else
    {
        Debug.Log("Bu hayvan zaten görev için sayıldı, tekrar eklenmedi.");
    }
}

    private void CompleteQuest()
{
    questText.gameObject.SetActive(true); // ✅ Yazı kesinlikle görünür olsun
    questText.text = "🎉 Görev tamamlandı! +300 para";
    MoneyManager.Instance.AddMoney(300);
    StartCoroutine(HideQuestTextAfterDelay(3f)); // ✅ 3 saniye sonra yazıyı gizle
    Invoke(nameof(StartNextQuest), 3f); // ✅ 3 saniye sonra yeni görevi başlat
}


    private IEnumerator HideQuestText()
    {
        yield return new WaitForSeconds(3f);
        questText.gameObject.SetActive(false);
    }

    public void RegisterUpgrade()
    {
        if (quest1Completed) return;

        currentUpgradeCount++;

        if (currentUpgradeCount >= upgradeGoal)
        {
            quest1Completed = true;
            MoneyManager.Instance.AddMoney(200);
            questText.gameObject.SetActive(true);
            questText.text = $"🎉 Görev tamamlandı! +200 para";

            StartCoroutine(HideQuestText());

            // ✅ Yeni görevi başlat
            Invoke(nameof(StartNextQuest), 3f);
        }
        else
        {
            questText.gameObject.SetActive(true);
            questText.text = $"Görev: {upgradeGoal} hayvanı upgrade et ({currentUpgradeCount}/{upgradeGoal})";
            StartCoroutine(HideQuestText());
        }
    }

private void StartNextQuest()
{
    StopAllCoroutines(); // 🛑 Önceki coroutine’leri durdur

    if (!quest2Completed)
    {
        // GÖREV 2: Habitat açma görevi
        questText.gameObject.SetActive(true);
        questText.text = "GÖREV 2: 1 habitat alanı aç (0/1)";
        StartCoroutine(HideQuestTextAfterDelay(5f));
    }
   else if (!quest3Started)
{
    quest3Started = true;
    moneyAtQuest3Start = MoneyManager.Instance.CurrentMoney; // 🔥 Başlangıçtaki parayı kaydet
    questText.gameObject.SetActive(true);
    questText.text = $"GÖREV 3: 500 para biriktir ({moneyAtQuest3Start}/500)";
    StartCoroutine(HideQuestTextAfterDelay(5f));
}

}



private IEnumerator HideQuestTextAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    questText.gameObject.SetActive(false);
}

    public void RegisterHabitatUnlock()
{
    if (quest2Completed || !quest1Completed) return;

    unlockedAreaCount++;

    if (unlockedAreaCount >= requiredUnlockedAreas)
    {
        quest2Completed = true;
        MoneyManager.Instance.AddMoney(300);
        questText.gameObject.SetActive(true);
        questText.text = $"🎉 Görev tamamlandı! +300 para";

        StartCoroutine(HideQuestText());

        Invoke(nameof(StartNextQuest), 3f); // ✅ 3 saniye sonra Görev 3 başlasın
    }
    else
    {
        questText.gameObject.SetActive(true);
        questText.text = $"Görev: {requiredUnlockedAreas} habitat alanı aç ({unlockedAreaCount}/{requiredUnlockedAreas})";
        StartCoroutine(HideQuestText());
    }
}

private void Update()
{
    if (quest3Started && !quest3Completed)
    {
        int currentMoney = MoneyManager.Instance.CurrentMoney;
        int earnedDuringQuest = currentMoney - moneyAtQuest3Start;

        if (earnedDuringQuest >= moneyGoal)
        {
            quest3Completed = true;
            questText.gameObject.SetActive(true);
            questText.text = "🎉 Görev 3 tamamlandı! +500 para ödülü";
            MoneyManager.Instance.AddMoney(500);
            StartCoroutine(HideQuestTextAfterDelay(4f));
        }
        else
        {
            questText.text = $"GÖREV 3: 500 para biriktir ({earnedDuringQuest}/500)";
        }
    }
}


}
