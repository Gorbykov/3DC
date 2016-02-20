using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChargeSyns : MonoBehaviour
{

    public GameObject targetChGo;
    public Charge targetCh;
    public InputField chName, q, x, y, z;
    public Text e, f;

    // Use this for initialization
    void Start()
    {
        //targetCh = targetChGo.GetComponent<Charge>();
        needUpdateIn();
    }

    public void ChDestoy()
    {
        Destroy(targetChGo);
        //Destroy(this);
    }

    public void needUpdateIn()
    {
        chName.text = targetCh.name;
        q.text = targetCh.q.ToString("N");
        x.text = targetChGo.transform.position.x.ToString("N");
        y.text = targetChGo.transform.position.y.ToString("N");
        z.text = targetChGo.transform.position.z.ToString("N");
        //Debug.Log(targetCh.f.ToString());
        if (targetCh.q != 0)
        {
            f.text = (targetCh.f.magnitude).ToString("N");
            e.text = (Mathf.Abs(targetCh.f.magnitude / targetCh.q)).ToString("N");
        }
        else
        {
            f.text = "φ=" + targetCh.fi;
            e.text = (targetCh.f.magnitude).ToString("N");
        }
    }

    public void needUpdateOut()
    {
        Vector3 pos = Vector3.zero;
        targetCh.name = chName.text;
        float.TryParse(q.text, out targetCh.q);
        float.TryParse(x.text, out pos.x);
        float.TryParse(y.text, out pos.y);
        float.TryParse(z.text, out pos.z);
        targetChGo.transform.position = pos;
    }

    void Update()
    {
        needUpdateOut();
    }
}
