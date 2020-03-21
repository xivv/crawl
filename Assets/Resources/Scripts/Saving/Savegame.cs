using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Savegame : MonoBehaviour
{

    private string savePath;
    private BinaryFormatter binaryFormatter;
    private Vector3 position = new Vector3(0.5f, 2.5f, 0);

    // Start is called before the first frame update
    void Start()
    {
        this.savePath = Application.persistentDataPath + "/savegame.save";
        this.binaryFormatter = new BinaryFormatter();
    }

    public void Load()
    {
        if (File.Exists(this.savePath))
        {
            using (var fileStream = File.Open(savePath, FileMode.Open))
            {
                this.binaryFormatter.Deserialize(fileStream);
            }
        }

        SceneManager.LoadScene("WorldMap", LoadSceneMode.Single);
        PlayerController.position = position;
        // CreatePlayerObject(position);
    }

    public void Save()
    {

        using (var fileStream = File.Create(this.savePath))
        {
            this.binaryFormatter.Serialize(fileStream, null);
        }

        Debug.Log("Savegame created");
    }

    private void CreatePlayerObject(Vector2 position)
    {
        // Instantiate it by name, this needs to be fixed later
        WorldMap.CreatePlayer(position);
    }

}
