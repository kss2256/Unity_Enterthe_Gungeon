using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    private Vector3 mMouseDirPos;



    private void Awake()
    {
       


    }


    private void Update()
    {

        mMouseDirPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Transform playerTr = transform.GetComponentInParent<Player>().transform;
        mMouseDirPos = playerTr.position - mMouseDirPos;
        if(mMouseDirPos.x > 0)
        {
            transform.position *= Vector2.right;
        }
        else
        {
            transform.position *= Vector2.left;
        }


    }





}
