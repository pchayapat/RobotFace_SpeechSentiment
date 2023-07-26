using System.Collections;
using System.IO;
using UnityEngine;

public class RealTimeTextFileReader : MonoBehaviour
{
    public string filePath = "Path/To/Your/TextFile.txt";
    public float updateInterval = 3f;

    private string fileContents;

    public GameObject happy;
    public GameObject sad;
    public GameObject neutral;

    private void Start()
    {
        // Start the coroutine to read the text file
        StartCoroutine(ReadTextFileRoutine());
    }

    private IEnumerator ReadTextFileRoutine()
    {
        // Initial read
        ReadTextFile();

        while (true)
        {
            // Wait for the specified update interval
            yield return new WaitForSeconds(updateInterval);

            // Check for updates and read the file again if it has been modified
            if (HasFileChanged())
            {
                ReadTextFile();
            }
        }
    }

    private void ReadTextFile()
    {
        try
        {
            fileContents = File.ReadAllText(filePath);
            // Use 'fileContents' as needed, e.g., display it in a UI Text component
            Debug.Log("File updated. Contents: " + fileContents);
            if (fileContents == "Overall sentiment: Positive")
            {
                happy.gameObject.SetActive(true);
                sad.gameObject.SetActive(false);
                neutral.gameObject.SetActive(false);
            }
            else if (fileContents == "Overall sentiment: Negative")
            {
                happy.gameObject.SetActive(false);
                sad.gameObject.SetActive(true);
                neutral.gameObject.SetActive(false);
            }
            else if (fileContents == "Overall sentiment: Neutral")
            {
                happy.gameObject.SetActive(false);
                sad.gameObject.SetActive(false);
                neutral.gameObject.SetActive(true);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error reading text file: " + e.Message);
        }
    }

    private bool HasFileChanged()
    {
        try
        {
            // Get the last write time of the file
            System.DateTime lastWriteTime = File.GetLastWriteTime(filePath);
            // Get the current time
            System.DateTime currentTime = System.DateTime.Now;
            // Check if the file was modified within the update interval
            return (currentTime - lastWriteTime).TotalSeconds <= updateInterval;
        }
        catch
        {
            // In case of any error, assume the file has not changed
            return false;
        }
    }
}
