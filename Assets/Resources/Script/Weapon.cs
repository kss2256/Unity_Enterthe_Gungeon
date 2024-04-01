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
        // 마우스의 스크린 좌표를 월드 좌표로 변환
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        

        // 오브젝트가 마우스 위치를 바라보도록 설정
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;        
        transform.localRotation = Quaternion.AngleAxis(angle + mReverseDir, Vector3.forward);


    }

}
