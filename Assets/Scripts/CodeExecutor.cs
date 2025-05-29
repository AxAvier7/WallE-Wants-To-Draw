using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CodeExecutor : MonoBehaviour
{
    [SerializeField] private InputField codeEditor;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private InputField fileNameInput;

    private string projectPath;

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


    void Start()
    {
        projectPath = Application.dataPath + "/SavedCodes/";
        if(!Directory.Exists(projectPath))
        {
            Directory.CreateDirectory(projectPath);
        }
    }

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

    public void SaveCode()
    {
        string fileName = fileNameInput.text;
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.LogError("Nombre de archivo vacío");
            return;
        }
        string fullPath = projectPath + fileName + ".pw";
        File.WriteAllText(fullPath, codeEditor.text);
        Debug.Log($"Archivo guardado en: {fullPath}");
    }

    public void LoadCode()
    {
        string fileName = fileNameInput.text;
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.LogError("Nombre de archivo vacío");
            return;
        }
        string fullPath = projectPath + fileName + ".pw";
        if (File.Exists(fullPath))
        {
            codeEditor.text = File.ReadAllText(fullPath);
            Debug.Log($"Archivo cargado desde: {fullPath}");
        }
        else
        {
            Debug.LogError($"Archivo no encontrado: {fullPath}");
        }
    }

}
