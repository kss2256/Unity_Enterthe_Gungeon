using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{

    public enum PartList
    {
        Hand,
        Weapon,
    }


    private Transform mHandTr;
    private Transform mWeaponTr;


    private void Awake()
    {
        mHandTr = transform.GetChild((int)PartList.Hand);
        mWeaponTr = transform.GetChild((int)PartList.Weapon);



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
