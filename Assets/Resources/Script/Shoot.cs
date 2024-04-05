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


    public void BulletInit()
    {
        Bullet bullet;

        if (mBulletQue.Count <= 0)
        {
            Initalize(5);
        }

        bullet = mBulletQue.Dequeue();
        bullet.gameObject.SetActive(true);

        Vector3 weaponpos = GameObject.Find("Part").GetComponent<Part>().weaponTr.position;


        Vector3 mouPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);        
        Vector3 dir = mouPos - weaponpos;
        dir.z = 0;



        bullet.transform.position = weaponpos;
        bullet.Shoot(dir.normalized);
       


        Debug.Log(mBulletQue.Count);
        StartCoroutine(ReturnBullet(bullet));
       
    }



}
