using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    
    public static void SaveGame(Player player)
    {
        // saves data of the player (including lvl, health, xp, etc)
        BinaryFormatter formatter = new BinaryFormatter();
        string path;
        path = Application.persistentDataPath + "/PlayerData.pschwrd";
        
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }


    public static PlayerData LoadPlayer()
    {
        // making the same variable again like a boss
        string path;
        path = Application.persistentDataPath + "/PlayerData.pschwrd";


        if (File.Exists(path))
        {
            // load data
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveStorage(StorageBox storage)
    {
        // saves the items held in the storage box
        BinaryFormatter formatter = new BinaryFormatter();
        string path;
        path = Application.persistentDataPath + "/StorageData.pschwrd";

        FileStream stream = new FileStream(path, FileMode.Create);

        StorageData data = new StorageData(storage);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static StorageData LoadStorage()
    {
        string path;
        path = Application.persistentDataPath + "/StorageData.pschwrd";


        if (File.Exists(path))
        {
            // load data
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            StorageData data = formatter.Deserialize(stream) as StorageData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    /*public static void SaveSettings(SystemSettings settings)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.poo";
        FileStream stream = new FileStream(path, FileMode.Create);

        SystemData data = new SystemData(settings);

        formatter.Serialize(stream, data);
        stream.Close();
    } */

    /*public static SystemData LoadSettings()
    {
        string path = Application.persistentDataPath + "/settings.poo";
        if (File.Exists(path))
        {
            // load data
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SystemData data = formatter.Deserialize(stream) as SystemData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    } */
   
    // check if a save file for the player exists and if it doesn't, return false 
    public static bool DoesPlayerFileExist()
    {
        string path = Application.persistentDataPath + "/PlayerData.pschwrd";
        if (File.Exists(path))
        {
            Debug.Log("exists");
            return true;
        }
        else
        {
            Debug.Log("Doesn't exist for some reason");
            return false;
        }
    }

    public static bool DoesSettingsFileExist()
    {
        string path = Application.persistentDataPath + "/settings.poo";
        if (File.Exists(path))
        {
            Debug.Log("exists");
            return true;
        }
        else
        {
            Debug.Log("Doesn't exist for some reason");
            return false;
        }
    }

    public static bool DoesStorageFileExist()
    {
        string path = Application.persistentDataPath + "/StorageData.pschwrd";

        if (File.Exists(path))
        {
            Debug.Log("StorageData exists");
            return true;
        }
        else
        {
            Debug.Log("StorageData doesn't exist");
            return false;
        }    
    }

    // initially create a player file with data and do nothing if there is one
    public static void CreatePlayerFile(Player player)
    {
        string path;


        path = Application.persistentDataPath + "/PlayerData.pschwrd";

        if (!File.Exists(path))
        {
            Debug.Log("GameData doesn't exist, creating!");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(player);

            formatter.Serialize(stream, data);
            stream.Close();
        }
        else
        {
            Debug.Log("GameData exists, doing nothing!");
            return;
        }
    }

    public static void CreateStorageFile(StorageBox storage)
    {
        string path = Application.persistentDataPath + "/StorageData.pschwrd";

        if (!File.Exists(path))
        {
            Debug.Log("StorageData doesn't exist, creating!");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            StorageData data = new StorageData(storage);

            formatter.Serialize(stream, data);
            stream.Close();
        }
        else
        {
            Debug.Log("StorageData exists, doing nothing!");
            return;
        }
    }

    /*public static void CreateSettingsFile(SystemSettings settings)
    {
        string path = Application.persistentDataPath + "/settings.poo";
        if (!File.Exists(path))
        {
            Debug.Log("GameData doesn't exist, creating!");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            SystemData data = new SystemData(settings);

            formatter.Serialize(stream, data);
            stream.Close();
        }
        else
        {
            Debug.Log("settings exists, doing nothing!");
            return;
        }
    } */

    public static void DeletePlayerFile()
    {

        string path;
        
        path = Application.persistentDataPath + "/PlayerData.pschwrd";
        
        File.Delete(path);
    }

    public static void DeleteSettingsFile()
    {
        string path = Application.persistentDataPath + "/settings.poo";
        File.Delete(path);
    }

    public static void DeleteStorageFile()
    {
        string path = Application.persistentDataPath + "/StorageData.pschwrd";
        File.Delete(path);
    }
}
