using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class SceneGen : MonoBehaviour
{

    private SceneState state;   // отражающий класс
    public string datapath = ""; // путь к файлу сохранения для этой локации
    public string fName = "";

    /* void Awake()
     {
         datapath = Application.persistentDataPath + "/Saves/SavedData" + Application.loadedLevel + ".gs";
     }*/

    public void LoadState()
    {
        //datapath = Application.persistentDataPath+ "/Saves/SavedData" + Application.loadedLevel + ".xml";
        if (fName == "")
        {
            fName = PlayerPrefs.GetString("lastFName");
        }
        datapath = Application.dataPath + "/Saves/" + fName + ".csxml";
        if (File.Exists(datapath))  // если файл сохранения уже существует
            state = SaveLoad.DeXml(datapath);  // считываем state оттуда
        else
            setDefault();       // иначе задаём дефолт

        Generate(); // 	генерируем локацию по информации из state

    }

    void setDefault()
    {
        //Application.LoadLevel("mainScene");
        SceneManager.LoadScene(0);
        state = new SceneState();
        //		//chair, table, lamp - нужные префабы из Resourses
        //		state.AddItem(new PositData("chair", new Vector3(15f, 1f, -4f)));
        //		state.AddItem(new PositData("chair", new Vector3(10f, 1f, 0f)));
        //		state.AddItem(new PositData("table", new Vector3(5f, 1f, 4f)));
        //		state.AddItem(new Lamp("lamp", new Vector3(5f, 4f, 4f), true));

    }

    public void Generate()
    {
        foreach (ChData felt in state.charges)
        {  // для всех предметов в комнате
            felt.inst = Instantiate(Resources.Load(felt.Type), felt.Position, Quaternion.identity) as GameObject;
            // овеществляем их
            felt.Estate(); // и задаём дополнительные параметры
        }

    }

    public void Dump()
    {
        state = new SceneState();
        GameObject[] chArr = GameObject.FindGameObjectsWithTag("isCharge");
        foreach (GameObject chGo in chArr)
        {
            Charge ch = chGo.GetComponent("Charge") as Charge;
            if (!ch.isSilent)
            {
                state.AddItem(new ChData("Sphere", ch.name, ch.q, chGo.transform.position));
            }
        }
        GameObject[] strArr = GameObject.FindGameObjectsWithTag("isStruct");
        foreach (GameObject strGo in strArr)
        {
            Structure str = strGo.GetComponent("Structure") as Structure;
            state.AddItem(new StrData("StructureObj", str.name, str.q, strGo.transform.position, str.arg, str.minx, str.miny, str.minz, str.maxx, str.maxy, str.maxz, str.delta));
        }
        if (datapath == "")
        {
            fName = DateTime.Now.ToString("MMddyyHHmmss");
            datapath = Application.dataPath + "/Saves/SavedData" + fName + ".csxml";
        }
        PlayerPrefs.SetString("lastFName", fName);
        //Debug.Log(PlayerPrefs.GetString("lastDataPath") + "--"+ datapath);
        SaveLoad.Serializator(state, datapath); // и его сериализация
    }
}
