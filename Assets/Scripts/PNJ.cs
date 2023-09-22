using System.Collections.Generic;

[System.Serializable]
public class PNJ
{
    //En este script ponemos los Stats de los PNGs, están vacíos para crear personajes en el Principal y ponerlos para cada PNJ que creemos
    public int id, afinidad, salud, fuerza, defensa, suerte, edad, estatura, peso;
    public string name, complexion, description;
}
[System.Serializable]
public class PNJs
{
    public List<PNJ> pnjs;
}