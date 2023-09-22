using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveLoad : MonoBehaviour
{
    public GameObject BotonLoad, BotonsSavePlus;
    public List<CurrentCharacter> savedGames = new List<CurrentCharacter>();
    public CurrentCharacter SavedCurrent;
    public void LeerArchivo()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/savedGames.rgd", FileMode.Open);
        savedGames = (List<CurrentCharacter>)bf.Deserialize(file);
        GetComponent<Principal>().current = savedGames[0];
        file.Close();

        GetComponent<Principal>().CargarEscena();
    }
    public void GuardarArchivo()
    {
        BinaryFormatter bf = new();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.rgd");
        if (savedGames.Count == 0)
            savedGames.Add(GetComponent<Principal>().current);
        else
            savedGames[0] = GetComponent<Principal>().current;
        bf.Serialize(file, savedGames);
        file.Close();
    }
    public void Save()
    {
        savedGames[0] = GetComponent<Principal>().current;
        GuardarArchivo();
    }
    public void Load()
    {
        LeerArchivo();
        GetComponent<Principal>().current = savedGames[0];
        GetComponent<Principal>().CargarEscena();
    }
}