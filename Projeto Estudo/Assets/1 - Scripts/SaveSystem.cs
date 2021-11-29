using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem {
    public static void SaveState(int health, int points, int pop, int maxPop, bool[] unitsList, int week, int[] cards) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "State.cringe";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(health, points, pop, maxPop, unitsList, week, cards);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadState() {

        string path = Application.persistentDataPath + "State.cringe";

        if (File.Exists(path)) {

            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            
            return data;

        }
        else {
            //Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    public static void ClearState() {

        string path = Application.persistentDataPath + "State.cringe";

        if (File.Exists(path)) {

            File.Delete(path);

        }
        else {
            Debug.LogError("No file to clear at " + path);
        }
    }
}
