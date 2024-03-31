using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{

    public enum PartList
    {
        Hand,
    }


    private Transform mHandTr;


    private void Awake()
    {
        mHandTr = transform.GetChild((int)PartList.Hand);



    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Q))
        {
            mHandTr.gameObject.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            mHandTr.gameObject.SetActive(false);
        }





    }



}
