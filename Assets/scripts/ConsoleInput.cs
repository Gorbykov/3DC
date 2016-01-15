using UnityEngine;
using UnityEngine.UI;


public class ConsoleInput : MonoBehaviour
{

    public CameraControl Cam;
    public InputField inField;
    public Text[] outFields;
    public GameObject newCharge;
    public GameObject prefCharge;
    public GameObject newStructure;
    public GameObject prefStructure;
    public bool okButton = false;
    public string[] commandList;
    public string[] errorList;
    string[] argv = new string[100];
    int argc = 0;
    string inString;

    public void okPress()
    { okButton = true; }

    string GetWord(ref string inStr)
    {
        string word = "";
        int i = 0;
        while (inStr[i] != ' ')
        {
            word += inStr[i].ToString();
            i++;
        }
        inStr = inStr.Remove(0, i + 1);
        //Debug.Log("inStr after :"+inStr);
        return word;
    }

    void Start()
    {
        Color alphaChanger;
        alphaChanger.r = 0f;
        alphaChanger.g = 0f;
        alphaChanger.b = 0f;
        alphaChanger.a = 1f;
        float deltaAlpha = 1f / (outFields.Length + 1);
        for (int i = 0; i < outFields.Length; i++)
        {
            alphaChanger.a = 1f - deltaAlpha * (i + 1);
            //Debug.Log(alphaChanger.a.ToString());
            outFields[i].color = alphaChanger;
        }
    }

    public void sendOut(string outStr)
    {
        for (int i = outFields.Length - 2; i >= 0; i--)
        {
            outFields[i + 1].text = outFields[i].text;
        }
        outFields[0].text = outStr;
    }

    bool findStr(string[] arr, string fel)
    {
        foreach (string el in arr)
        {
            if (el == fel)
                return true;
        }
        return false;
    }

    /// Commands functions add here
    void add()
    {
        if (argc != 5)
        {
            sendOut(errorList[1] + '"' + argv[0] + '"');
            return;
        }
        bool error = true;
        float x, y, z, q;
        string name;
        name = argv[1];
        error = float.TryParse(argv[2], out q);
        error = float.TryParse(argv[3], out x);
        error = float.TryParse(argv[4], out y);
        error = float.TryParse(argv[5], out z);
        if (!error)
        {
            sendOut(errorList[2] + '"' + argv[0] + '"');
            return;
        }
        //Debug.Log(name+q.ToString()+x.ToString()+y.ToString()+z.ToString());
        newCharge = Instantiate(prefCharge, new Vector3(x, y, z), transform.rotation) as GameObject;
        Charge charge = newCharge.GetComponent<Charge>();
        charge.q = q;
        charge.name = name;
        charge.isSilent = false;
        //charge.Start();
        sendOut('/' + argv[0] + ' ' + name + ' ' + q.ToString() + ' ' + x.ToString() + ' ' + y.ToString() + ' ' + z.ToString());
    }

    void addStruct()
    {
        if (argc != 13)
        {
            sendOut(errorList[1] + '"' + argv[0] + '"');
            return;
        }
        bool error = true;
        float x, y, z, q, minx, miny, minz, maxx, maxy, maxz, delta;
        string name, arg;
        name = argv[1];
        arg = argv[2];
        error = float.TryParse(argv[3], out q);
        error = float.TryParse(argv[4], out x);
        error = float.TryParse(argv[5], out y);
        error = float.TryParse(argv[6], out z);
        error = float.TryParse(argv[7], out minx);
        error = float.TryParse(argv[8], out miny);
        error = float.TryParse(argv[9], out minz);
        error = float.TryParse(argv[10], out maxx);
        error = float.TryParse(argv[11], out maxy);
        error = float.TryParse(argv[12], out maxz);
        error = float.TryParse(argv[13], out delta);
        if (!error)
        {
            sendOut(errorList[2] + '"' + argv[0] + '"');
            return;
        }
        //Debug.Log(name+q.ToString()+x.ToString()+y.ToString()+z.ToString());
        newStructure = Instantiate(prefStructure, new Vector3(x, y, z), transform.rotation) as GameObject;
        Structure str = newStructure.GetComponent<Structure>();
        str.q = q;
        str.name = name;
        str.minx = minx;
        str.miny = miny;
        str.minz = minz;
        str.delta = delta;
        str.maxx = maxx;
        str.maxy = maxy;
        str.maxz = maxz;
        str.arg = arg;
        sendOut('/' + argv[0] + ' ' + name + ' ' + q.ToString() + ' ' + x.ToString() + ' ' + y.ToString() + ' ' + z.ToString());
    }

