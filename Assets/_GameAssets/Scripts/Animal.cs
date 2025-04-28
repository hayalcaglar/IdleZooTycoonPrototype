using System.Collections;
using UnityEngine;

public class Animal : MonoBehaviour
{
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
}
