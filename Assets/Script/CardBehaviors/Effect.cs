using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{

    public enum EffectTypes {ATTACK, BOOST, HEAL, SHIELD};
    public Card self;
    public EffectTypes type;

    public void LauchEffect(int effectValue){
        switch (type)
        {
            case EffectTypes.ATTACK:
                Attack(effectValue);
                break;
            case EffectTypes.BOOST:
                Boost(effectValue);
                break;
            case EffectTypes.HEAL:
                Heal(effectValue);
                break;
            case EffectTypes.SHIELD:
                Shield(effectValue);
                break;
            default:
                break;
        }
    }

    void Attack(int effectValue){
        StartCoroutine(MainGameLoop.instance.enemy.loseEnemyHealth(effectValue));
        return;
    }
    void Boost(int effectValue){
        foreach (Card card in MainGameLoop.instance.cards)
        {
            card.addBoost(effectValue);
        }
        return;
    }
    void Heal(int effectValue){
        StartCoroutine(MainGameLoop.instance.player.gainPlayerHealth(effectValue));
        foreach (Transform trans in MainGameLoop.instance.slots)
        {
            Slot slot = trans.GetComponent<Slot>();
            StartCoroutine(slot.gainPlayerHealth(effectValue));
        }
        return;
    }
    void Shield(int effectValue){
        foreach (Card card in MainGameLoop.instance.cards)
        {
            if(card != self){
                card.giveShield();
            }
        }
        return;
    }

}
