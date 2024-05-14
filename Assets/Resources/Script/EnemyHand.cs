using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyHand : MonoBehaviour
{

    [SerializeField, Tooltip("손 오브젝트")] private GameObject handObject;
    [SerializeField, Tooltip("총 오브젝트")] private GameObject gunObject;


    private Transform targetTr;
    private Vector3 defaultHandPos;
    private bool bRightDir;
    
    private SpriteRenderer gunspriteRenderer;
    


    private void Start()
    {
        targetTr = Engine.mInstant.player.transform;
        if (handObject == null)
            Debug.Log("handObject == null");
        if (gunObject == null)
            Debug.Log("gunObject == null");

        gunspriteRenderer = gunObject.GetComponent<SpriteRenderer>();
        defaultHandPos = handObject.transform.localPosition;
    }

    private void Update()
    {

        DirectionCheak();
       

    }

    private void LateUpdate()
    {
        if(bRightDir)
        {
            handObject.transform.localPosition = defaultHandPos;
        }
        else
        {
            Vector3 pos = new Vector3(defaultHandPos.x * -1, defaultHandPos.y, defaultHandPos.z);
            handObject.transform.localPosition = pos;
            
        }

        
        gunspriteRenderer.flipX = !bRightDir;
    }


    private void WeaponRotation()
    {

    }


    private void DirectionCheak()
    {
        Vector3 pos = targetTr.position - transform.position;

        bRightDir = pos.x > 0;
    }

}
