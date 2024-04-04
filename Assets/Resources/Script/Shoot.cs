using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject mBasicBulletPreFab;

    private Camera mMainCamera;


    private void Awake()
    {
       
    }


    public void BulletInit(Vector2 _pos)
    {
        Vector3 mouPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mouPos - transform.position;
        

        Bullet bullet = Instantiate(mBasicBulletPreFab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.Shoot(dir.normalized);

        Debug.Log(_pos);
    }



}
