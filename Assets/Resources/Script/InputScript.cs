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

    private DirectionType mDirection;
    private float mDiagonal;
    private Rigidbody2D mRigidbody;


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

        float sqrt = Mathf.Sqrt(2);
        mDiagonal = 1 / sqrt;
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


        if (mInputVec.magnitude > 0)
            GetComponent<PlayerStatus>().state = PlayerStatus.StateType.Walking;
        else
            GetComponent<PlayerStatus>().state = PlayerStatus.StateType.Idle;


    }
    private void FixedUpdate()
    {

        Vector2 pos = mInputVec.normalized * mSpeed * Time.fixedDeltaTime;


        mRigidbody.MovePosition(mRigidbody.position + pos);


      

    }


    void AddDir(DirectionType _dir)
    {
        mDirection = _dir;       
    }
}
