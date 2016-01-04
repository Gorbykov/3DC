using UnityEngine;

public class CameraControl : MonoBehaviour {

	public float RotationSpeed = 100f;
	public float MoveSpeed = 100f;
	public float ZoomSpeed = 500f;
	public float MobileZoomSpeed = 1f;
	public float MobileRotationSpeed = 1f;
	public bool firstTouch2=true;
	public bool firstTouch1=true;
	float f=0;
	float distance=0;
	float prevDistance=0;
	Vector2 pos= Vector2.zero;
	Vector2 prevPos=Vector2.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if((Input.GetKey(KeyCode.Mouse0))&&(Input.touchCount==0))
		{
			//Debug.Log("Mouse2 down");
			float hr = RotationSpeed * Time.deltaTime * Input.GetAxis("Mouse X");
			float vr = RotationSpeed * Time.deltaTime * Input.GetAxis("Mouse Y");
			transform.Rotate(vr, -hr, 0);
		}
		if ((Input.touchCount >= 1)&&(firstTouch1))
		{
			pos = Input.GetTouch(0).position;
			prevPos=pos;
			firstTouch1=false;
		}
		if ((Input.touchCount < 1)&&(!firstTouch1))
		{firstTouch1=true;}
		if((Input.GetKey(KeyCode.Mouse0))&&(Input.touchCount==1)&&(!firstTouch1))
		{
			pos = Input.GetTouch(0).position;
			float hr = MobileRotationSpeed * Time.deltaTime * (pos.x - prevPos.x);
			float vr = MobileRotationSpeed * Time.deltaTime * (pos.y - prevPos.y);
			transform.Rotate(vr, -hr, 0);
			Debug.Log(vr.ToString()+hr.ToString());
			prevPos=pos;
		}
		if ((Input.touchCount >= 2)&&(firstTouch2))
		{
			Vector2 touch0, touch1;
			touch0 = Input.GetTouch(0).position;
			touch1 = Input.GetTouch(1).position;
			distance = Vector2.Distance(touch0, touch1);
			Debug.Log("first");
			//f = MobileZoomSpeed * Time.deltaTime * (distance- prevDistance);
			prevDistance=distance;
			firstTouch2=false;
		}
		if ((Input.touchCount < 2)&&(!firstTouch2))
		{firstTouch2=true;}
		if ((Input.touchCount >= 2)&&(!firstTouch2))
		{
			Vector2 touch0, touch1;
			touch0 = Input.GetTouch(0).position;
			touch1 = Input.GetTouch(1).position;
			distance = Vector2.Distance(touch0, touch1);
			//Debug.Log(f.ToString());
			f = MobileZoomSpeed * Time.deltaTime * (distance- prevDistance);
			prevDistance=distance;
		}
		else
		{
			f = ZoomSpeed * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel");
		}
		float h = 0f;
		float v = 0f;
		if (Input.GetKey(KeyCode.D))
		{
			h+=MoveSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.A))
		{
			h-=MoveSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.W))
		{
			v=MoveSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.S))
		{
			v-=MoveSpeed * Time.deltaTime;
		}
		transform.Translate(h,v,f);

	
	}
}
