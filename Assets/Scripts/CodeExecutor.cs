using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CodeExecutor : MonoBehaviour
{
    [SerializeField] private InputField codeEditor;
    [SerializeField] private GridManager gridManager;

    private Wall_E WallE;
    private Dictionary<string, UnityEngine.Color> colorMap = new Dictionary<string, UnityEngine.Color>()
    {
        {"Red", UnityEngine.Color.red},
        {"Green", UnityEngine.Color.green},
        {"Blue", UnityEngine.Color.blue},
        {"Yellow", UnityEngine.Color.yellow},
        {"Black", UnityEngine.Color.black},
        {"White", UnityEngine.Color.white},
        {"Orange", new UnityEngine.Color(1f, 0.65f, 0f)},
        {"Purple", new UnityEngine.Color(0.5f, 0f, 0.5f)},
        {"Transparent", new UnityEngine.Color(0, 0, 0, 0)}
    };

    public void ExecuteCode()
    {
        string code = codeEditor.text;
        ProcessCode(code);
    }

    private void ProcessCode(string code)
    {
        string[] lines = code.Split('\n');
        foreach (var line in lines)
        {
            throw new NotImplementedException("Code execution is not implemented yet.");
        }
    }

    public void LoadCode()
    {
        string path = UnityEngine.Application.persistentDataPath + "/saved_code.pw";
        if (File.Exists(path))
        {
            string code = File.ReadAllText(path);
            codeEditor.text = code;
        }
    }
    
    public void SaveCode()
    {
        string path = UnityEngine.Application.persistentDataPath + "/saved_code.pw";
        File.WriteAllText(path, codeEditor.text);
    }
}
