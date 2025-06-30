using System.Collections.Generic;
using UnityEngine;

//Contexto del interprete
public class Context
{
    public Wall_E WallE { get; }
    public GridManager GridManager { get; }
    public VariableManager Variables { get; }
    public Dictionary<string, int> Labels { get; } = new Dictionary<string, int>();
    public int Counter { get; set; }
    public int ErrorLine { get; set; } = -1;
    public string ErrorMessage { get; private set; }

    public Context(Wall_E wallE, GridManager gridManager, VariableManager variables)
    {
        WallE = wallE;
        GridManager = gridManager;
        Variables = variables;
    }

    //Muestra errores en consola
    public void SetError(int line, string message)
    {
        ErrorLine = line;
        ErrorMessage = message;
        Debug.LogError($"Error at line {line}: {message}");
    }

    //Indica al interprete que continue el parsing en el label que corresponda
    public void ExecuteGoTo(string label)
    {
        if (Labels.TryGetValue(label, out int target))
        {
            Counter = target;
        }
        else
        {
            SetError(Counter, $"Label '{label}' not found.");
        }
    }

    //Guarda un label
    public void RegisterLabel(string label, int lineNumber)
    {
        if (Labels.ContainsKey(label))
        {
            SetError(lineNumber, $"Duplicate label: {label}");
            return;
        }
        Labels.Add(label, lineNumber);
    }

    public bool HasError() => ErrorLine != -1;
}