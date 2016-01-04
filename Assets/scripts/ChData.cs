using UnityEngine;
using System.Xml.Serialization;

[XmlType("ChargesData")]
public class ChData
{
    protected GameObject _inst; // тут храним ссылку на отражаемый объект
    public GameObject inst { set { _inst = value; } }

    [XmlElement("Type")]
    public string Type { get; set; }  // это будет название префаба из Resourses

    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("q")]
    public float Q { get; set; }

    [XmlElement("Position")]
    public Vector3 Position { get; set; }

    public ChData() { }

    public ChData(string type, string name, float q, Vector3 position)
    {
        this.Type = type;
        this.Position = position;
        this.Name = name;
        this.Q = q;
    }

    public virtual void Estate()
    {
        Charge ch = _inst.GetComponent("Charge") as Charge;
        ch.name = Name;
        Debug.Log("create" + Name);
        ch.q = Q;
    }   // для "доработки" объекта после создания

    public virtual void Update()
    {  // обновление нашего рефлектора
        Position = _inst.transform.position;  // согласно реальной информации об объекте
    }

}
