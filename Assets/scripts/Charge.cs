using UnityEngine;
using UnityEngine.UI;

public class Charge : MonoBehaviour
{
    public GameObject Content;
    public GameObject prefPanel;
    public GameObject canvas;
    public GameObject title;
    public float k = 9;
    public float scale = 10;
    public float q = 1.0f;
    public Texture plus;
    public Texture minus;
    public Vector3 f = new Vector3(0, 0, 0);
    public float fi = 0;
    public float w = 0;
    public bool phisOn = false;
    public Rigidbody rb;
    public bool needUpdate = false;
    public UpdateStream upStr;
    public GameObject XYZ;
    public bool isSilent = false;
    Text titleText;
    GameObject[] charges;
    GameObject target;
    Transform arrow;
    bool isTitleCreate = false;
    float oldQ;
    Vector3 oldPos;
    GameObject panel;
    //public GUIStyle style= new GUIStyle();
    //Transform target = new Transform();
    // Use this for initialization
    void Start()
    {
        gameObject.tag = "isCharge";
        canvas = GameObject.Find("Canvas");
        upStr = GameObject.Find("UpdateStreamObj").GetComponent<UpdateStream>();
        if (!isSilent)
        {
            upStr.UpdateCharges();
            Debug.Log("try to update from " + name);
            oldQ = q;
            oldPos = transform.position;
            panel = Instantiate(prefPanel);
            Content = GameObject.Find("Content");
            panel.transform.SetParent(Content.transform);
            panel.transform.SetAsLastSibling();
            ChargeSyns ChS = panel.GetComponent<ChargeSyns>();
            ChS.targetChGo = gameObject;
            ChS.targetCh = this;
        }
        XYZ = upStr.XYZ;
        //ChS.needUpdateIn();
    }

    void UpdateCharge()
    {
        //Наложение текстур
        /*if (isUpdated)
        {
            return;
        }*/
        //isUpdated = true;
        Debug.Log(name + " updated");
        if (q > 0)
            GetComponent<Renderer>().material.SetTexture("_MainTex", plus);
        else
            GetComponent<Renderer>().material.SetTexture("_MainTex", minus);
        //transform.localScale = new Vector3(Mathf.Abs(q), Mathf.Abs(q), Mathf.Abs(q));//Масштаб по величене заряда
        //
        //Просчет силы кулона и всего остального
        f = Vector3.zero;
        fi = 0f;
        charges = GameObject.FindGameObjectsWithTag("isCharge");//оптимизировать!!//вынесено в отдельную функцию
        foreach (GameObject charge in charges)
        {
            if (transform.position != charge.transform.position)
            {
                Charge chs = charge.GetComponent<Charge>();
                //chs.UpdateCharge();
                fi += (k * chs.q) / (transform.position - charge.transform.position).magnitude;
                if (q != 0)
                {
                    f += k * (transform.position - charge.transform.position).normalized * ((q * chs.q) / ((transform.position - charge.transform.position).sqrMagnitude));
                }
                else
                {
                    f += k * (transform.position - charge.transform.position).normalized * (chs.q / ((transform.position - charge.transform.position).sqrMagnitude));
                }
            }
        }
        w = fi * q;
        //
        //Масштаб стрелки
        arrow = transform.GetChild(0);
        if (q != 0)
            arrow.localScale = new Vector3(0.5f, Mathf.Abs(f.magnitude * scale), 0.5f);
        //
        //Разворот стрелки (костыль через LookAt)
        //Debug.Log(arrow.localScale.ToString());
        target = new GameObject("target " + name);
        target.transform.position = f + transform.position;
        arrow.LookAt(target.transform);
        arrow.Rotate(90, 0, 0);
        Destroy(target);
        //Debug.Log(arrow.transform.rotation);
        //Debug.DrawLine(transform.position,transform.position+f*scale, Color.green);
        //needUpdate = false;
        if (!isSilent)
        {
            ChargeSyns ChS = panel.GetComponent<ChargeSyns>();
            ChS.needUpdateIn();
        }
    }

    void Update()
    {
        if ((!oldPos.Equals(transform.position)) || (!oldQ.Equals(q)))
        {
            oldPos = transform.position;
            oldQ = q;
            if (!isSilent)
            {
                upStr.UpdateCharges();
            }
        }
        if (needUpdate)
        {
            UpdateCharge();
            needUpdate = false;
        }
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Vector3 cameraRelative = Camera.main.transform.InverseTransformPoint(transform.position);
        if (!isTitleCreate)
        {
            title = Instantiate(title, new Vector3(screenPosition.x, Screen.height - screenPosition.y, 0), transform.rotation) as GameObject;
            title.transform.SetParent(canvas.transform);
            titleText = title.GetComponent<Text>();
            //titleText.text = (name+"\n"+(f.magnitude).ToString()+" ГH");
            isTitleCreate = true;
            title.transform.SetAsFirstSibling();
            //Rect position = new Rect(screenPosition.x, Screen.height - screenPosition.y, 200f, 50f);
            //GUI.Label(position, name+"\n"+(f.magnitude).ToString()+" ГH", style);
        }
        else
        {
            if (cameraRelative.z > 0)
            {
                title.SetActive(true);
                title.transform.position = new Vector3(screenPosition.x,/*Screen.height -*/ screenPosition.y, 0);
                //Debug.Log(cameraRelative.ToString());
                if (q != 0)
                {
                    titleText.text = (name/* + "=" + q + "\n" + "F=" + (f.magnitude).ToString() + " ГH" + "\n" + "E=" + (f.magnitude / q).ToString() + "ГВ/м"*/);
                }
                else
                {
                    titleText.text = (name + "\n" + "E=" + (f.magnitude).ToString() + " ГДж" + "\n" + "fi=" + fi);
                }
            }
            else
            {
                title.SetActive(false);
            }
        }
        //Зачаток физики
        if (phisOn)
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
                rb.mass = q;
            }
            rb.velocity += f;
        }
        //
    }
    //куски мертвого кода
    /*void OnGUI()
	{
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		Vector3 cameraRelative = Camera.main.transform.InverseTransformPoint(transform.position);
		//Debug.Log(screenPosition.ToString()+"   "+cameraRelative.ToString());
		if (cameraRelative.z > 0)
		{
			Rect position = new Rect(screenPosition.x, Screen.height - screenPosition.y, 200f, 50f);
			GUI.Label(position, name+"\n"+(f.magnitude).ToString()+" ГH", style);
		}
	}*/
    void OnDestroy()
    {
        XYZ.transform.SetParent(upStr.gameObject.transform.parent);
        XYZ.SetActive(false);
        //upStr = GameObject.Find("UpdateStreamObj").GetComponent<UpdateStream>();
        upStr.UpdateCharges();
        Destroy(panel);
        Destroy(title);
    }

    void OnMouseDown()
    {
        Debug.Log("Click on " + name);
        XYZ.transform.position = Vector3.zero;
        XYZ.transform.SetParent(transform, true);
        XYZ.transform.position = transform.position;
        //XYZ.SetActive(false);
        XYZ.SetActive(true);
    }
}
