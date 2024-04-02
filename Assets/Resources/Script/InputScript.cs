using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



public class InputScript : MonoBehaviour
{
    


    public enum DirectionType
    {        
        Up,
        Down,
        Left,
        Right,
        DiagonalUpLeft,
        DiagonalUpRight,
        DiagonalDownLeft,
        DiagonalDownRight,
    }


    [SerializeField] private Vector2 mInputVec;
    [SerializeField] private float mSpeed = 5.0f; //default speed
    [SerializeField] private float mRollFower = 7.0f; // default fower

    private Vector2[] mDirectionValue;

    private PlayerStatus mPlayerStatus;
    private DirectionType mDirection;
    private float mDiagonal;
    private Rigidbody2D mRigidbody;
    private bool mbMoveStop = false;

    public DirectionType direction
    {
        get { return mDirection; }
        set {  mDirection = value; }
    }

    public Vector2 InputVec
    {
        get { return mInputVec; }
        set { mInputVec = value; }
    }

    private void Awake()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
        mPlayerStatus = GetComponent<PlayerStatus>();
        float sqrt = Mathf.Sqrt(2);
        mDiagonal = 1 / sqrt;

        mDirectionValue = new Vector2[]
        {
        new Vector2(0, 1),                  // 위쪽
        new Vector2(0, -1),                 // 아래쪽
        new Vector2(-1, 0),                 // 왼쪽
        new Vector2(1, 0),                  // 오른쪽
        new Vector2(-1, 1) * mDiagonal,     // 왼쪽 위 대각선
        new Vector2(1, 1)* mDiagonal,       // 오른쪽 위 대각선
        new Vector2(-1, -1)* mDiagonal,     // 왼쪽 아래 대각선
        new Vector2(1, -1)* mDiagonal       // 오른쪽 아래 대각선
        };

    }

    private void Update()
    {
        

        #region PlayerMove
        Vector2 value = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            if (value.x > -0.1f)
                value.x += -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (value.x < 0.1f)
                value.x += +1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (value.y < 0.1f)
                value.y += +1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (value.y > -0.1f)
                value.y += -1;
        }


        if (Input.GetKeyUp(KeyCode.A))
        {
            if (value.x < -0.1f)
                value.x = +1;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            if (value.x > 0.1f)
                value.x = -1;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (value.y > 0.1f)
                value.y = -1;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            if (value.y < -0.1f)
                value.y = +1;
        }

        if(Math.Abs(value.x) + Math.Abs(value.y) == 2)
        {
            value *= mDiagonal;
        }


        mInputVec = value;
        #endregion

        #region PlayerDir

        if (mInputVec.magnitude != 0)
        {
            if (mInputVec.x >= 0)
            {
                if (mInputVec.x == 1)
                    AddDir(DirectionType.Right);
                else if(mInputVec.x == 0)
                {
                    if (mInputVec.y == 1)
                        AddDir(DirectionType.Up);
                    else if (mInputVec.y == -1)
                        AddDir(DirectionType.Down);
                }
                else
                {
                    if (mInputVec.y > 0.7)
                        AddDir(DirectionType.DiagonalUpRight);
                    else if (mInputVec.y < -0.7)
                        AddDir(DirectionType.DiagonalDownRight);
                }
            }
            else if(mInputVec.x < 0)
            {
                if (mInputVec.x == -1)
                    AddDir(DirectionType.Left);
                else
                {
                    if (mInputVec.y > 0.7)
                        AddDir(DirectionType.DiagonalUpLeft);
                    else if (mInputVec.y < -0.7)
                        AddDir(DirectionType.DiagonalDownLeft);
                }
            }              
        }

        #endregion

        

        



        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (mPlayerStatus.state != PlayerStatus.StateType.Roll)
            {
                mPlayerStatus.state = PlayerStatus.StateType.Roll;
                GameObject.Find("Part").GetComponent<Part>().OffEqpmn();
                mbMoveStop = true;

                mRigidbody.AddForce(mDirectionValue[(int)mDirection] * mRollFower, ForceMode2D.Impulse);
            }            
        }



        if (!mbMoveStop)
        {
            if (mInputVec.magnitude > 0)
                mPlayerStatus.state = PlayerStatus.StateType.Walking;
            else
                mPlayerStatus.state = PlayerStatus.StateType.Idle;
        }
        

    }
    private void FixedUpdate()
    {
        if(!mbMoveStop)
        {
            Vector2 pos = mInputVec.normalized * mSpeed * Time.fixedDeltaTime;
            mRigidbody.MovePosition(mRigidbody.position + pos);
        }


    }

    public void IsMove()
    {
        mbMoveStop = false;
    }
    public void MoveStop()
    {
        mbMoveStop = true;
    }

    void AddDir(DirectionType _dir)
    {
        mDirection = _dir;       
    }
}
