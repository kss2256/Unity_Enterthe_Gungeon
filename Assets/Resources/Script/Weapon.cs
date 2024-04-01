using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Weapon : MonoBehaviour
{

    private const float mReverseDir = -180;



    private void Awake()
    {
        
    }


    private void FixedUpdate()
    {
        // ���콺�� ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        

        // ������Ʈ�� ���콺 ��ġ�� �ٶ󺸵��� ����
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;        
        transform.localRotation = Quaternion.AngleAxis(angle + mReverseDir, Vector3.forward);


    }

}
