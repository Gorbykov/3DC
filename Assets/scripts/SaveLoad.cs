using UnityEngine;
using System.Xml.Serialization;
using System;
using System.IO;

public class SaveLoad
{

    static public void Serializator(SceneState state, string datapath)
    {

        Type[] extraTypes = { typeof(ChData) };
        XmlSerializer serializer = new XmlSerializer(typeof(SceneState), extraTypes);
        Debug.Log("Try to open :" + datapath);
        FileStream fs = new FileStream(datapath, FileMode.Create);
        serializer.Serialize(fs, state);
        fs.Close();

    }

    static public SceneState DeXml(string datapath)
    {

        Type[] extraTypes = { typeof(ChData) };
        XmlSerializer serializer = new XmlSerializer(typeof(SceneState), extraTypes);

        FileStream fs = new FileStream(datapath, FileMode.Open);
        SceneState state = (SceneState)serializer.Deserialize(fs);
        fs.Close();

        return state;
    }
}
