using UnityEngine;
using System.Collections;

public class XYZMove : MonoBehaviour {

    public Transform Ch;
    //public Transform X, Y, Z;
    public Vector3 moveTo;

    void OnTransformParentChanged()
    {
        Ch = transform.parent;
        //transform.position = Vector3.zero;
    }

    void Update () {
        //Debug.Log(X.position.x.ToString() +" "+ Y.position.y.ToString() +" "+ Z.position.z.ToString());
        //transform.position = new Vector3(X.position.x, Y.position.y, Z.position.z);
        Ch.position += moveTo;
	}
}
