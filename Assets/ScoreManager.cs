using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public bool player = true;
    private bool canScore = true;
    private bool colliding = false; //���������� �� ������, ���� ����� �������� ��� ��� ������������ � �����, 
    //�.� ��������� �������� � ��������������� ������ � �������������� ������

    public delegate void ScoreUpdate(bool player);
    public static event ScoreUpdate OnScore;

    public void scoreGoal()
    {
        if(canScore)
        {
            OnScore(player);
            canScore = false;
            if (!colliding)
            {
                StartCoroutine(WaitToScore());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ScoreSphere")
        {
            colliding = true;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag =="ScoreSphere")
        {
            colliding = false;
            canScore = true;
        }
    }

    private IEnumerator WaitToScore()
    {
        yield return new WaitForSeconds(.2f);
        canScore = true;
    }

}
