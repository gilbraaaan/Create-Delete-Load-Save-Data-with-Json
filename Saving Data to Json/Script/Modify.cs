using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Modify : MonoBehaviour
{
    public User user;
    public int Catch, exp, health, gold;
    public TMP_Text ExpValue, HealthValue, GoldValue;
    public TMP_InputField ExpField, HealthField, GoldField;
    void Start()
    {
        user = FindObjectOfType<User>();
    }

    public void CatchData(int getValue)
    {
        Catch = getValue;
        ExpValue.text = user.checkPlayerList.player[Catch].EXP.ToString();
        HealthValue.text = user.checkPlayerList.player[Catch].Health.ToString();
        GoldValue.text = user.checkPlayerList.player[Catch].Gold.ToString();
        textField(ExpValue.text, HealthValue.text, GoldValue.text);
    }

    void textField(string fieldA, string fieldB, string fieldC)
    {
        exp = int.Parse(fieldA);
        health = int.Parse(fieldB);
        gold = int.Parse(fieldC);
        ExpField.text = fieldA;
        HealthField.text = fieldB;
        GoldField.text = fieldC;
    }

    public void SaveTheData()
    {
        user.DataModify(Catch, exp, health, gold);
        CatchData(Catch);
    }

    #region Set
    public void ExpSet(string set)
    {
        exp = int.Parse(set);
    }
    public void HealthSet(string set)
    {
        health = int.Parse(set);
    }
    public void GoldSet(string set)
    {
        gold = int.Parse(set);
    }
    #endregion
}
