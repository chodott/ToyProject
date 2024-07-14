using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField, Range(10, 150)]
    private float leverRange;
    public type joystickType;

    private Vector2 directionVector;
    private bool isInput;
    public PlayerController playerController;
    public enum type
    {
        circle, horizontal
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 inputPos = eventData.position - rectTransform.anchoredPosition;
        if (joystickType == type.horizontal) inputPos.y = 0;
        Vector2 inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 inputPos = eventData.position - rectTransform.anchoredPosition;
        if (joystickType == type.horizontal) inputPos.y = 0;
        Vector2 inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
       
        directionVector = inputPos;
        directionVector.Normalize();
        isInput = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInput)
        {
            if (joystickType == type.horizontal) 
                playerController.Stop();
            return;
        }

        if(joystickType == type.horizontal) 
        {
            playerController.Run();
            playerController.moveDir.x = directionVector.x;
        }

        else
        {
            playerController.aimDir.x = directionVector.x;
            playerController.aimDir.z = directionVector.y;
        }

    }
}
