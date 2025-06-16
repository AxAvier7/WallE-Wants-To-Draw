using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CodeExecutor : MonoBehaviour
{
    [SerializeField] private InputField codeEditor;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private InputField fileNameInput;
    [SerializeField] private Button executeButton;
    [SerializeField] private Text errorDisplayText;

    private string projectPath;

    void Start()
    {
        executeButton.onClick.AddListener(ExecuteCode);
        projectPath = Application.dataPath + "/SavedCodes/";
        if (!Directory.Exists(projectPath))
        {
            Directory.CreateDirectory(projectPath);
        }
    }

    public void ExecuteCode()
    {
        gridManager.ClearGrid();

        string code = codeEditor.text;
        var lexer = new Lexer(code);
        var tokens = lexer.Tokenize(code);
        // foreach (var token in tokens)
        // {
        //     Debug.Log($"Token: {token.Type} - Value: {token.Value} at Line: {token.Line}, Column: {token.Column}");
        // }
        if (lexer.LexerErrors.Count > 0)
        {
            DisplayLexerErrors(lexer.LexerErrors);
            return;
        }

        var parser = new Parser();
        List<ASTNode> ast = parser.Parse(tokens);

        if (parser.errors.Count > 0)
        {
            DisplayParserErrors(parser.errors);
            return;
        }

        try
        {
            ExecuteAST(ast);
        }
        catch (Exception ex)
        {
            DisplayRuntimeError(ex.Message);
        }
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
    
    private void DisplayLexerErrors(List<LexErrors> errors)
    {
        StringBuilder errorMessage = new StringBuilder("Errores léxicos:\n");
        foreach (var error in errors)
        {
            errorMessage.AppendLine($"- Línea {error.Line}: {error.Message}");
            Debug.LogError(error.ToString());
        }
        errorDisplayText.text = errorMessage.ToString();
    }

    private void DisplayParserErrors(List<ParseException> errors)
    {
        StringBuilder errorMessage = new StringBuilder("Errores sintácticos:\n");
        foreach (var error in errors)
        {
            errorMessage.AppendLine($"- Línea {error.Line}: {error.Message}");
            Debug.LogError(error.ToString());
        }
        errorDisplayText.text = errorMessage.ToString();
    }

    private void DisplayRuntimeError(string message)
    {
        errorDisplayText.text = $"Error en ejecución:\n{message}";
        Debug.LogError(message);
    }
}