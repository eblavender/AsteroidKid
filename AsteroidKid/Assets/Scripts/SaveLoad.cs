using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.instance;

        if (BinarySaveLoad.CheckFileExists("/SaveData/", "GameSave"))
            LoadSaveData();
        else
            SaveData();
    }

    /// <summary>
    /// Stores the current variables to save file
    /// </summary>
    public void SaveData()
    {
        SaveInfo save = new SaveInfo();

        SaveToGame(ref save);
        if (BinarySaveLoad.CheckFileExists("/SaveData/", "GameSave"))
        {
            BinarySaveLoad.MarshalData(save, "/SaveData/", "GameSave");
        }
    }
    /// <summary>
    /// Pulls the stored save data
    /// </summary>
    public void LoadSaveData()
    {
        SaveInfo save = new SaveInfo();
        object obj = new object();
        obj = save;

        if (BinarySaveLoad.CheckFileExists("/SaveData/", "GameSave"))
        {
            BinarySaveLoad.UnMarshalData(ref obj, "/SaveData/", "GameSave", save.GetType());
            save = (SaveInfo)obj;

            GameToSave(ref save);
        }
    }

    /// <summary>
    /// Parses save data to game
    /// </summary>
    /// <param name="save"></param>
    private void GameToSave(ref SaveInfo save)
    {
        int.TryParse(save.highscore.ToString(), out manager.highScore);
    }
    /// <summary>
    /// Parses game data to save
    /// </summary>
    /// <param name="save"></param>
    private void SaveToGame(ref SaveInfo save)
    {
        save.highscore = manager.highScore;

    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
