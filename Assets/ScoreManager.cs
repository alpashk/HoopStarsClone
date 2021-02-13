using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public bool player = true;
    private bool canScore = true;

    public delegate void ScoreUpdate(bool player);
    public static event ScoreUpdate OnScore;

    public void scoreGoal()
    {
        if(canScore)
        {
            OnScore(player);
            canScore = false;
        }
    }



    private void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag =="ScoreSphere" && !collider.isTrigger)
        {
            canScore = true;
        }
    }

}
