using UnityEngine;
using System.Collections;

public class MoveAxis : MonoBehaviour {

    public CameraControl camCtrl;
    public bool X, Y, Z;
    float fX, fY, fZ;
    Vector3 screenPoint, offset;
    //public Transform XYZ;
    //XYZMove XYZscript;
    Vector3 curPos;
    Vector3 oldPos;
    Vector3 curScreenPoint;
    Transform Ch;

    void Start()
    {
        fX = fY = fZ = 0F;
        //XYZ = transform.GetComponentInParent<Transform>();
        if (X) fX = 1;
        if (Y) fY = 1;
        if (Z) fZ = 1;
        //XYZscript = XYZ.GetComponent<XYZMove>();
    }
    
    void OnMouseDown()
    {
        Debug.Log("Axis click");
        Ch = transform.parent.parent;
        screenPoint = Camera.main.WorldToScreenPoint(Ch.position);
        offset = -Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        //offset = Vector3.zero;
        camCtrl.enabled = false;
        oldPos = transform.parent.parent.position;
    }

    void OnMouseUp()
    {
        //Debug.Log("");
        camCtrl.enabled = true;
    }

    void OnMouseDrag()
    {
        curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        curPos = new Vector3((Camera.main.ScreenToWorldPoint(curScreenPoint).x+offset.x)*fX, (Camera.main.ScreenToWorldPoint(curScreenPoint).y + offset.y) * fY, (Camera.main.ScreenToWorldPoint(curScreenPoint).z + offset.z) * fZ);
        Debug.Log(curPos.ToString());
        Ch.position = curPos + oldPos;
    }
}
