using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapToStart : MonoBehaviour, IPointerClickHandler
{
    public delegate void StartTap();
    public static event StartTap OnStart;


    public void OnPointerClick(PointerEventData data)
    {
        OnStart();
        gameObject.SetActive(false);
    }


}
