using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
    
}
