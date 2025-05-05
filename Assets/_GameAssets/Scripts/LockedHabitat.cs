using UnityEngine;
using TMPro;

public class LockedHabitat : MonoBehaviour
{
    public int unlockCost = 100; 
    public TextMeshProUGUI interactionText;
    private Transform playerTransform;
    private bool isPlayerNearby = false;

    public GameObject areaToActivate; 
    public float interactionDistance = 10f;

    public string habitatName = "Habitat1"; // 🔥 Her kilitli alan için benzersiz isim

    private void Start()
    {
        playerTransform = GameObject.Find("Player").transform;

        // 🔥 Oyunu açınca durum kontrolü
        if (PlayerPrefs.GetInt(habitatName + "_Unlocked", 0) == 1)
        {
            areaToActivate.SetActive(true);
            gameObject.SetActive(false);
        }

        interactionText.gameObject.SetActive(false);
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (interactionText == null)
           

        if (areaToActivate == null)
           

        if (distance <= interactionDistance)
        {
            if (!isPlayerNearby)
            {
                isPlayerNearby = true;
                interactionText.text = $"Press E to Unlock Area\nCost: {unlockCost}";
                interactionText.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                
                TryUnlock();
            }
        }
        else
        {
            if (isPlayerNearby)
            {
                isPlayerNearby = false;
                interactionText.gameObject.SetActive(false);
            }
        }
    }

    private void TryUnlock()
    {
        if (MoneyManager.Instance.CurrentMoney >= unlockCost)
        {
            MoneyManager.Instance.SpendMoney(unlockCost);
            areaToActivate.SetActive(true);
            gameObject.SetActive(false);
            interactionText.gameObject.SetActive(false);

            PlayerPrefs.SetInt(habitatName + "_Unlocked", 1); // 🔥 Kaydet
            QuestManager.Instance.RegisterHabitatUnlock();
        }
        else
        {
            interactionText.text = "Yeterli paran yok!";
        }
    }
}
