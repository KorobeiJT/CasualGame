using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Image playerHealthBar;
    public int currentPlayerHealth;
    public int maxPlayerHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerHealthBar = transform.Find("/Overlay/PlayerHealthBar/PlayerHealth").GetComponent<Image>();
    }

    public IEnumerator losePlayerHealth(int value)
    {
        currentPlayerHealth = Mathf.Max(currentPlayerHealth - value, 0);
        yield return StartCoroutine(barGoingDown());
    }

    public IEnumerator gainPlayerHealth(int value)
    {
        currentPlayerHealth = Mathf.Min(currentPlayerHealth + value, maxPlayerHealth);
        yield return StartCoroutine(barGoingUp());
    }

    IEnumerator barGoingDown()
    {
        while (playerHealthBar.fillAmount > (float)((float)currentPlayerHealth / (float)maxPlayerHealth))
        {
            playerHealthBar.fillAmount -= Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator barGoingUp()
    {
        while (playerHealthBar.fillAmount < (float)((float)currentPlayerHealth / (float)maxPlayerHealth))
        {
            playerHealthBar.fillAmount += Time.deltaTime;
            yield return null;
        }
    }

}
