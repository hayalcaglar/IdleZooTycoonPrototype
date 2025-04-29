using UnityEngine;
using TMPro;

public class LockedHabitat : MonoBehaviour
{

    
    public int unlockCost = 100; // Açma maliyeti
    public TextMeshProUGUI interactionText;
    private Transform playerTransform;
    private bool isPlayerNearby = false;

    public GameObject areaToActivate; // Yeni hayvan veya alan (açılınca aktif edilecek)

    public float interactionDistance = 10f;

    private void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        interactionText.gameObject.SetActive(false);
    }

 private void Update()
{
    float distance = Vector3.Distance(transform.position, playerTransform.position);
    Debug.Log("Update çalışıyor.");
    // Debug.Log("Distance to player: " + distance);

    if (interactionText == null)
        Debug.LogWarning("interactionText bağlı değil!");

    if (areaToActivate == null)
        Debug.LogWarning("areaToActivate bağlı değil!");

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
            Debug.Log("E tuşuna basıldı.");
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

        QuestManager.Instance.RegisterHabitatUnlock(); // ✅ Görev bildir
    }
    else
    {
        interactionText.text = "Yeterli paran yok!";
    }
}

}
