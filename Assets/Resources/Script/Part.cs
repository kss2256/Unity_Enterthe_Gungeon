using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatus;

public class Part : MonoBehaviour
{

    public enum PartList
    {
        Hand,
        One_Handed,
        Two_Handed,
    }

    
    private Vector3 mMouseDirPos;
    private const int mDirtransform = -1;
    private bool mbIsRightDir = true;

    private SpriteRenderer mSpriteRenderer;
    private Transform mHandTr;
    private Transform mWeaponTr;

    public bool mouseIsRight
    {
        get { return mbIsRightDir; }
    }

    public Transform weaponTr
    {
        get { return mWeaponTr; }
    }



    protected virtual void Start()
    {
       
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        

    }

    protected virtual void Update()
    {
        //Player Left Right Cheak - mbIsRightDir;
        playerDirCheak();



        //테스트용 장비 탈 부착
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ClearEqpmn();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            InstEqpmn(PartList.One_Handed);
        }

    }

    protected virtual void FixedUpdate()
    {

        
      


    }

    protected virtual void LateUpdate()
    {
        turnabout();

        if (mSpriteRenderer)
        mSpriteRenderer.flipX = !mbIsRightDir;
    }

    private void playerDirCheak()
    {
       mMouseDirPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
       Transform playerTr = transform.GetComponentInParent<Player>().transform;
       mMouseDirPos = mMouseDirPos - playerTr.position;

        mbIsRightDir = mMouseDirPos.x > 0;

    }

    private void turnabout()
    {

        if (mouseIsRight)
        {
            if (transform.localPosition.x < 0)
            {
                Vector2 pos = transform.localPosition;
                pos.x *= mDirtransform;
                transform.localPosition = pos;
            }
        }
        else
        {
            if (transform.localPosition.x > 0)
            {
                Vector2 pos = transform.localPosition;
                pos.x *= mDirtransform;
                transform.localPosition = pos;
            }
        }

    }


    public void ClearEqpmn()
    {
        OffEqpmn();
        transform.parent.GetComponent<PlayerStatus>().weapon = WeaponType.None;
    }

    public void InstEqpmn(PartList _part)
    {
        if(_part == PartList.One_Handed)
        {
            Animator ani = Engine.mInstant.player.GetComponent<Animator>();
            ani.runtimeAnimatorController =
                (RuntimeAnimatorController)Resources.Load
                ("Animation\\Player\\The Hunter\\One_Hand");
        }
        else if (_part == PartList.Two_Handed)
        {
            Debug.Log("두손 무기");
        }


        mHandTr = transform.GetChild((int)PartList.Hand);
        mWeaponTr = transform.GetChild((int)_part);

        mHandTr.gameObject.SetActive(true);
        mWeaponTr.gameObject.SetActive(true);

        transform.parent.GetComponent<PlayerStatus>().weapon = (WeaponType)_part;  

        
    }


    public void OnEqpmn()
    {
        if (mHandTr != null)
            mHandTr.gameObject.SetActive(true);
        if (mWeaponTr != null)
            mWeaponTr.gameObject.SetActive(true);
    }
    public void OffEqpmn()
    {
        if (mHandTr != null)
            mHandTr.gameObject.SetActive(false);
        if (mWeaponTr != null)
            mWeaponTr.gameObject.SetActive(false);
    }



}
