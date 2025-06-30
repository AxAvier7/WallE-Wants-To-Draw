using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

//Clase que ejecuta el codigo al pulsar un boton en la IU
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

    //Metodo que controla todo el proceso de ejecución del código introducido en el InputField
    public void ExecuteCode()
    {
        gridManager.ClearGrid();

        string code = codeEditor.text;

        //El lexer procesa el texto para devolver los tokens
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

        //El Parser convierte los tokens en un AST
        var parser = new Parser();
        List<ASTNode> ast = parser.Parse(tokens);

        if (parser.errors.Count > 0)
        {
            DisplayParserErrors(parser.errors);
            return;
        }

        //Se intenta ejecutar el AST generado por el Parser
        try
        {
            ExecuteAST(ast);
        }
        catch (Exception ex)
        {
            DisplayRuntimeError(ex.Message);
        }
    }

    //Metodo que ejecuta todos los nodos del AST generado por el Parser
    private void ExecuteAST(List<ASTNode> nodes)
    {
        var context = new Context(new Wall_E(), gridManager, new VariableManager());
        for (int i = 0; i < nodes.Count; i++)
        {
            //si alguno de los nodos es una etiqueta se registra en el contexto
            if (nodes[i] is LabelNode labelNode)
            {
                context.RegisterLabel(labelNode.LabelName, i);
            }
        }

        //Counter sirve para saber que nodo se va a ejecutar
        context.Counter = 0;

        while (context.Counter < nodes.Count && !context.HasError())
        {
            var node = nodes[context.Counter];

            //Si el nodo es una etiqueta se ignora
            if (node is LabelNode)
            {
                context.Counter++;
                continue;
            }

            //Intenta ejecutar el nodo y si no puede lanza una excepcion de ejecucion
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

    //Metodo que guarda un archivo .pw con el codigo introducido
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

    //Carga los codigos con formato .pw
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

    //Metodo que muestra los errores léxicos en la IU
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

    //Similar al metodo anterior pero con errores del Parser
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

    //Metodo que recibe cualquier excepcion que se lance al ejecutar el AST
    private void DisplayRuntimeError(string message)
    {
        errorDisplayText.text = $"Error en ejecución:\n{message}";
        Debug.LogError(message);
    }
}