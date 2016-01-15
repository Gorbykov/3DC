using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class Structure : MonoBehaviour
{

    public float q = 1;
    public int type = 1;
    public string arg = "x";
    public StringCollection arr = new StringCollection();
    public string[] polandArr;
    public string error = "";
    public bool needUpdate = true;
    public List<GameObject> charges;
    public float minx, miny, minz, maxx, maxy, maxz, delta;
    public GameObject prefCharge;
    public UpdateStream upStr;
    ConsoleInput console;
    GameObject panel;
    public GameObject Content;
    public GameObject prefPanel;

    void Start()
    {
        tag = "isStruct";
        upStr = GameObject.Find("UpdateStreamObj").GetComponent<UpdateStream>();
        console = GameObject.Find("ConsoleControl").GetComponent<ConsoleInput>();
        panel = Instantiate(prefPanel);
        Content = GameObject.Find("Content");
        panel.transform.SetParent(Content.transform);
        panel.transform.SetAsLastSibling();
        StructureSyns StructS = panel.GetComponent<StructureSyns>();
        StructS.targetStructGo = gameObject;
        StructS.targetStruct = this;
    }

    void strToArr()
    {
        bool waitChar = false;
        arg.ToLower();
        arr = new StringCollection();
        //int j = 0;
        for (int i = 0; i < arg.Length; i++)
        {
            if (((arg[i] >= 'a') && (arg[i] <= 'z')) || ((arg[i] >= '0') && (arg[i] <= '9')) || arg[i] == '.')
            {
                if (waitChar && (arr[arr.Count - 1] != "-"))
                    arr[arr.Count - 1] += arg[i];
                else
                {
                    arr.Add(arg[i] + "");
                    waitChar = true;
                }
            }
            else
                if ((arg[i] == '-') && ((i == 0) || ((arg[i - 1] < '0') || (arg[i - 1] > '9'))))
            {
                arr.Add("unmin");
                //waitChar = true;
            }
            else
            {
                arr.Add(arg[i] + "");
                waitChar = false;
            }
        }
        arr.Add("$");
    }

    void arrToPoland()
    {
        Stack<string> operations = new Stack<string>();
        Stack<string> poland = new Stack<string>();
        operations.Push("-1");
        int state = 2;
        int i = 0;
        float buf;
        while (state == 2)
        {
            string peek = operations.Peek();
            if (float.TryParse(arr[i], out buf) || (arr[i] == "x") || (arr[i] == "y"))
            {
                poland.Push(arr[i]);
                i++;
            }
            else
            {
                if ((arr[i] == "+") || (arr[i] == "-") || (arr[i].Length >= 2))
                {
                    if ((peek == "-1") || (peek == "("))
                    {
                        operations.Push(arr[i]);
                        i++;
                    }
                    else
                    if ((peek == "+") || (peek == "-") || (peek == "*") || (peek == "/") || (peek == "^") || (peek.Length >= 2))
                    {
                        poland.Push(operations.Pop());
                    }
                }
                else
                    if ((arr[i] == "*") || (arr[i] == "/") || (arr[i] == "^"))
                {
                    if ((peek == "-1") || (peek == "(") || (peek == "+") || (peek == "-") || (peek.Length >= 2))
                    {
                        operations.Push(arr[i]);
                        i++;
                    }
                    else
                    {
                        poland.Push(operations.Pop());
                    }
                }
                else
                    if (arr[i] == "(")
                {
                    operations.Push(arr[i]);
                    i++;
                }
                else
                    if (arr[i] == ")")
                {
                    if (peek == "-1")
                        state = 0;
                    else
                        if ((peek == "+") || (peek == "-") || (peek == "*") || (peek == "/") || (peek == "^") || (peek.Length >= 2))
                    {
                        poland.Push(operations.Pop());
                    }
                    else
                        if (peek == "(")
                    {
                        operations.Pop();
                        i++;
                    }
                }
                else
                    if (arr[i] == "$")
                {
                    if (peek == "-1")
                        state = 1;
                    else
                        if ((peek == "+") || (peek == "-") || (peek == "*") || (peek == "/") || (peek == "^") || (peek.Length >= 2))
                        poland.Push(operations.Pop());
                    else
                        if (peek == "(")
                    {
                        state = 0;
                        error += "wrong(), ";
                    }
                    else
                    {
                        state = 0;
                        error += "unknown symbol,";
                    }
                }
            }
        }
        polandArr = poland.ToArray();
    }

    float Calc(float x, float y)
    {
        Stack<string> p = new Stack<string>();
        float a, b, c;
        for (int i = polandArr.Length - 1; i >= 0; i--)
        {
            if (float.TryParse(polandArr[i], out a))
            {
                p.Push(polandArr[i]);
            }
            else
                if (polandArr[i] == "x")
            {
                p.Push(x.ToString());
            }
            else
                if (polandArr[i] == "y")
            {
                p.Push(y.ToString());
            }
            else
                if (polandArr[i].Length == 1)
            {
                b = float.Parse(p.Pop());
                a = float.Parse(p.Pop());
                c = 0;
                switch (polandArr[i])
                {
                    case "+":
                        c = a + b;
                        break;
                    case "-":
                        c = a - b;
                        break;
                    case "*":
                        c = a * b;
                        break;
                    case "/":
                        c = a / b;
                        break;
                    case "^":
                        c = Mathf.Pow(a, b);
                        break;
                    default:
                        error += "unknown binary operation, ";
                        break;
                }
                p.Push(c.ToString());
            }
            else
                if (polandArr[i].Length > 1)
            {
                a = float.Parse(p.Pop());
                c = 0;
                switch (polandArr[i])
                {
                    case "sqrt":
                        c = Mathf.Sqrt(a);
                        break;
                    case "sin":
                        c = Mathf.Sin(a);
                        break;
                    case "cos":
                        c = Mathf.Cos(a);
                        break;
                    case "tg":
                        c = Mathf.Tan(a);
                        break;
                    case "ctg":
                        c = 1 / Mathf.Tan(a);
                        break;
                    case "abs":
                        c = Mathf.Abs(a);
                        break;
                    case "unmin":
                        c = -a;
                        break;
                    default:
                        error += "unknown unary operation(func), ";
                        break;
                }
                p.Push(c.ToString());
            }
            else
                error += "uncknown symbol, ";
        }
        if (float.TryParse(p.Pop(), out c))
        {
            return c;
        }
        else
        {
            error += "calc err, ";
            return -1;
        }
    }

    public IEnumerator UpdateStruct()
    {
        foreach (GameObject el in charges)
        {
            Destroy(el);
        }
        charges = new List<GameObject>();
        strToArr();
        /*if (error != "")
        {
            console.sendOut(error);
            yield break;
        }
        yield return null;*/
        arrToPoland();
        /*if (error != "")
        {
            console.sendOut(error);
            yield break;
        }*/
        string st = "";
        foreach (string el in polandArr)
        {
            st += el + " ";
        }
        Debug.Log("poland: " + st);
        st = "";
        foreach (string el in arr)
        {
            st += el + " ";
        }
        Debug.Log("arr: " + st + "=" + Calc(2, 3).ToString());
        float z;
        for (float x = minx; x < maxx; x += delta)
        {
            for (float y = miny; y < maxx; y += delta)
            {
                z = Calc(x, y);
                if (!float.IsInfinity(z) && !float.IsNaN(z) && ((z <= maxz) && (z >= minz)))
                {
                    charges.Add(Instantiate(prefCharge, new Vector3(x, y, z)+transform.position, transform.rotation) as GameObject);
                    charges[charges.Count - 1].name = "";
                    Charge charge = charges[charges.Count - 1].GetComponent<Charge>();
                    charge.isSilent = true;
                    charges[charges.Count - 1].transform.SetParent(transform);
                    //yield return null;
                }
            }
        }
        //yield return null;
        foreach (GameObject el in charges)
        {
            if (el != null)
            {
                Charge ch = el.GetComponent<Charge>();
                ch.q = q / charges.Count;
            }
            //yield return null;
        }
        yield return null;
        upStr.UpdateCharges();
        yield return null;
        StructureSyns StrS = panel.GetComponent<StructureSyns>();
        StrS.needUpdateIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (needUpdate)
        {
            needUpdate = false;
            StartCoroutine(UpdateStruct());
        }
        
    }
    
    void OnDestoy()
    {
        Debug.Log("desstroy structure panel");
        Destroy(panel);
    }
}
