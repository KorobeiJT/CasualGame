using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeToFit : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Slot slot;
    public bool dropped;
    private OnScreenDraggable cardDropped;
    private void Start() {
        slot = GetComponent<Slot>();
    }

    public void OnPointerEnter(PointerEventData dataPointer) {
        if(dataPointer.dragging){
            GameObject so = dataPointer.pointerDrag;
            so.GetComponent<OnScreenDraggable>()._isOnSpot = true;
            if(!dropped){
                so.GetComponent<CanvasGroup>().alpha = 1;
            }
            dataPointer.pointerDrag.transform.SetParent(this.transform);
            RectTransform sort = so.GetComponent<RectTransform>();
            RectTransform rt = this.GetComponent<RectTransform>();
            so.GetComponent<Rigidbody>().velocity = new Vector3();
            sort.anchorMin = new Vector2(0.5f,0.5f);
            sort.anchorMax = new Vector2(0.5f,0.5f);
            sort.anchoredPosition = new Vector2(0.5f,0.5f);
            so.transform.localScale = new Vector3(rt.sizeDelta.x/sort.sizeDelta.x, rt.sizeDelta.y/sort.sizeDelta.y, 0.1f);
            so.transform.localPosition = new Vector3();
            so.transform.localEulerAngles = new Vector3();
        }
    }
    public void OnPointerExit(PointerEventData dataPointer) {
        if(dataPointer.dragging){
            GameObject so = dataPointer.pointerDrag;
            so.GetComponent<OnScreenDraggable>()._isOnSpot = false;
            so.GetComponent<CanvasGroup>().alpha = 0;
            if(dropped && cardDropped == so.GetComponent<OnScreenDraggable>()){
                GameObject.Destroy(slot.creature.transform.gameObject);
                dropped = false;
                MainGameLoop.instance.cards[slot.getIndex()] = null;
                MainGameLoop.instance.buttonTurnLauch.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
    }

    // Remember : OnDrop is triggered BEFORE OnEndDrag
    public void OnDrop(PointerEventData dataPointer){
        OnScreenDraggable d = dataPointer.pointerDrag.GetComponent<OnScreenDraggable>();
        Debug.Assert(d != null, "Draggable Object without Dragglable Script !");
        d._parentToReturnTo = this.transform;
        slot.drop(d.GetComponent<Card>());
        dropped = true;
        cardDropped = d;
    }
}
