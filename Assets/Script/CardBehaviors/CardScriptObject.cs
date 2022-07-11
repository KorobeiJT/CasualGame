using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardScriptObject : ScriptableObject
{
    public string cardName;
    public int HP;
    public Effect.EffectTypes effect;
    public int effectValue;
    public Sprite sprite;
}
