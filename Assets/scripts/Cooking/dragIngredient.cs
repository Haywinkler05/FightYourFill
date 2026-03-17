using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ref:https://www.youtube.com/watch?v=0yHBDZHLRbQ

public class DragIngredient : MonoBehaviour

{
    private Vector3 mOffset;
    private float mZCoord;
    public float yPos;

    public LayerMask detectionLayer;
    public float rayLength = 20f;

    private GameObject indicator;
    

    void Start() {
        detectionLayer |= (1 << 8);    
    }
 
    

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        // z coordinate of game object on screen
        mousePoint.z = mZCoord;
        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDown()
    {
        Cursor.visible = false;
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

        //drop indicator
        // indicator = Instantiate(gameObject, transform.position, Quaternion.identity);
        // indicator.GetComponent<Rigidbody>().useGravity = false;
        // indicator.GetComponent<Collider>().enabled = false;
        // indicator.GetComponent<Renderer>().material = Resources.Load<Material>("Assets/Materials/Consumables/TransparentGreen");
        // indicator.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("Assets/Materials/Consumables/TransparentGreen");
       
       
        indicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        indicator.GetComponent<Collider>().enabled = false;
        indicator.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        indicator.GetComponent<Renderer>().material.color = Color.green;
        
    }

    private void OnMouseDrag()
    {
        Vector3 posObj = GetMouseAsWorldPoint() + mOffset;
        posObj.y = yPos;
        transform.position = posObj;
        transform.rotation = Quaternion.identity;
        //Debug.Log(posObj + "posobj");


        RaycastHit hit;
        //raycast
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, rayLength, detectionLayer))
        {
            //Debug.Log("raycast detect" + hit.point);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 10, Color.white);

            indicator.transform.position = new Vector3(hit.point.x,hit.point.y+.01f,hit.point.z);
        }
        else{Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 10, Color.red);}

    }
    private void OnMouseUp() {
        Destroy(indicator);
        Cursor.visible = true;
    }
}