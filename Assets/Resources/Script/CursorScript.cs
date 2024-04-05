using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CursorScript : MonoBehaviour
{

    



    private void Awake()
    {

        Cursor.visible = false;
        
       
    }


    private void Update()
    {
        transform.position = Input.mousePosition;
       


    }
    


   


}
