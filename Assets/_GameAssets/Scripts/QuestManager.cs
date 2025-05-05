using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public TextMeshProUGUI questText;

    private int upgradeGoal = 3;
    private int currentUpgradeCount;
    private HashSet<Animal> upgradedAnimals = new HashSet<Animal>();
    private int unlockedAreaCount;
    private int requiredUnlockedAreas = 1;
    private bool quest1Completed, quest2Completed, quest3Started, quest3Completed;
    private int moneyGoal = 300;
    private int moneyAtQuest3Start;

    private void Awake() { Instance = this; }

    private void Start()
    {
        currentUpgradeCount = PlayerPrefs.GetInt("UpgradeCount", 0);
        unlockedAreaCount = PlayerPrefs.GetInt("UnlockedAreaCount", 0);
        quest1Completed = PlayerPrefs.GetInt("Quest1Completed", 0) == 1;
        quest2Completed = PlayerPrefs.GetInt("Quest2Completed", 0) == 1;
        quest3Started = PlayerPrefs.GetInt("Quest3Started", 0) == 1;
        quest3Completed = PlayerPrefs.GetInt("Quest3Completed", 0) == 1;
        moneyAtQuest3Start = PlayerPrefs.GetInt("MoneyAtQuest3Start", 0);
        UpdateQuestUI();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void OnAnimalUpgraded(Animal animal)
    {
        if (quest1Completed) return;
        if (!upgradedAnimals.Contains(animal))
        {
            upgradedAnimals.Add(animal);
            currentUpgradeCount++;
            UpdateQuestUI();
            if (currentUpgradeCount >= upgradeGoal)
            {
                quest1Completed = true;
                StartCoroutine(ShowCompletionMessage("🎉 Görev 1 tamamlandı! +100 para"));
                MoneyManager.Instance.AddMoney(100);
                Invoke(nameof(StartNextQuest), 3f);
            }
        }
    }

    public void RegisterHabitatUnlock()
    {
        if (quest2Completed || !quest1Completed) return;
        unlockedAreaCount++;
        UpdateQuestUI();
        if (unlockedAreaCount >= requiredUnlockedAreas)
        {
            quest2Completed = true;
            StartCoroutine(ShowCompletionMessage("🎉 Görev 2 tamamlandı! +150 para"));
            MoneyManager.Instance.AddMoney(150);
            Invoke(nameof(StartNextQuest), 3f);
        }
    }

    private void StartNextQuest()
    {
        if (!quest3Started)
        {
            quest3Started = true;
            moneyAtQuest3Start = MoneyManager.Instance.CurrentMoney;
        }
        UpdateQuestUI();
    }

    private void Update()
    {
        if (quest3Started && !quest3Completed)
        {
            int earned = MoneyManager.Instance.CurrentMoney - moneyAtQuest3Start;
            if (earned >= moneyGoal)
            {
                quest3Completed = true;
                StartCoroutine(ShowCompletionMessage("🎉 Görev 3 tamamlandı! +500 para"));
                MoneyManager.Instance.AddMoney(500);
            }
            UpdateQuestUI();
        }
    }

    private void UpdateQuestUI()
    {
        if (!quest1Completed)
            questText.text = $"Görev: 3 farklı hayvanı upgrade et ({currentUpgradeCount}/3)";
        else if (!quest2Completed)
            questText.text = $"Görev: 1 habitat alanı aç ({unlockedAreaCount}/{requiredUnlockedAreas})";
        else if (!quest3Completed)
        {
            int earned = MoneyManager.Instance.CurrentMoney - moneyAtQuest3Start;
            questText.text = $"Görev 3: 300 para biriktir ({earned}/300)";
        }
        else
            questText.text = "🎉 Tüm görevler tamamlandı!";
    }

    private IEnumerator ShowCompletionMessage(string message)
    {
        questText.text = message;
        questText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        UpdateQuestUI();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("UpgradeCount", currentUpgradeCount);
        PlayerPrefs.SetInt("UnlockedAreaCount", unlockedAreaCount);
        PlayerPrefs.SetInt("Quest1Completed", quest1Completed ? 1 : 0);
        PlayerPrefs.SetInt("Quest2Completed", quest2Completed ? 1 : 0);
        PlayerPrefs.SetInt("Quest3Started", quest3Started ? 1 : 0);
        PlayerPrefs.SetInt("Quest3Completed", quest3Completed ? 1 : 0);
        PlayerPrefs.SetInt("MoneyAtQuest3Start", moneyAtQuest3Start);
    }
}
