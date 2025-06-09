using System.Collections.Generic;
using UnityEngine;

public class VariableManager : MonoBehaviour
{
    private Dictionary<string, int> variables = new Dictionary<string, int>();

    public void SetVariable(string name, int value)
    {
        if (!IsValidVariableName(name))
            throw new System.ArgumentException($"Invalid variable name: {name}");
        variables[name] = value;
    }

    public int GetVariable(string name)
    {
        if (variables.TryGetValue(name, out int value))
            return value;
        return 0;
    }

    public bool VariableExists(string name) => variables.ContainsKey(name);

    private bool IsValidVariableName(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;

        char firstChar = name[0];

        if (!char.IsLetter(firstChar) && firstChar != '_') return false;
        foreach (char c in name)
        {
            if (!char.IsLetterOrDigit(c) && c != '_')
                return false;
        }
        return true;
    }

    public void Clear()
    {
        variables.Clear();
    }
}
