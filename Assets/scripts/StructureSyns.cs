using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StructureSyns : MonoBehaviour {


    public GameObject targetStructGo;
    public Structure targetStruct;
    public InputField StructName, q, x, y, z, minx, miny, minz, maxx, maxy, maxz, delta, arg;

    // Use this for initialization
    void Start()
    {
        //targetCh = targetChGo.GetComponent<Charge>();
        needUpdateIn();
    }

    public void StructDestoy()
    {
        Destroy(targetStructGo);
        Destroy(this.gameObject);
    }

    public void StructUpdate()
    {

        StartCoroutine(targetStruct.UpdateStruct());
    }

    public void needUpdateIn()
    {
        StructName.text = targetStruct.name;
        q.text = targetStruct.q.ToString("N");
        x.text = targetStructGo.transform.position.x.ToString("N");
        y.text = targetStructGo.transform.position.y.ToString("N");
        z.text = targetStructGo.transform.position.z.ToString("N");
        arg.text = targetStruct.arg;
        minx.text = targetStruct.minx.ToString("N");
        miny.text = targetStruct.miny.ToString("N");
        minz.text = targetStruct.minz.ToString("N");
        maxx.text = targetStruct.maxx.ToString("N");
        maxy.text = targetStruct.maxy.ToString("N");
        maxz.text = targetStruct.maxz.ToString("N");
        delta.text = targetStruct.delta.ToString("N");
        //Debug.Log(targetCh.f.ToString());
    }

    public void needUpdateOut()
    {
        Vector3 pos = Vector3.zero;
        targetStruct.name = StructName.text;
        targetStruct.arg = arg.text;
        float.TryParse(q.text, out targetStruct.q);
        float.TryParse(x.text, out pos.x);
        float.TryParse(y.text, out pos.y);
        float.TryParse(z.text, out pos.z);
        float.TryParse(minx.text, out targetStruct.minx);
        float.TryParse(miny.text, out targetStruct.miny);
        float.TryParse(minz.text, out targetStruct.minz);
        float.TryParse(maxx.text, out targetStruct.maxx);
        float.TryParse(maxy.text, out targetStruct.maxy);
        float.TryParse(maxz.text, out targetStruct.maxz);
        float.TryParse(delta.text, out targetStruct.delta);
        targetStructGo.transform.position = pos;
    }

    void Update()
    {
        needUpdateOut();
    }
}
