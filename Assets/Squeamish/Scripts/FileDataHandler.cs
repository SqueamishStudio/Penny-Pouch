using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string path;
    private string name;

    public string LoadedString;

    public FileDataHandler(string path)
    {
        this.path = path;
        name = "data.json";
    }

    public void Save(string data)
    {
        string fullPath = Path.Combine(path, name);
        if (!File.Exists(fullPath))
        {
            FileStream f = File.Create(fullPath);
            f.Close();
            f.Dispose();
        }
        try
        {
            File.WriteAllText(fullPath, data);
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }
    }

    public string Load(string Default)
    {  
        // Check for file exsistance
        if (File.Exists(Path.Combine(path, name)))
        {
            return File.ReadAllText(Path.Combine(path, name));

        } else // Create file if it doesn't exist
        {
            return Default;
        }
    }
}
