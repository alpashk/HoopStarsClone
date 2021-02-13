using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{
    private MovementScript move;
    private Transform sphere;
    private Rigidbody enemyRigidbody;

    private int layerMask;

    [SerializeField] 
    private float moveCooldown = 0.4f;

    [Tooltip("When enemy is this close to ball, it will not do anything")]
    [SerializeField] 
    private float allowedDistanceToBall = .5f;

    [Tooltip("Enemy checks if it is moving towards the score sphere using a raycast of this length")]
    [SerializeField] 
    private float raycastDistance = 3f;


    private void Awake()
    {
        layerMask = 1 << 3;
        move = GetComponent<MovementScript>();
        enemyRigidbody = GetComponent<Rigidbody>();
        ScoreManager.OnScore += UpdateSpherePos;
    }

    private void Start()
    {
        UpdateSpherePos(false);
        StartCoroutine(WaitToAct());
    }

    private void UpdateSpherePos(bool player)
    {
        sphere = ScoreDetection.score.transform;

    }

    private IEnumerator WaitToAct()
    {
        while(true)
        {
            int direction = PositionCheck();
            if(direction !=0)
            {
                move.jump(direction);
            }

            yield return new WaitForSeconds(moveCooldown);
        }
    }

    private int PositionCheck()
    {
        Vector3 enemyPos = transform.position;
        Vector3 spherePos = sphere.position;

        RaycastHit sphereHit;
        if(Physics.Raycast(enemyPos, enemyRigidbody.velocity.normalized,out sphereHit, raycastDistance, layerMask ))
        {
            return 0; //if enemy is moving toards the sphere it does nothing
        }


        if (enemyPos.y > spherePos.y + allowedDistanceToBall && enemyPos.x > spherePos.x - allowedDistanceToBall
            && enemyPos.x < spherePos.x + allowedDistanceToBall)
        {
            return 0;

        }
        else if(enemyPos.y> spherePos.y)
        {
            return 0;
        }


        if(enemyPos.y < spherePos.y - allowedDistanceToBall && enemyPos.x < spherePos.x-allowedDistanceToBall)
        {
            return 1;
        }
        else if (enemyPos.y < spherePos.y - allowedDistanceToBall && enemyPos.x > spherePos.x + allowedDistanceToBall)
        {
            return -1;
        }
        if (enemyPos.y < spherePos.y - allowedDistanceToBall)
        {
            int randomize = Random.Range(0, 101);
            if (randomize % 2 == 0)
            {
                return -1;
            }
            else 
            {
                return 1;
            }
        }

        return 0;

    }



    private void OnDestroy()
    {
        ScoreManager.OnScore -= UpdateSpherePos;
    }
}
