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
        var lexer = new Lexer(code);
        var tokens = lexer.Tokenize(code);
        foreach (var token in tokens)
        {
            Debug.Log($"Token: {token.Type} - Value: {token.Value} at Line: {token.Line}, Column: {token.Column}");
        }
        
        var parser = new Parser();
        List<ASTNode> ast = parser.Parse(tokens);
        
        if (parser.errors.Count > 0)
        {
            foreach (var error in parser.errors)
            {
                Debug.LogError($"Syntax error at line {error.Line}: {error.Message}");
            }
            return;
        }
        
        ExecuteAST(ast);
    }

    private void ExecuteAST(List<ASTNode> nodes)
    {
        var context = new Context(new Wall_E(), gridManager, new VariableManager());
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] is LabelNode labelNode)
            {
                context.RegisterLabel(labelNode.LabelName, i);
            }
        }

        context.Counter = 0;
        while (context.Counter < nodes.Count && !context.HasError())
        {
            var node = nodes[context.Counter];

            if (node is LabelNode)
            {
                context.Counter++;
                continue;
            }

            try
            {
                node.Execute(context);
                context.Counter++;
            }
            catch (Exception ex)
            {
                context.SetError(context.Counter, ex.Message);
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
