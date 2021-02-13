using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovementScript : MonoBehaviour
{
    [SerializeField]
    private float jumpStrength = 7.0f;

    [Range(0, 90)]
    [SerializeField]
    private float jumpAngle = 55.0f;

    [SerializeField]
    private float bounceStrengthDiviser = 2;

    private Rigidbody playerRigidbody;
    private static float xForce;
    private static float yForce;
    private readonly float UpperBorder = 8.5f;

    private bool canBounce = true;


    // Start is called before the first frame update
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        float degreeConversion = (float)(jumpAngle * System.Math.PI / 180);
        xForce = (float)System.Math.Cos(degreeConversion);
        yForce = (float)System.Math.Sin(degreeConversion);
    }

    public void jump(int direction)
    {
        if (transform.position.y < UpperBorder) 
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.AddForce(new Vector3(xForce * direction, yForce, 0) * jumpStrength, ForceMode.VelocityChange);
        }
    }


    //Fix this
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Borders" && canBounce)
        {
            canBounce = false;
            StartCoroutine(BounceTimeOut());

            int direction;
            if (transform.position.x > collision.transform.position.x)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
            playerRigidbody.AddForce(Vector3.left * jumpStrength * -1 / bounceStrengthDiviser * direction,
                ForceMode.VelocityChange);
        }
    }

    private IEnumerator BounceTimeOut()
    {
        yield return new WaitForSeconds(0.1f);
        canBounce = true;
    }

}
