using System.Collections;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public int level = 1;
    public int upgradeCost = 50;
    public string animalName = "Cow";
    public int moneyPerClick = 10;

    private bool isJumping = false;

    private void Start()
    {
        level = PlayerPrefs.GetInt(animalName + "_Level", level);
        moneyPerClick = Mathf.RoundToInt(10 * Mathf.Pow(1.1f, level - 1));
        upgradeCost = Mathf.RoundToInt(50 * Mathf.Pow(1.5f, level - 1));
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(animalName + "_Level", level);
    }

    private void OnMouseDown()
    {
        if (!isJumping)
        {
            StartCoroutine(ScaleEffect());
            StartCoroutine(JumpEffect());
            PlayClickEffect();
            PlayClickSound();
            MoneyManager.Instance.AddMoney(moneyPerClick);
            GameManager.Instance.OpenUpgradePanel(this);
        }
    }

    private IEnumerator JumpEffect()
    {
        isJumping = true;
        Vector3 startPosition = transform.position;
        Vector3 jumpUpPosition = startPosition + new Vector3(0, 0.5f, 0);
        float elapsed = 0f, duration = 0.2f;
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

    private IEnumerator ScaleEffect()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 2.2f;
        float duration = 0.1f, elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
        elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
    }

    public void Upgrade()
    {
        if (MoneyManager.Instance.CurrentMoney >= upgradeCost)
        {
            MoneyManager.Instance.SpendMoney(upgradeCost);
            level++;
            moneyPerClick = Mathf.RoundToInt(10 * Mathf.Pow(1.1f, level - 1));
            upgradeCost = Mathf.RoundToInt(50 * Mathf.Pow(1.5f, level - 1));
            
            QuestManager.Instance.OnAnimalUpgraded(this);
            GameManager.Instance.UpdateUpgradeProgress(level);
        }
        else
        {
            Debug.Log("Yeterli paran yok!");
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(animalName + "_Level", level);
    }

    public override bool Equals(object obj)
    {
        if (obj is Animal other)
            return animalName == other.animalName;
        return false;
    }

    public override int GetHashCode()
    {
        return animalName.GetHashCode();
    }

    private void PlayClickEffect()
    {
        GameObject effect = Instantiate(Resources.Load<GameObject>("ClickEffect"), transform.position, Quaternion.identity);
        Destroy(effect, 1f);
    }

    private void PlayClickSound()
    {
        AudioClip clip = Resources.Load<AudioClip>("ClickSound");
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
