using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light redLight;

    [SerializeField]
    private float timeout = .5f;
    private void Awake()
    {
        redLight = GetComponent<Light>();
    }
    private void OnEnable()
    {
        StartCoroutine(Flicker());
    }
    
    private IEnumerator Flicker()
    {
        while(gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(timeout);
            redLight.enabled = !redLight.enabled;
        }
    }
}
