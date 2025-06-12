using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CodeExecutor : MonoBehaviour
{
    [SerializeField] private InputField codeEditor;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private InputField fileNameInput;
    [SerializeField] private Button executeButton;

    private string projectPath;

    void Start()
    {
        executeButton.onClick.AddListener(ExecuteCode);
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
        var lexer = new Lexer(code);
        var tokens = lexer.Tokenize(code);
        if (lexer.LexerErrors.Count > 0)
        {
            foreach (var error in lexer.LexerErrors)
            {
                Debug.LogError($"Lexical Error: {error}");
            }
            return;
        }
        foreach (var token in tokens)
        {
            Debug.Log($"Token: {token.Type} - {token.Value} at {token.Line}:{token.Column}");
        }

        var parser = new Parser();
        List<ASTNode> nodes = parser.Parse(tokens);

        ExecuteCommands(nodes);
    }

    private void ExecuteCommands(List<ASTNode> nodes)
    {
        var context = new Context(new Wall_E(), gridManager, new VariableManager());
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] is LabelNode labelNode)
            {
                if (context.Labels.ContainsKey(labelNode.LabelName))
                {
                    context.SetError(i, $"The label already exists: {labelNode.LabelName}");
                    return;
                }
                context.Labels.Add(labelNode.LabelName, i);
            }
        }
        context.Counter = 0;
        while (context.Counter < nodes.Count && !context.HasError())
        {
            try
            {
                nodes[context.Counter].Execute(context);
                context.Counter++;
            }
            catch (System.Exception ex)
            {
                context.SetError(context.Counter, ex.Message);
                Debug.LogError($"Execution Error at line {context.Counter + 1}: {ex.Message}");
                return;
            }
            if (context.HasError())
            {
                Debug.LogError($"Error en línea {context.ErrorLine}: {context.ErrorMessage}");
            }
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
