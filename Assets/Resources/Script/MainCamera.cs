using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainCamera : MonoBehaviour
{

    
    private GameObject m_Player = null;
    private Transform m_PlayerTr = null;
    private Vector3 m_Pos = Vector2.zero;


    
    [SerializeField]    
    private string m_TargetName = "";
    private bool mbIsTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.Find(m_TargetName);
        if (m_Player != null)
        {
            m_PlayerTr = m_Player.GetComponent<Transform>();
            mbIsTarget = true;
        }
           
    }

    // Update is called once per frame
    void Update()
    {
       if(null == m_Player)
        {
            TargetDetection();
        }

    }

    private void LateUpdate()
    {
        if (mbIsTarget)
        {
            m_Pos = m_PlayerTr.position;
            m_Pos.z = -10;
            transform.position = m_Pos;           
        }

    }
    public void TargetDetection()
    {
        m_Player = GameObject.Find(m_TargetName);
        if (m_Player != null)
            m_PlayerTr = m_Player.GetComponent<Transform>();
    }

    public void TargetChange(string _targetName)
    {
        m_TargetName = _targetName;
        TargetDetection();
    }


}
