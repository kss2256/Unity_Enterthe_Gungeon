using UnityEngine;
using static InputScript;
using static PlayerStatus;

public class PlayerAnimation : MonoBehaviour
{

    private Vector2 mInputVec;
    private SpriteRenderer mSpriteRenderer;
    private Animator mAnimator;

    public StateType mCurState;
    private StateType mPrevState;


    private void Awake()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mAnimator = GetComponent<Animator>();
       
    }


    private void Update()
    {
        string strDir = dir();
        
        mCurState = GetComponent<PlayerStatus>().state;
        mInputVec = GetComponent<InputScript>().InputVec;

        switch (mCurState)
        {
            case StateType.None:
                break;
            case StateType.Idle:
                {
                  


                }
                break;
            case StateType.Walking:
                {
                    
                  
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


    private string dir()
    {     
        string dir = "";
        switch (GetComponent<InputScript>().direction)
        {
            case DirectionType.Up:
                dir = "Up";
                break;
            case DirectionType.Down:
                dir = "Down";
                break;
            case DirectionType.Left:               
            case DirectionType.Right:
            case DirectionType.DiagonalDownLeft:               
            case DirectionType.DiagonalDownRight:
                {
                    dir = "DiagonalDown";
                }              
                break;
            case DirectionType.DiagonalUpLeft:               
            case DirectionType.DiagonalUpRight:
                {
                    dir = "DiagonalUp";
                }
                break; 
        }

        return dir;
    }
}
