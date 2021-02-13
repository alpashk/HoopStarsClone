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

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            move.jump(-1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            move.jump(1);
        }
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
