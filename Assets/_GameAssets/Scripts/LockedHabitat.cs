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
    Debug.Log("Distance to player: " + distance);

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
        Debug.Log("TryUnlock çağrıldı");
        if (MoneyManager.Instance.CurrentMoney >= unlockCost)
        {
             Debug.Log("YETERLİ PARA VAR, ALAN AÇILIYOR");
            MoneyManager.Instance.SpendMoney(unlockCost);
            interactionText.gameObject.SetActive(false);
            if (areaToActivate != null)
                areaToActivate.SetActive(true);
                Debug.Log("Yeni alan aktif edildi."); 
            
            Destroy(gameObject); // Bu cube silinir (kilitli alan açılmış olur)
           Debug.Log("Kilitli alan objesi silindi.");
        }
        else
        {
            Debug.Log("YETERLİ PARA YOK!"); 
            interactionText.text = "Not enough money to unlock!";
            Debug.Log("Yetersiz para!");
        }
    }
}
