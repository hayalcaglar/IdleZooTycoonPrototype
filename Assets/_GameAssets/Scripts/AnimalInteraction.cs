using UnityEngine;
using TMPro;

public class AnimalInteraction : MonoBehaviour
{
    private Animal animalScript;
    private Transform playerTransform;
    private bool isPlayerNearby = false;

    public float interactionDistance = 3f; 
    public TextMeshProUGUI interactionText; // ARTIK public

    private void Start()
    {
        animalScript = GetComponent<Animal>();
        playerTransform = GameObject.Find("Player").transform;
        // interactionText = GameObject.Find("InteractionText").GetComponent<TextMeshProUGUI>(); ← Bunu siliyoruz
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= interactionDistance)
        {
            if (!isPlayerNearby)
            {
                isPlayerNearby = true;
                interactionText.gameObject.SetActive(true);
                Debug.Log(animalScript.animalName + " yaklaşıldı! E'ye basarak para toplayabilirsin.");
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                CollectIncome();
                interactionText.gameObject.SetActive(false);
            }
        }
        else
        {
            if (isPlayerNearby)
            {
                isPlayerNearby = false;
                interactionText.gameObject.SetActive(false);
                Debug.Log(animalScript.animalName + " uzaklaşıldı!");
            }
        }
    }

    private void CollectIncome()
    {
        MoneyManager.Instance.AddMoney(animalScript.moneyPerClick);
        Debug.Log(animalScript.animalName + " üzerinden para toplandı: " + animalScript.moneyPerClick);
    }
}
