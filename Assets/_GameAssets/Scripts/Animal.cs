using System.Collections;
using UnityEngine;

public class Animal : MonoBehaviour
{
    
    public int level = 1;            // Hayvanın seviyesi
    public int upgradeCost = 50;     // İlk upgrade maliyeti

    public string animalName = "Cow"; // Hayvanın ismi
    public int moneyPerClick = 10;     // Hayvana tıklayınca kazanılan para

   private bool isJumping = false;
   

private void OnMouseDown()
{
    if (!isJumping)
    {
        StartCoroutine(JumpEffect());
        MoneyManager.Instance.AddMoney(moneyPerClick);
        GameManager.Instance.OpenUpgradePanel(this); // Hayvan upgrade panelini aç
    }
}

private IEnumerator JumpEffect()
{
    isJumping = true;

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
    isJumping = false;
}

  public void Upgrade()
{
    if (MoneyManager.Instance.CurrentMoney >= upgradeCost)
    {
        MoneyManager.Instance.SpendMoney(upgradeCost);

        level++;
        moneyPerClick = Mathf.RoundToInt(moneyPerClick * 1.1f);
        upgradeCost = Mathf.RoundToInt(upgradeCost * 1.5f);

        Debug.Log(animalName + " upgrade oldu! Yeni level: " + level);

        // 🔥 Görev ilerletme
        QuestManager.Instance.OnAnimalUpgraded(this);

        // 🔥 Barı güncelle (sadece başarılı upgrade sonrası)
        GameManager.Instance.UpdateUpgradeProgress(level);
    }
    else
    {
        Debug.Log("Yeterli paran yok!");
    }
}



}
