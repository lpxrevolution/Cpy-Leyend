using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UserConfig
{
    public float tama�otamaTxt;
    public string Idioma;
    public bool Tuts;

    public void ConfInic()
    {
        tama�otamaTxt = 1;
        Idioma = "ES";
        Tuts = true;
    }
}
