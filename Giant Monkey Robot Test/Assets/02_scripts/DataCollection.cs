using System;

//Holds JSON main information
[Serializable]
public class DataCollection
{
    public string Title;
    public string[] ColumnHeaders;
    public DataContent[] Data;
}