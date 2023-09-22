using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Armament
{
    public int id, type;
    public string name, mod, description, stat, value;
}
[System.Serializable]
public class Armaments
{
    public List<Armament> armament;
}
