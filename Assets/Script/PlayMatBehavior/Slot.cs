using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private int index;
    public GameObject creaturePrefab;
    public GameObject creature;
    public Card selfCard;
    public Image cardHealthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        index = int.Parse(name[name.Length - 1].ToString());
    }

    public void drop(Card card){
        selfCard = card;
        MainGameLoop.instance.newCard(index);
        //Invoc a thing to illustrate the card
        creature = Instantiate(creaturePrefab, transform.position + Vector3.up, Quaternion.identity, transform) as GameObject;
        creature.transform.GetChild(0).GetComponent<changeMaterial>().change(card.get_effect().type);
        cardHealthBar = transform.Find("/Overlay/CreatureHealthBar"+index.ToString()+"/CreatureHealth").GetComponent<Image>();
        cardHealthBar.transform.parent.GetComponent<CanvasGroup>().alpha = 1;
        cardHealthBar.fillAmount = (float)((float)selfCard._CurrentHP / (float)selfCard._MaxHP);
    }

    public int getIndex()
    {
        return this.index;
    }


    public IEnumerator losePlayerHealth(int value)
    {
        selfCard._CurrentHP = Mathf.Max(selfCard._CurrentHP - value, 0);
        yield return StartCoroutine(barGoingDown());
    }

    public IEnumerator gainPlayerHealth(int value)
    {
        selfCard._CurrentHP = Mathf.Min(selfCard._CurrentHP + value, selfCard._MaxHP);
        yield return StartCoroutine(barGoingUp());
    }

    IEnumerator barGoingDown()
    {
        while (cardHealthBar.fillAmount > (float)((float)selfCard._CurrentHP / (float)selfCard._MaxHP))
        {
            cardHealthBar.fillAmount -= Time.deltaTime;
            yield return null;
        }
        if(selfCard._CurrentHP<=0){
            cardHealthBar.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
    IEnumerator barGoingUp()
    {
        while (cardHealthBar.fillAmount < (float)((float)selfCard._CurrentHP / (float)selfCard._MaxHP))
        {
            cardHealthBar.fillAmount += Time.deltaTime;
            yield return null;
        }
    }
}
