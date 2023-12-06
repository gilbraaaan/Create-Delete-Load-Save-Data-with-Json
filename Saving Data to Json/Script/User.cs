using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

#region Atribute For Json
[System.Serializable]
public class PlayerData
{
    public string Name;
    public int EXP;
    public int Health;
    public int Gold;
}
[System.Serializable]
public class PlayerList
{
    public List<PlayerData> player;
}
#endregion
public class User : MonoBehaviour
{
    public Modify modify;

    [Header("Ui BASE :")]
    [SerializeField] GameObject UiCreate;
    [SerializeField] GameObject UiCreated, UiData;

    [Space(20f)]
    [Header("Error Notification :")]
    [SerializeField] TMP_Text TextError;

    [Space(20f)]
    [Header("USER CHECK :")]
    [SerializeField] TMP_InputField[] dataUserInput;
    [SerializeField] TMP_Text[] setTextName;
    public string DataName, saveFilePath, SelectedDataName;
    public int rangeDataName, playerDataAreMade;
    public List<GameObject> BtnCreated;
    int countData;
    [SerializeField] GameObject newData, newDataToContent;
    [SerializeField] bool boolCheckContains = false;
    string error;

    [Space(20f)]
    public PlayerList checkPlayerList = new PlayerList();
    void Start()
    {
        modify = FindObjectOfType<Modify>();
        saveFilePath = Application.dataPath + "/Saving Data To Json/Json/PlayerData.json";
        if (File.Exists(saveFilePath))
        {
            Debug.Log("File Founded");
            readJson();
        }
        else
        {
            createJson();
        }
    }

    #region Btn Menu
    public void GoToCreated()
    {
        if (checkPlayerList.player[checkPlayerList.player.Count - 1].Name == "")
        {
            checkPlayerList.player.RemoveAt(checkPlayerList.player.Count - 1);
        }
        UiCreate.SetActive(false);
        UiData.SetActive(false);
        UiCreated.SetActive(true);
        RemoveInputDataUser();
    }
    public void GoToData()
    {
        if (SelectedDataName != "")
        {
            modify.CatchData(rangeDataName);
            UiCreated.SetActive(false);
            UiData.SetActive(true);
        }else
        {
            error = "No data selected";
            StartCoroutine(ProsesData(error, false));
        }
    }
    public void GotoCreate()
    {
        if (checkPlayerList.player.Count < 3)
        {
            UiCreate.SetActive(true);
            UiCreated.SetActive(false);
        }
        else
        {
            error = "Data Name is Full";
            StartCoroutine(ProsesData(error,false));
        }
    }
    public void DeleteData()
    {
        if (SelectedDataName != "")
        {
            for(int i = 0; i < BtnCreated.Count; i++)
            {
                Destroy(BtnCreated[i]);
            }
            checkPlayerList.player.RemoveAt(rangeDataName);
            countData = 0;
            BtnCreated.Clear();
            createJson();
            SelectedDataName = "";
            for (int t = 0; t < setTextName.Length; t++)
            {
                setTextName[t].text = "";
            }
            error = "Data Deleted";
            StartCoroutine(ProsesData(error, false));
        }
        else
        {
            error = "No data selected";
            StartCoroutine(ProsesData(error, false));
        }
    }
    #endregion

    #region Input User
    public void ReadUsername(string user)
    {
        DataName = user;
    }
    #endregion

    #region Check User
    public void CreateNewData()
    {
        if (DataName != "")
        {
            boolCheckContains = true;
            foreach (var check in checkPlayerList.player)
            {
                if (check.Name.Contains(DataName))
                {
                    error = "Data Name can not be same";
                    boolCheckContains = false;
                }
            }
            StartCoroutine(ProsesData(error,boolCheckContains));
        }
        else
        {
            error = "Wrong Data Name, please write correctly";
            StartCoroutine(ProsesData(error, false));
        }
        RemoveInputDataUser();
    }
    #endregion

    #region Proses the data
    void RemoveInputDataUser()
    {
        DataName = "";
        for (int i = 0; i < dataUserInput.Length; i++)
        {
            dataUserInput[i].text = null;
        }
    }
    void createData()
    {
        checkPlayerList.player[checkPlayerList.player.Count - 1].Name = DataName;
        checkPlayerList.player[checkPlayerList.player.Count - 1].EXP = 1000;
        checkPlayerList.player[checkPlayerList.player.Count - 1].Health = 100;
        checkPlayerList.player[checkPlayerList.player.Count - 1].Gold = 5;
        createJson();
        GoToCreated();
    }
    void createJson()
    {
        string output = JsonUtility.ToJson(checkPlayerList);
        File.WriteAllText(Application.dataPath +
            "/Saving Data To Json/Json/PlayerData.json", output);
        readJson();
    }
    void readJson()
    {
        string loadPlayerData = File.ReadAllText(saveFilePath);
        checkPlayerList = JsonUtility.FromJson<PlayerList>(loadPlayerData);
        playerDataAreMade = checkPlayerList.player.Count;
        InstantiatePlayersData();
    }
    void InstantiatePlayersData()
    {
        if(countData < playerDataAreMade)
        {
            Clone();
            countData += 1;
            InstantiatePlayersData();
        }
    }
    void Clone()
    {
        GameObject data = Instantiate(newData) as GameObject;
        #region Clone Data
        data.transform.SetParent(newDataToContent.transform);
        data.GetComponent<SelectData>().user = this.GetComponent<User>();
        data.transform.localPosition = new Vector3(data.transform.localPosition.x,
            data.transform.localPosition.y, 0f);
        data.transform.localScale = new Vector3(1f, 1f, 1f);
        data.GetComponent<SelectData>().getTextForShow = setTextName;
        data.GetComponent<SelectData>().nameData = checkPlayerList.player[countData].Name;
        data.GetComponent<SelectData>().valueData = countData;
        data.GetComponent<SelectData>().inputData();
        #endregion
    }
    #endregion

    IEnumerator ProsesData(string errorText,bool Check)
    {
        if (Check)
        {
            checkPlayerList.player.Add(new PlayerData());
            createData();
        }else
        {
            TextError.enabled = true;
            TextError.text = errorText;
        }
        yield return new WaitForSeconds(1f);
        TextError.enabled = false;
        boolCheckContains = false;
    }

    public void DataModify(int Catch, int exp, int health, int gold)
    {
        checkPlayerList.player[Catch].EXP = exp;
        checkPlayerList.player[Catch].Health = health;
        checkPlayerList.player[Catch].Gold = gold;
        string output = JsonUtility.ToJson(checkPlayerList);
        File.WriteAllText(Application.dataPath +
            "/Saving Data To Json/Json/PlayerData.json", output);
        playerDataAreMade = checkPlayerList.player.Count;
    }
}
