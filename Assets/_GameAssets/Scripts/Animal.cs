using System.Collections;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public int level = 1;            // Hayvanın seviyesi
    public int upgradeCost = 50;     // İlk upgrade maliyeti

    public string animalName = "Cow"; // Hayvanın ismi
    public int moneyPerClick = 10;     // Hayvana tıklayınca kazanılan para

    private void OnMouseDown()
    {
        StartCoroutine(JumpEffect());
        MoneyManager.Instance.AddMoney(moneyPerClick);
    }

    private IEnumerator JumpEffect()
    {
        Vector3 startPosition = transform.position;
        Vector3 jumpUpPosition = startPosition + new Vector3(0, 0.5f, 0);

        float elapsed = 0f;
        float duration = 0.2f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, jumpUpPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = jumpUpPosition;

        elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(jumpUpPosition, startPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
    }

    public void Upgrade()
    {
        if (MoneyManager.Instance.CurrentMoney >= upgradeCost)
        {
            MoneyManager.Instance.SpendMoney(upgradeCost); // ✅ Doğru kullanım: SpendMoney ile eksilt

            level++;
            moneyPerClick = Mathf.RoundToInt(moneyPerClick * 1.1f); // %10 artış
            upgradeCost = Mathf.RoundToInt(upgradeCost * 1.5f);      // Maliyet 1.5x artıyor

            Debug.Log(animalName + " upgrade oldu! Yeni level: " + level + ", Yeni gelir: " + moneyPerClick + ", Yeni upgrade maliyeti: " + upgradeCost);
            QuestManager.Instance.RegisterUpgrade(); // Görev yöneticisine upgrade kaydı yapıyoruz
        }
        else
        {
            Debug.Log("Yeterli paran yok! Upgrade yapılamıyor.");
        }
        QuestManager.Instance.RegisterUpgrade();
        // Görev yöneticisine upgrade kaydı yapıyoruz
    }
}
