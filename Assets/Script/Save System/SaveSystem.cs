using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void SaveGameState(RecordData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveGame.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static RecordData LoadGameState()
    {
        string path = Application.persistentDataPath + "/SaveGame.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            RecordData  data = formatter.Deserialize(stream) as RecordData;

            stream.Close();
            return data;

        }
        else
        {
            Debug.LogError("file not found");
            return null;


        }
    }

    public static void AddGems(int num)
    {
        RecordData data = SaveSystem.LoadGameState();
        data.gems += num;
        if (num > data.score)
            data.score = num;
        SaveSystem.SaveGameState(data);

    }



}
   

