using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Image enemyHealthBar;
    public int currentEnemyHealth;
    public int maxEnemyHealth;

    public int attack;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealthBar = transform.Find("/Overlay/EnemyHealthBar/EnemyHealth").GetComponent<Image>();
    }

    public IEnumerator loseEnemyHealth(int value)
    {
        currentEnemyHealth -= value;
        yield return StartCoroutine(barGoingDown());
    }

    public IEnumerator gainEnemyHealth(int value)
    {
        currentEnemyHealth += value;
        yield return StartCoroutine(barGoingUp());
    }

    IEnumerator barGoingDown()
    {
        while (enemyHealthBar.fillAmount > (float)((float)currentEnemyHealth / (float)maxEnemyHealth))
        {
            enemyHealthBar.fillAmount -= Time.deltaTime * 0.5f;
            yield return null;
        }
    }
    IEnumerator barGoingUp()
    {
        while (enemyHealthBar.fillAmount < (float)((float)currentEnemyHealth / (float)maxEnemyHealth))
        {
            enemyHealthBar.fillAmount += Time.deltaTime * 0.5f;
            yield return null;
        }
    }
    
}
