using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("SceneState")]
[XmlInclude(typeof(ChData))]
public class SceneState
{

    [XmlArray("Charges")]
    [XmlArrayItem("Charge")]
    public List<ChData> charges = new List<ChData>();

    public SceneState() { }
    public void AddItem(ChData item)
    {   // добавление элементов - будем этим пользоваться
        charges.Add(item);          // при генерации дефолтной версии локации
    }

    public void Update()
    {    // функция, по которой данные этого класса-дубликата объектов 
        foreach (ChData felt in charges) // будут обновляться
            felt.Update();
    }

}
