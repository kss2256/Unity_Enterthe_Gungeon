using Unity.VisualScripting;
using UnityEngine;
using static InputScript;
using static PlayerStatus;

public class PlayerAnimation : MonoBehaviour
{

    private Vector2 mInputVec;
    private Vector3 mMousePos;
    private SpriteRenderer mSpriteRenderer;
    private Animator mAnimator;

    private StateType mCurState;    

    private const float mRangeDegree = 22.5f;

    private void Awake()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mAnimator = GetComponent<Animator>();
       
    }


    private void Update()
    {
        
        mCurState = GetComponent<PlayerStatus>().state;
        mInputVec = GetComponent<InputScript>().InputVec;
            
        switch (mCurState)
        {
            case StateType.None:
                break;
            case StateType.Idle:
                {
                    Vector2 vecDir = animationDir();
                    mAnimator.SetBool("Walking", false);
                    mAnimator.SetFloat("IdleX", vecDir.x);
                    mAnimator.SetFloat("IdleY", vecDir.y);
                }
                break;
            case StateType.Walking:
                {
                    mAnimator.SetBool("Walking", true);
                    if (GetComponent<PlayerStatus>().weapon == WeaponType.None)
                    {
                        mAnimator.SetFloat("DirX", mInputVec.x);
                        mAnimator.SetFloat("DirY", mInputVec.y);
                    }
                    else
                    {
                        Vector2 vecDir = animationDir();
                        mAnimator.SetFloat("DirX", vecDir.x);
                        mAnimator.SetFloat("DirY", vecDir.y);
                    }
                }
                break;
            case StateType.Roll:
                {
                    mAnimator.SetBool("Roll", true);
                    Vector2 vecDir = animationDir(true);
                    mAnimator.SetFloat("RollX", vecDir.x);
                    mAnimator.SetFloat("RollY", vecDir.y);
                }
                break;
            default:
                break;
        }
       
        
    }


    private void LateUpdate()
    {
        
        if(GetComponent<PlayerStatus>().weapon == WeaponType.None 
            || (mCurState == StateType.Roll))
        {
            //Sprite 방향 전환
            if (mInputVec.x != 0)
            {
                //trut가 왼쪽
                mSpriteRenderer.flipX = mInputVec.x < 0;
            }
        }
        else
        {
            mMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pos = mMousePos - transform.position;

            mSpriteRenderer.flipX = pos.x < 0;
        }
      

    }

    private DirectionType mouseAnimationDir()
    {
        DirectionType dir;

        mMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = mMousePos - transform.position;

        float radian = Mathf.Atan2(pos.y, pos.x);
        float degree = (radian * 180) / Mathf.PI;

        if (degree < 0)
        {
            degree += 360;
        }

        float result = (degree / mRangeDegree);

        if (result > 3 && result < 5)
        {
            dir = DirectionType.Up;
        }
        else if (result > 11 && result < 13)
        {
            dir = DirectionType.Down;
        }
        else if (result > 1 && result < 3 || result > 5 && result < 7)
        {
            dir = DirectionType.DiagonalUpLeft;
        }
        else
        {
            dir = DirectionType.DiagonalDownLeft;
        }

        return dir;
    }


    private Vector2 animationDir(bool roll = false)
    {     
        Vector2 vecDir = Vector2.zero;
        DirectionType dir = GetComponent<InputScript>().direction;
        if(GetComponent<PlayerStatus>().weapon != WeaponType.None)
        {
            if (!roll)                
            dir = mouseAnimationDir();
        }


        switch (dir)
        {
            case DirectionType.Up:
                vecDir = Vector2.up;
                break;
            case DirectionType.Down:
                vecDir = Vector2.down;
                break;
            case DirectionType.Left:               
            case DirectionType.Right:
            case DirectionType.DiagonalDownLeft:               
            case DirectionType.DiagonalDownRight:
                {
                    vecDir.x = 0.7f;
                    vecDir.y = -0.7f;                   
                }              
                break;
            case DirectionType.DiagonalUpLeft:               
            case DirectionType.DiagonalUpRight:
                {
                    vecDir.x = 0.7f;
                    vecDir.y = 0.7f;
                }
                break; 
        }

        return vecDir;
    }



    public void RollCallBack()
    {
        mAnimator.SetBool("Roll", false);
        GetComponent<InputScript>().IsMove();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameObject.Find("Part").GetComponent<Part>().OnEqpmn();
    }
}
