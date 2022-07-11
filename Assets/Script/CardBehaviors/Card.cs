using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public CardScriptObject _cardInfos;
    [SerializeField] private string _cardName;
    public int _MaxHP;
    public int _CurrentHP;
    [SerializeField] private Effect _effect;
    [SerializeField] private int _effectValue;
    [SerializeField] private int _boostValue;
    [SerializeField] private Image _portrait;
    public bool _isShield;
    [SerializeField] private TextMeshProUGUI _cardNameText;
    [SerializeField] private TextMeshProUGUI _HPText;
    [SerializeField] private Image _effectImage;
    [SerializeField] private Sprite _attackImage;
    [SerializeField] private Sprite _boostImage;
    [SerializeField] private Sprite _healImage;
    [SerializeField] private Sprite _shieldImage;
    [SerializeField] private TextMeshProUGUI _effectValueText;

    [SerializeField] private GameObject _shieldPrefab;


    void Start()
    {
        _cardName = _cardInfos.cardName;
        _cardNameText = transform.Find("CardName").GetComponent<TextMeshProUGUI>();
        _cardNameText.SetText(_cardName);
        _MaxHP = _cardInfos.HP;
        _HPText = transform.Find("Health").Find("Health").GetComponent<TextMeshProUGUI>();
        _HPText.SetText(_MaxHP.ToString());
        _CurrentHP = _cardInfos.HP;
        _effect = this.gameObject.AddComponent<Effect>();
        _effect.type = _cardInfos.effect;
        _effect.self = this;
        _effectImage = transform.Find("EffectSprite").GetComponent<Image>();
        _effectValue = _cardInfos.effectValue;
        _effectValueText = transform.Find("EffectSprite/EffectValue").GetComponent<TextMeshProUGUI>();
        _effectValueText.SetText(_effectValue.ToString());
        LoadImage();
        _boostValue = 0;
        _portrait = transform.Find("Portrait").GetComponent<Image>();
        _portrait.sprite = _cardInfos.sprite;
    }

    void LoadImage(){
        switch (_effect.type)
        {
            case Effect.EffectTypes.ATTACK:
                _effectImage.sprite = _attackImage;
                break;
            case Effect.EffectTypes.BOOST:
                _effectImage.sprite = _boostImage;
                break;
            case Effect.EffectTypes.HEAL:
                _effectImage.sprite = _healImage;
                break;
            case Effect.EffectTypes.SHIELD:
                _effectImage.sprite = _shieldImage;
                _effectValueText.SetText("");
                break;
            default:
                break;
        }
    }

    public int get_HP()
    {
        return this._CurrentHP;
    }
    public int get_effectValue()
    {
        return this._effectValue;
    }

    public void launchEffect(){
        _effect.LauchEffect(_effectValue + _boostValue);
        _boostValue = 0;
    }

    public void addBoost(int value){
        _boostValue += value;
    }

    public void giveShield(){
        if(!_isShield){
            Transform creature = transform.parent.Find("Creature(Clone)");
            Instantiate(_shieldPrefab, creature.position, creature.rotation, creature);
        }
        _isShield = true;
    }
    
    public Effect get_effect()
    {
        return this._effect;
    }

    public string get_cardName()
    {
        return this._cardName;
    }

}
