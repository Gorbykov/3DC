using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    public SceneGen sgobj;
    public Dropdown fList;
    public bool needUpdate = true;

    string getFName(string fullName)
    {
        string fName;
        int spos = fullName.LastIndexOf("/");
        fName = fullName.Substring(spos+1,(fullName.Length-spos-7));
        return fName;
    }

    // Use this for initialization
    void Start()
    {
        if (!Directory.Exists(Application.dataPath + "/Saves/"))
        {
            Debug.Log("no save dir");
            Directory.CreateDirectory(Application.dataPath + "/Saves/");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (needUpdate)
        {
            fList.options.Clear();
            foreach (string file in Directory.GetFiles(Application.dataPath + "/Saves/","*.csxml"))
            {
                fList.options.Add(new Dropdown.OptionData(getFName(file)));
            }
            needUpdate = false;
        }
    }

    public void LoadFrom()
    {
        sgobj.fName = fList.options[fList.value].text;
        sgobj.LoadState();
        PlayerPrefs.SetString("lastFName", fList.options[fList.value].text);
    }

    public void SwitchUpdateFlag()
    {
        needUpdate = !needUpdate;
    }
}
