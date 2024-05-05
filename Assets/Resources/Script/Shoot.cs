using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject mBasicBulletPreFab;
    
    Queue<Bullet> mBulletQue = new Queue<Bullet>();

    private void Awake()
    {
        Initalize(10);
        
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(mBasicBulletPreFab, transform).GetComponent<Bullet>();
        bullet.gameObject.SetActive(false);

        return bullet;
    }

    private IEnumerator ReturnBullet(Bullet _bullet)
    {
        yield return new WaitForSeconds(5);
        _bullet.gameObject.SetActive(false);       
        mBulletQue.Enqueue(_bullet);
    }

    private void Initalize(int _value)
    {
        for (int i = 0; i < _value; i++)
        {
            mBulletQue.Enqueue(CreateBullet());
        }
    }


    public void BulletFire(Vector3 _target, Vector3 _start)
    {
        Bullet bullet;

        if (mBulletQue.Count <= 0)
        {
            Initalize(5);
        }

        bullet = mBulletQue.Dequeue();
        bullet.gameObject.SetActive(true);
       
            
        Vector3 dir = _target - _start;
        dir.z = 0;



        bullet.transform.position = _start;
        bullet.Shoot(dir.normalized);
       
       
        StartCoroutine(ReturnBullet(bullet));
       
    }



}
