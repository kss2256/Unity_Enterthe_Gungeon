using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
   
    [SerializeField] private float mShakeDistance = 0.1f;
    [SerializeField] private float mShakeSpeed = 1.0f;
    [SerializeField] private float mShakeTime = 0.5f;

    private Vector3 mFixPos;
    private Vector3 mShakePos;
    private bool mbIsShakeing = false;


    public bool IsShakeing
    {
        get { return mbIsShakeing; }
    }

    private void Awake()
    {
        mFixPos = transform.position;
    }

    private void LateUpdate()
    {

        if (mbIsShakeing)
        {

            mShakePos = transform.position;

            float offsetX = mShakeDistance * Mathf.Cos(Time.time * mShakeSpeed);
            float offsetY = mShakeDistance * Mathf.Sin(Time.time * mShakeSpeed);
            transform.position = mShakePos + new Vector3(offsetX, offsetY, 0);

        }


    }

    public void CameraShaking()
    {
        mbIsShakeing = true;
        mFixPos = transform.position;
       
        StartCoroutine(cameraShaking());

    }

    private IEnumerator cameraShaking()
    {

        yield return new WaitForSeconds(mShakeTime);

    
        mbIsShakeing = false;
        transform.position = mFixPos;

    }



}
