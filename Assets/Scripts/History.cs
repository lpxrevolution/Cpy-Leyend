using System.Collections.Generic;

[System.Serializable]
public class History
{
    public int id;
    public string text, text2, text3;
}
[System.Serializable]
public class Histories
{
    public List<History> history;
}