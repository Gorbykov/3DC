using UnityEngine;

public class SaveRotate : MonoBehaviour {
	public GameObject go;
	// Update is called once per frame
	void Update () {
		go.transform.eulerAngles=Vector3.zero;
	}
}
