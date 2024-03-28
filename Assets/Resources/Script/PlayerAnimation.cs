using UnityEngine;
using static InputScript;
using static PlayerStatus;

public class PlayerAnimation : MonoBehaviour
{

    private Vector2 mInputVec;
    private SpriteRenderer mSpriteRenderer;
    private Animator mAnimator;

    private StateType mCurState;
    private StateType mPrevState;


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
                    if(mPrevState != StateType.Idle)
                    {
                        Vector2 vecDir = dir();
                        mAnimator.SetBool("Walking", false);
                        mAnimator.SetFloat("IdleX", vecDir.x);
                        mAnimator.SetFloat("IdleY", vecDir.y);
                    }                   
                }
                break;
            case StateType.Walking:
                {
                    mAnimator.SetBool("Walking", true);
                    mAnimator.SetFloat("DirX", mInputVec.x);
                    mAnimator.SetFloat("DirY", mInputVec.y);
                }
                break;
            case StateType.Roll:
                break;
            default:
                break;
        }
        mPrevState = mCurState;
        
    }


    private void LateUpdate()
    {
        //Sprite 방향 전환
        if (mInputVec.x != 0)
        {
            //trut가 왼쪽
            mSpriteRenderer.flipX = mInputVec.x < 0;
        }
    }


    private Vector2 dir()
    {     
        Vector2 vecDir = Vector2.zero;
        switch (GetComponent<InputScript>().direction)
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
}
