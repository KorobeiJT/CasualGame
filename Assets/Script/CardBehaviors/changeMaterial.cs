using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeMaterial : MonoBehaviour
{
    public void change(Effect.EffectTypes type){
        switch (type)
        {
            case Effect.EffectTypes.ATTACK:
                GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case Effect.EffectTypes.BOOST:
                GetComponent<MeshRenderer>().material.color = Color.yellow;
                break;
            case Effect.EffectTypes.HEAL:
                GetComponent<MeshRenderer>().material.color = Color.green;
                break;
            case Effect.EffectTypes.SHIELD:
                GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
            default:
                break;
        }
    }
}
