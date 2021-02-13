using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public static Transform player;


    private readonly float YborderMax = 8.5f; //����������, ������ ������� ����� �� ����� �����
    private readonly float YborderMin = -2.5f;

    private readonly float Ymax = 1; //������������ ����������� ������ �� ����

    private float yTotal;
    private void Awake()
    {
        yTotal = YborderMax - YborderMin;
    }

    private void FixedUpdate()
    {
        float yPlayerPos = (player.position.y - YborderMin) / yTotal; //� ����� ���� ����� �������� ���� ��������� ����� �� ��� Y
        if (yPlayerPos > 1)
            yPlayerPos = 1;
        float yCoord = -1 * Ymax + yPlayerPos * 2 * Ymax;


        transform.position = new Vector3(0, yCoord, transform.position.z);
    }
}
