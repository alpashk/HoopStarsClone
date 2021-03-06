using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDetection : MonoBehaviour
{
    public static ScoreDetection score;
    private void OnEnable()
    {
        score = this;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.isTrigger)
        {
            collider.gameObject.GetComponent<ScoreManager>().scoreGoal();
        }
    }
}
