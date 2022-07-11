using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameLoop : MonoBehaviour
{
    public CardScriptObject[] cardInfos;
    public GameObject cardPrefab;
    public Enemy enemy;
    public Player player;
    public Transform[] slots;
    public Transform hand;
    public Card[] cards;
    public Button buttonTurnLauch;
    public static MainGameLoop instance;
    public Image endGameScreen;
    public Sprite Victory;
    public Sprite Defeat;

    void Awake()
    {
        instance = this;
    }

    void Start(){
        endGameScreen = transform.Find("/Overlay/EndGame").GetComponent<Image>();
        endGameScreen.gameObject.SetActive(false);
        slots = new Transform[3];
        cards = new Card[3];
        for (int i = 0; i < 3; i++)
        {
            slots[i] = transform.Find("/BoardGame/Panel" + i.ToString());
        }
        hand = transform.Find("/Overlay/Hand");
        buttonTurnLauch = transform.Find("/Overlay/ButtonTurnLauch").GetComponent<Button>();
        player = transform.Find("/Player").GetComponent<Player>();
        enemy = transform.Find("/Enemy").GetComponent<Enemy>();
        for (int i = 0; i < 5; i++)
        {
            drawCard();
        }
    }

    public void newCard(int index){
        cards[index] = slots[index].GetChild(0).GetComponent<Card>();
        foreach (Card card in cards)
        {
            if(card == null){
                return;
            }
        }
        buttonTurnLauch.gameObject.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void drawCard(){
        GameObject card = Instantiate(cardPrefab,hand) as GameObject;
        int rand = Random.Range(0,4);
        card.GetComponent<Card>()._cardInfos = cardInfos[rand];
    }

    IEnumerator CoroutineTurn(){
        foreach (Card item in cards)
        {
            item.launchEffect();
            yield return new WaitForSeconds(0.5f);
        }
        if(enemy.currentEnemyHealth<=0){
            Win();
            yield break;
        }
        StartCoroutine(enemyAttack());
    }

    public void LaunchTurn(){
        buttonTurnLauch.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        
        StartCoroutine(CoroutineTurn());
    }

    IEnumerator enemyAttack(){
        foreach (Card card in cards)
        {
            int id = card.transform.parent.GetComponent<Slot>().getIndex();
            if(card._isShield){
                card._isShield = false;
                Destroy(card.transform.parent.Find("Creature(Clone)").Find("Shield(Clone)").gameObject);
                continue;
            }
            yield return StartCoroutine(slots[id].GetComponent<Slot>().losePlayerHealth(enemy.attack));
            if(card._CurrentHP<=0){
                slots[id].GetComponent<ResizeToFit>().dropped = false;
                slots[id].GetComponent<Slot>().selfCard = null;
                Destroy(slots[id].GetComponent<Slot>().creature);
                Destroy(card.gameObject);
                buttonTurnLauch.gameObject.GetComponent<CanvasGroup>().alpha = 0;
                drawCard();
                cards[id] = null;
            }
        }
        bool verifallCard = true;
        for (int i = 0; i < 3; i++)
        {
            if(cards[i] == null){
                verifallCard = false;
            }
        }
        if(verifallCard){
            buttonTurnLauch.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }
        yield return StartCoroutine(player.losePlayerHealth(enemy.attack));
        if(player.currentPlayerHealth<=0){
            Lose();
        }
    }

    void Win(){
        endGameScreen.gameObject.SetActive(true);
        endGameScreen.sprite = Victory;
        CanvasGroup rb = transform.Find("/Overlay/RestartGame").GetComponent<CanvasGroup>();
        rb.alpha = 1;
        rb.interactable = true;
        rb.blocksRaycasts = true;
    }
    void Lose(){
        endGameScreen.gameObject.SetActive(true);
        endGameScreen.sprite = Defeat;
        CanvasGroup rb = transform.Find("/Overlay/RestartGame").GetComponent<CanvasGroup>();
        rb.alpha = 1;
        rb.interactable = true;
        rb.blocksRaycasts = true;
    }

    public void reloadScene(){
        SceneManager.LoadScene("MainScene",LoadSceneMode.Single);
    }
}
