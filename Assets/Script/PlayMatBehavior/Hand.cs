using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private RectTransform _rt;

    void Start(){
        _rt = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData dataPointer)
    {
        if(dataPointer.dragging){
            GameObject go = dataPointer.pointerDrag;
            go.GetComponent<LayoutElement>().ignoreLayout = false;
            go.GetComponent<CanvasGroup>().alpha = 1;
            OnScreenDraggable osd = go.GetComponent<OnScreenDraggable>();
            osd._parentToReturnTo = this.transform;
            go.transform.SetParent(transform);
            go.transform.position = dataPointer.pointerPressRaycast.worldPosition;
            go.transform.localEulerAngles = new Vector3();
            go.transform.localScale = new Vector3(1,1,1);
            go.transform.SetParent(transform.parent);
            osd._isOut = false;
        }
    }

    public void OnPointerExit(PointerEventData dataPointer)
    {
        if(dataPointer.dragging){
            GameObject go = dataPointer.pointerDrag;
            go.GetComponent<LayoutElement>().ignoreLayout = true;
            go.GetComponent<CanvasGroup>().alpha = 0;
            RectTransform rt = go.GetComponent<RectTransform>();
            OnScreenDraggable osd = go.GetComponent<OnScreenDraggable>();
            go.transform.SetParent(osd._boardGame);
            go.transform.position = dataPointer.pointerPressRaycast.worldPosition;
            Debug.DrawLine(new Vector3(), dataPointer.pointerCurrentRaycast.worldPosition);
            go.transform.localEulerAngles = new Vector3();
            osd._isOut = true;
        }
    }
}
