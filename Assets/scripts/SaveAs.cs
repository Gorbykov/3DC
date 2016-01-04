using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SaveAs : MonoBehaviour
{

    public InputField inField;
    private string datapath = "";
    private string fName = "";
    private SceneGen scenegen;
    public GameObject SceneGenObj;
    public bool saveButton = false;

    void Start()
    {
        if (!Directory.Exists(Application.dataPath + "/Saves/"))
        {
            Debug.Log("no save dir");
            Directory.CreateDirectory(Application.dataPath + "/Saves/");
        }
        scenegen = SceneGenObj.GetComponent<SceneGen>();
        fName = PlayerPrefs.GetString("lastFName");
        //Debug.Log(datapath);
        inField.text = fName;
    }

    public void switchButtonFalg()
    {
        saveButton = !saveButton;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return)) || saveButton)
        {
            datapath = Application.dataPath + "/Saves/" + inField.text + ".csxml";
            scenegen.datapath = datapath;
            scenegen.Dump();
            saveButton = false;
        }
    }
}
