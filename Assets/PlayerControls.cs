using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private MovementScript move;
    private void Awake()
    {
        move = GetComponent<MovementScript>();
        PlayerInput.OnTap += GetInput;
    }
    private void GetInput(int direction)
    {
        move.jump(direction);
    }

    private void OnDestroy()
    {
        PlayerInput.OnTap -= GetInput;
    }
}
