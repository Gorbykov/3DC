using UnityEngine;

public class SaveLoadInterface : MonoBehaviour
{

    //string datapath =Application.dataPath + "/Saves/SavedData" + Application.loadedLevel + ".xml";
    public SceneGen gen;

    public void InterfaceLoad()
    {
        gen.LoadState();
    }
}
