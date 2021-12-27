using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeMoviment : MonoBehaviour
{
    
    public Transform button;
    public Transform posTracker;
   

    Vector3 mousePos;
    float angle;
 
    
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
        if (Input.GetMouseButton(0))
        {
         
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 dir = mousePos - button.position;
             angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle -90));

            dir = dir.normalized;
            dir *= 0.5f;

           
            posTracker.position = button.position + new Vector3(dir.x, dir.y, 0);
        }
       
    }
}
