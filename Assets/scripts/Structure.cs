using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class Structure : MonoBehaviour
{

    public float q = 1;
    public int type = 1;
    public string arg = "x/2+3";
    public StringCollection arr = new StringCollection();
    public Stack<string> poland = new Stack<string>();
    string err = "";
    public bool needUpdate = true;

    void strToArr()
    {
        bool waitChar = false;
        arg.ToLower();
        //int j = 0;
        for (int i = 0; i < arg.Length; i++)
        {
            if (((arg[i] >= 'a') && (arg[i] <= 'z')) || ((arg[i] >= '0') && (arg[i] <= '9')) || arg[i] == '.')
            {
                if (waitChar)
                    arr[arr.Count - 1] += arg[i];
                else
                {
                    arr.Add(arg[i] + "");
                    waitChar = true;
                }
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
        operations.Push("-1");
        int state = 2;
        int i = 0;
        int buf;
        while (state == 2)
        {
            string peek = operations.Peek();
            if (int.TryParse(arr[i], out buf))
            {
                poland.Push(arr[i]);
                i++;
            }
            else
            {
                if ((arr[i] == "+") || (arr[i] == "-"))
                {
                    if ((peek == "-1") || (peek == "("))
                    {
                        operations.Push(arr[i]);
                        i++;
                    }
                    else
                    if ((peek == "+") || (peek == "-") || (peek == "*") || (peek == "/"))
                    {
                        poland.Push(operations.Pop());
                    }
                }
                else
                    if ((arr[i] == "*") || (arr[i] == "/"))
                {
                    if ((peek == "-1") || (peek == "(") || (peek == "+") || (peek == "-"))
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
                        if ((peek == "+") || (peek == "-") || (peek == "*") || (peek == "/"))
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
                        if ((peek == "+") || (peek == "-") || (peek == "*") || (peek == "/"))
                        poland.Push(operations.Pop());
                    else
                        if (peek == "(")
                    {
                        state = 0;
                        err += "wrong() ";
                    }
                    else
                    {
                        state = 0;
                        err += "unknown symbol";
                    }
                }
            }
        }
    }
    // Use this for initialization
    void Start()
    {
        tag = "isStruct";
    }

    public void UpdateStruct()
    {
        strToArr();
        arrToPoland();
        /*while (poland.Count!=0)//Дебаг польского стека и массива
        {
            Debug.Log(poland.Pop().ToString());
        }
        Debug.Log("arr");
        foreach (string el in arr)
        {
            Debug.Log(el.ToString());
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (needUpdate)
        {
            UpdateStruct();
            needUpdate = false;
        }

    }
}
