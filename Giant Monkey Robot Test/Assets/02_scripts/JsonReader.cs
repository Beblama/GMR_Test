using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class JsonReader : MonoBehaviour
{
    [Header("References")]
    //References of all needed GameObjects and componenets to create the table.
    [SerializeField]
    Text tableTitle;
    [SerializeField]
    Transform columnLayout;
    [SerializeField]
    GameObject tableColumnPrefab;
    [SerializeField]
    GameObject tableRowTextPrefab;    

    [Header("Settings")]
    //String to modify the json file path if ever needed.
    [SerializeField]
    string jsonFilePath;
    [SerializeField]
    string[] AllVariables;

    //private variables to hold information about columns, json and serializable data
    List<GameObject> AllColumns = new List<GameObject>();
    string json;
    DataCollection dataCollection;

    //Calls function LoadData
    private void Start()
    {
        //Json file is read into string
        json = File.ReadAllText(Application.streamingAssetsPath + jsonFilePath);
        LoadData(json);
    }

    //Calls function CheckCurrentJson
    private void Update()
    {
        CheckCurrentJson();
    }

    //Loads information from the Json file and displays it on screen.
    void LoadData(string JSON)
    {
        //Information is deserialized into dataCollection
        dataCollection = JsonUtility.FromJson<DataCollection>(JSON);       

        //A single column is created and added to the AllColumns list
        GameObject columnToUse = Instantiate(tableColumnPrefab, columnLayout);
        AllColumns.Add(columnToUse);
        
        //For every Data (Team Member) a single text field is added to the column for further use.
        for (int i = 0; i < dataCollection.Data.Length; i++)
        {
            Instantiate(tableRowTextPrefab, columnToUse.transform);
        }

        //After all "rows" are added, our column is duplicated. Columns are added until we have one for each header.
        for (int i = 1; i < dataCollection.ColumnHeaders.Length; i++)
        {
            GameObject newColumn = Instantiate(columnToUse, columnLayout);
            AllColumns.Add(newColumn);
        }

        //Set texts on screen
        AssignTexts();
    }
    
    //Assigns the proper string to each of the the texts on the table
    void AssignTexts()
    {
        //Title text assigned 
        tableTitle.text = dataCollection.Title;

        //Header texts assigned
        for (int i = 0; i < AllColumns.Count; i++)
        {
            AllColumns[i].transform.GetChild(0).GetComponent<Text>().text = dataCollection.ColumnHeaders[i];
        }
        
        //Table information text assigned
        //EACH COLUMN TEXT IS HARDCODED HERE, KEEP IN MIND IF YOU WISH TO ADD MORE COLUMNS/CATEGORIES
        for (int e = 1; e < dataCollection.Data.Length + 1; e++)
        {
            AllColumns[0].transform.GetChild(e).GetComponent<Text>().text = dataCollection.Data[e - 1].ID;
            AllColumns[1].transform.GetChild(e).GetComponent<Text>().text = dataCollection.Data[e - 1].Name;
            AllColumns[2].transform.GetChild(e).GetComponent<Text>().text = dataCollection.Data[e - 1].Role;
            AllColumns[3].transform.GetChild(e).GetComponent<Text>().text = dataCollection.Data[e - 1].Nickname;
        }
    }

    //Checks if Json has changed, reloads data if it has
    void CheckCurrentJson()
    {
        string newJson = File.ReadAllText(Application.streamingAssetsPath + jsonFilePath);
        if (newJson != json)
        {
            DeleteThenLoad(newJson);
        }
    }

    //Deletes previous columns, clears column list and calls function LoadData
    void DeleteThenLoad(string newJson)
    {
        foreach (GameObject gO in AllColumns)
        {
            Destroy(gO);
        }
        AllColumns.Clear();
        LoadData(newJson);
    }
}