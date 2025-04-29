using UnityEngine;
using System.Collections;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public TextMeshProUGUI questText;
    public GameObject interactionTextObject; // 🔥 InteractionText objesi
    public int upgradeGoal = 3;
    public int currentUpgradeCount = 0;
    public int rewardAmount = 200;

    private bool questCompleted = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (questText != null)
        {
            questText.gameObject.SetActive(true);
            UpdateQuestUI();
            StartCoroutine(HideQuestTextAfterDelay());
        }
    }

    private IEnumerator HideQuestTextAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        if (questText != null)
            questText.gameObject.SetActive(false);

        // 🔥 Görev yazısı kapanınca interaction yazısını eski haline getiriyoruz
        if (interactionTextObject != null)
            interactionTextObject.SetActive(true);
    }

    public void RegisterUpgrade()
    {
        if (questCompleted) return;

        currentUpgradeCount++;

        if (currentUpgradeCount >= upgradeGoal)
        {
            questCompleted = true;
            MoneyManager.Instance.AddMoney(rewardAmount);

            if (interactionTextObject != null)
                interactionTextObject.SetActive(false); // 🔥 Görev tamamlanınca interaction yazısını kapatıyoruz

            questText.gameObject.SetActive(true);
            questText.text = $"🎉 Görev Tamamlandı! +{rewardAmount} para";

            StartCoroutine(HideQuestTextAfterDelay());

            Debug.Log("Görev tamamlandı! Ödül verildi.");
        }
        else
        {
            UpdateQuestUI();
        }
    }

    private void UpdateQuestUI()
    {
        if (questText != null)
            questText.text = $"Görev: {upgradeGoal} hayvanı upgrade et ({currentUpgradeCount}/{upgradeGoal})";
    }
}
