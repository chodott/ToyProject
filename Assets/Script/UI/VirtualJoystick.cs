using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField, Range(10, 150)]
    private float leverRange;
    [SerializeField]
    private type joystickType;

    private Vector2 directionVector;
    private bool isInput;
    public PlayerController playerController;

    //юс╫ц©К
    //public ItemSpawner itemSpawner;
    public enum type
    {
        circle, horizontal
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        lever = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AdjustLever(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        AdjustLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;

        //itemSpawner.SpawnBox();
    }

    public void AdjustLever(PointerEventData eventData)
    {
        Vector2 inputPos;
        if (joystickType == type.horizontal)
        {
            inputPos = eventData.position - rectTransform.anchoredPosition;
            inputPos.y = 0;
        }
        else
        {
            inputPos = new Vector2(-Screen.width + eventData.position.x, eventData.position.y) - rectTransform.anchoredPosition;
        }
        Vector2 inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;

        directionVector = inputPos;
        directionVector.Normalize();
        isInput = true;
    }

    void Update()
    {
        if (!isInput)
        {
            if (joystickType == type.horizontal)
            {
                playerController.Stop();
            }
            
            return;
        }

        if(joystickType == type.horizontal) 
        {
            playerController.Run(directionVector);
        }

        else
        {
            playerController.Turn(directionVector);
        }
    }
}
