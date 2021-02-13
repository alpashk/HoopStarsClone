using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SphereBounce : MonoBehaviour
{
    [SerializeField]
    private float duration = 1.0f;

    [SerializeField]
    private float strengthDiviser = 3f;

    private bool canBounce = true;

    private Sequence bounce;

    private void Awake()
    {
        bounce = DOTween.Sequence();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canBounce)
        {
            canBounce = false;
            //transform.DOPunchPosition(collision.relativeVelocity.normalized/strengthDiviser, duration, 4, .2f);
            bounce.Append(transform.DOShakePosition(duration, collision.relativeVelocity.normalized / strengthDiviser, 3).
                OnComplete(()=> { canBounce = true;}));
        }
    }
    public void KillSequence()
    {
        DOTween.Kill(bounce);
    }

    private void OnDestroy()
    {
        KillSequence();
    }
}
