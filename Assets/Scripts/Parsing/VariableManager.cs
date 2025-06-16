using System.Collections.Generic;
using UnityEngine;

public class VariableManager : MonoBehaviour
{
    private Dictionary<string, ExValue> variables = new Dictionary<string, ExValue>();

    public void SetVariable(string name, ExValue value)
    {
        if (!IsValidVariableName(name))
            throw new System.ArgumentException($"Invalid variable name: {name}");
        variables[name] = value;
    }

    public ExValue GetVariable(string name) => variables.TryGetValue(name, out ExValue value) ? value : new ExValue(0);

    public bool VariableExists(string name) => variables.ContainsKey(name);

    private bool IsValidVariableName(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;

        char firstChar = name[0];

        if (!char.IsLetter(firstChar) && firstChar != '_') return false;
        foreach (char c in name)
        {
            if (!char.IsLetterOrDigit(c) && c != '_' && c!= '-')
                return false;
        }
        return true;
    }

    public void Clear()
    {
        variables.Clear();
    }
}
