using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputScript : MonoBehaviour
{
    [SerializeField] private Vector2 mInputVec;

    public Vector2 test;

    private SpriteRenderer mSpriteRenderer;
    private Rigidbody2D mRigidbody;

    private void Awake()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mRigidbody = GetComponent<Rigidbody2D>();


    }

    private void Update()
    {
        #region 이동 로직
        
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
        //mInputVec = value;
        #endregion

    }

    void OnMove(InputValue _value)
    {
        mInputVec = _value.Get<Vector2>();
    }


    private void FixedUpdate()
    {
        Vector2 pos = mInputVec.normalized * 5.0f * Time.fixedDeltaTime;


        mRigidbody.MovePosition(mRigidbody.position + pos);



    }


}
