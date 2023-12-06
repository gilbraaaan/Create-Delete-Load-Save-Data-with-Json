using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectData : MonoBehaviour
{
    public User user;
    public string nameData;
    public int valueData;
    public TMP_Text userDataNameCreated;
    public TMP_Text[] getTextForShow;

    public void inputData()
    {
        userDataNameCreated.text = nameData;
        user.BtnCreated.Add(this.gameObject);
    }
    public void Selected()
    {
        for(int t = 0; t < getTextForShow.Length; t++)
        {
            getTextForShow[t].text = nameData;
        }
        user.SelectedDataName = nameData;
        user.rangeDataName = valueData;
    }
}
