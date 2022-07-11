using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnScreenDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private bool _isDragging;
    public bool _isOnSpot;
    public bool _isOut;
    [SerializeField] private Vector3 _currentPos;
    public Transform _overLay;
    public Transform _boardGame;
    public Transform _parentToReturnTo;
    private Rigidbody _rigidBody;
    public static int _DownYPosition = 0;
    public static int _UpYPosition = 2;

    private void Start()
    {
        _isOut=false;
        _rigidBody = this.GetComponent<Rigidbody>();
        _boardGame = transform.Find("/BoardGame"); // HARD CODED !!
        _overLay = transform.Find("/Overlay"); // HARD CODED !!
    }

    public void OnDrag(PointerEventData dataPointer)
    {
        this._currentPos = dataPointer.pointerCurrentRaycast.worldPosition;
    }

    public void OnBeginDrag(PointerEventData dataPointer)
    {
        _isDragging = true;
        _parentToReturnTo = this.transform.parent;
        this.transform.SetParent(_overLay);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        StartCoroutine(MoveCoroutine());
        // StartCoroutine(UpTheCard());
    }

    // Remember : OnEndDrag is triggered AFTER OnDrop
    public void OnEndDrag(PointerEventData dataPointer)
    {
        _isDragging = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        transform.SetParent(_parentToReturnTo);
        transform.localEulerAngles = new Vector3();
        // StartCoroutine(DownTheCard());
    }

    IEnumerator MoveCoroutine()
    {
        while (_isDragging)
        {
            if(!_isOnSpot){
                Debug.DrawLine(new Vector3(), _currentPos);
                if (Vector3.Distance(this._currentPos, this.transform.position) <= 0.05) //HARD CODED
                {
                    // If the card is near the cursor, cancel all force applied
                    _rigidBody.velocity = _rigidBody.velocity * 0.8f;
                }
                else
                {
                    if(_currentPos.magnitude!=0){
                        if(!_isOut){
                            // Create a force that pull the card toward the position of the cursor
                            Vector3 force = (this._currentPos - this.transform.position) * 200; //HARD CODED
                            _rigidBody.AddForce(force);
                            // Set the orientation of the card in order to make fill like the cursor drag physically the card
                            Vector3 lookAtPoint = _currentPos + getNormalOfCanvas() * 10;
                            this.transform.rotation = Quaternion.LookRotation(transform.position - lookAtPoint);
                            this.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
                        } else {
                            transform.position = Vector3.MoveTowards(transform.position, _currentPos, 10*Time.deltaTime);
                        }
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator UpTheCard()
    {
        while (Vector3.Dot(transform.position, getNormalOfCanvas()) <= _UpYPosition)
        {
            _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, 1, _rigidBody.velocity.z);
            yield return null;
        }
        _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, 0, _rigidBody.velocity.z);
        transform.position = getNormalOfCanvas() * _UpYPosition + transform.position;
    }

    IEnumerator DownTheCard()
    {
        while (Vector3.Dot(transform.position, getNormalOfCanvas()) >= _DownYPosition)
        {
            _rigidBody.velocity = new Vector3(0, -1, 0);
            yield return null;
        }
        _rigidBody.velocity = new Vector3();
        transform.position = getNormalOfCanvas() * _DownYPosition + transform.position;
    }

    Vector3 getNormalOfCanvas(){
        Vector3 firstVector = transform.position - transform.parent.position;
        Vector3 secondVector = _currentPos - transform.parent.position;
        Debug.DrawLine(transform.parent.position, transform.parent.position + secondVector);
        Debug.DrawLine(transform.parent.position, transform.parent.position + firstVector);
        Vector3 res = Vector3.Normalize(Vector3.Cross(firstVector, secondVector));
        if(res.x * res.y * res.z < 0){
            res = -res;
        }
        Debug.DrawLine(transform.parent.position, transform.parent.position + res);
        return res;
    }

}
