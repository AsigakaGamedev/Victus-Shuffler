using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordsManager : MonoBehaviour
{
    public int GetRecord(GameDificulty dificulty)
    {
        switch (dificulty)
        {
            case GameDificulty.Begginer: return PlayerPrefs.GetInt("begginer_record", 0); 
            case GameDificulty.Advanced: return PlayerPrefs.GetInt("advanced_record", 0); 
            case GameDificulty.Pro: return PlayerPrefs.GetInt("pro_record", 0);
            default: return 0;
        }
    }

    public void SetRecord(GameDificulty dificulty, int newRecord)
    {
        int oldRecord = GetRecord(dificulty);

        string recordKey = "";

        switch (dificulty)
        {
            case GameDificulty.Begginer: recordKey = "begginer_record"; break;
            case GameDificulty.Advanced: recordKey = "advanced_record"; break;
            case GameDificulty.Pro: recordKey = "pro_record"; break;
        }

        if (oldRecord < newRecord)
        {
            PlayerPrefs.SetInt(recordKey, newRecord);
        }
    }
}