    void remove()
    {
        if (argc != 1)
        {
            sendOut(errorList[1] + '"' + argv[0] + '"');
            return;
        }
        name = argv[1];
        GameObject delCharge = GameObject.Find(name);
        Debug.Log(delCharge.ToString());
        bool flag = false;
        flag = delCharge.tag == "isCharge";
        flag = flag ^ (delCharge.tag == "isStruct");
        if ((delCharge == null) && flag)
        {
            sendOut(errorList[3]);
            return;
        }
        //Destroy(delCharge.GetComponent<Charge>().title);
        Destroy(delCharge);
        sendOut('/' + argv[0] + ' ' + name);
    }

    void getf()
    {
        if (argc != 1)
        {
            sendOut(errorList[1] + '"' + argv[0] + '"');
            return;
        }
        name = argv[1];
        GameObject fCharge = GameObject.Find(name);
        if (fCharge == null)
        {
            sendOut(errorList[3]);
            return;
        }
        sendOut(name + " F = " + fCharge.GetComponent<Charge>().f.ToString("G4"));
    }

    void getw()
    {
        if (argc != 0)
        {
            sendOut(errorList[1] + '"' + argv[0] + '"');
            return;
        }
        float w = 0;
        GameObject[] charges = GameObject.FindGameObjectsWithTag("isCharge");
        foreach (GameObject charge in charges)
        {
            Charge chs = charge.GetComponent<Charge>();
            w += chs.w;
        }
        w = w / 2;
        sendOut("W= " + w.ToString("G4"));
    }
    /// 

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return)) || okButton)
        {
            //Start ();
            inString = inField.text;//+' #';
            inField.text = "";
            //Debug.Log(inString);
            if (inString != "")
                if (inString[0] == '/')
                {
                    inString += " endl ";
                    argv = new string[100];
                    argc = 0;
                    //string command;
                    argv[0] = GetWord(ref inString);
                    argv[0] = argv[0].Remove(0, 1);
                    while (argv[argc] != "endl")
                    {
                        argc++;
                        argv[argc] = GetWord(ref inString);
                    }
                    argc--;
                    if (findStr(commandList, argv[0]))
                    {
                        Invoke(argv[0], 0f);
                    }
                    else sendOut(errorList[0]);
                    //Debug.Log("argv= "+argv[0].ToString());
                    ///commands
                    ///
                }
                else
                    sendOut(inString);
            /*//OLDVERSION
			inString=inField.text+" ";
			string command;
			command=GetWord(ref inString);
			//Debug.Log(command);
			//add
			//bool errors;
			if(command=="/add"){
				float x,y,z,q;
				string name;
				name=GetWord(ref inString);
				float.TryParse(GetWord(ref inString),out q);
				float.TryParse(GetWord(ref inString),out x);
				float.TryParse(GetWord(ref inString),out y);
				float.TryParse(GetWord(ref inString),out z);
				Debug.Log(name+q.ToString()+x.ToString()+y.ToString()+z.ToString());
				newCharge=Instantiate(prefCharge, new Vector3(x,y,z),transform.rotation) as GameObject;
				Charge charge=newCharge.GetComponent<Charge>();
				charge.q=q;
				charge.name=name;

			}
			if(command=="/remove")
			{
				string name;
				name=GetWord(ref inString);
				GameObject delCharge=GameObject.Find(name);
				Destroy(delCharge.GetComponent<Charge>().title);
				Destroy(delCharge);
			}
			if(command=="/getf")
			{
				string name;
				name=GetWord(ref inString);
				GameObject fCharge=GameObject.Find(name);
				outField.text=fCharge.GetComponent<Charge>().f.ToString("G4");
			}
			if(command=="/getw")
			{
				float w=0;
				GameObject[] charges=GameObject.FindGameObjectsWithTag("isCharge");
				foreach(GameObject charge in charges)
				{
					Charge chs=charge.GetComponent<Charge>();
					w+=chs.w;
				}
				w=w/2;
				outField.text=w.ToString("G4");
			}
			//*/
            okButton = false;
            //Cam.enabled = true;
        }
    }
}
