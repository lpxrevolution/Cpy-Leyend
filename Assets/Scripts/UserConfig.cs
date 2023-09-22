using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UserConfig
{
    public float tamañotamaTxt;
    public string Idioma;
    public bool Tuts;

    public void ConfInic()
    {
        tamañotamaTxt = 1;
        Idioma = "ES";
        Tuts = true;
    }
}
