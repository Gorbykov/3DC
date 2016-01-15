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

[XmlType("Structure Data")]
public class StrData : ChData
{
    [XmlElement("Arg")]
    public string Arg { set; get; }

    [XmlElement("MinX")]
    public float MinX { set; get; }

    [XmlElement("MinY")]
    public float MinY { set; get; }

    [XmlElement("MinZ")]
    public float MinZ { set; get; }

    [XmlElement("MaxX")]
    public float MaxX { set; get; }

    [XmlElement("MaxY")]
    public float MaxY { set; get; }

    [XmlElement("MaxZ")]
    public float MaxZ { set; get; }

    [XmlElement("Delta")]
    public float Delta { set; get; }

    public StrData() { }

    public StrData(string type, string name, float q, Vector3 position, string arg, float minx, float miny, float minz, float maxx, float maxy, float maxz, float delta) : base(type, name, q, position)
    {
        this.Arg = arg;
        this.MinX = minx;
        this.MinY = miny;
        this.MinZ = minz;
        this.MaxX = maxx;
        this.MaxY = maxy;
        this.MaxZ = maxz;
        this.Delta = delta;
    }

    public override void Estate()
    {
        Structure str = _inst.GetComponent("Structure") as Structure;
        str.name = Name;
        Debug.Log("create" + Name);
        str.q = Q;
        str.minx = MinX;
        str.miny = MinY;
        str.minz = MinZ;
        str.maxx = MaxX;
        str.maxy = MaxY;
        str.maxz = MaxZ;
        str.arg = Arg;
        str.delta = Delta;
    }
}