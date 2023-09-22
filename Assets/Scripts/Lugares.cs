using System.Collections.Generic;
[System.Serializable]
public class Lugares
{
    public int id;
    public string name;
    public string description;
}
[System.Serializable]
public class Lugaress
{
    public List<Lugares> lugares;
}
