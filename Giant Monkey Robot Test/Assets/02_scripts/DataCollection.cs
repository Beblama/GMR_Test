using System;

//Holds all the information from the JSON
[Serializable]
public class DataCollection
{
    public string Title;
    public string[] ColumnHeaders;
    public DataContent[] Data;
}