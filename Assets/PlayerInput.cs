using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour, IPointerClickHandler
{
    private float screenHalfWidth;

    public delegate void InputTap(int direction);
    public static event InputTap OnTap;


    private void Awake()
    {
        screenHalfWidth = Screen.width/2;
    }
    public void OnPointerClick(PointerEventData data)
    {
        if(data.position.x<screenHalfWidth)
        {
            OnTap(-1);
        }
        else
        {
            OnTap(1);
        }
    }
}
