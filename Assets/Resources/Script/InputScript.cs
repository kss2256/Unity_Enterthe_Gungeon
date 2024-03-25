using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



public class InputScript : MonoBehaviour
{
    [SerializeField] private Vector2 mInputVec;
    [SerializeField] private float mSpeed = 5.0f; //default speed


    private float mDiagonal;
    private SpriteRenderer mSpriteRenderer;
    private Rigidbody2D mRigidbody;

    private void Awake()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
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
            value.x = +1;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            value.x = -1;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            value.y = -1;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            value.y = +1;
        }

        if(Math.Abs(value.x) + Math.Abs(value.y) == 2)
        {
            value *= mDiagonal;
        }


        mInputVec = value;
        #endregion






    }
    private void FixedUpdate()
    {
        Vector2 pos = mInputVec.normalized * mSpeed * Time.fixedDeltaTime;


        mRigidbody.MovePosition(mRigidbody.position + pos);

    }


}
