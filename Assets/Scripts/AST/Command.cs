using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command : StatementNode // Clase base para todos los comandos que Wall-E puede ejecutar
{
    public override void Accept(IVisitor visitor) => visitor.Visit(this);

}

public class AssignmentCommand : Command
{
    private string variableName;
    private ExpressionNode expression;
    public AssignmentCommand(string variableName, ExpressionNode expression, int line, int column)
    {
        this.variableName = variableName;
        this.expression = expression;
        Line = line;
        Column = column;
    }

    public override void Execute(Context context)
    {
        int value = expression.Evaluate(context).AsInt();
        context.Variables.SetVariable(variableName, value);
    }
}

public class ExpressionStatement : Command
{
    private ExpressionNode expression;

    public ExpressionStatement(ExpressionNode expression, int line, int column)
    {
        this.expression = expression;       
        Line = line;
        Column = column;
    }

    public override void Execute(Context context)
    {
        expression.Evaluate(context);
    }
}

public class GoToNode : Command
{
    public string Label { get; }
    public ExpressionNode Condition { get; }
    public GoToNode(string label, ExpressionNode condition, int line, int column)
    {
        Label = label;
        Condition = condition;
        Line = line;
        Column = column;
    }

    public override void Execute(Context context)
    {
        var condition = Condition.Evaluate(context);
        if (condition.AsBool())
        {
            context.ExecuteGoTo(Label);
        }
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}


public class Spawn : Command //Posiciona al Wall-E en las coordenadas (x, y) del grid
{
    ExpressionNode x, y;
    public Spawn(ExpressionNode x, ExpressionNode y, int line, int column)
    {
        this.x = x;
        this.y = y;
        Line = line;
        Column = column;
    }
    public override void Execute(Context context)
    {
        int xValue = x.Evaluate(context).AsInt();
        int yValue = y.Evaluate(context).AsInt();
        
        if (xValue < 0 || xValue >= context.GridManager.Width || 
            yValue < 0 || yValue >= context.GridManager.Height)
        {
            throw new Exception($"Spawn position ({xValue}, {yValue}) is out of bounds");
        }
        
        context.WallE.SetSpawnPoint(xValue, yValue);
        Debug.Log($"Spawn set at ({xValue}, {yValue})");
    }
}

public class Color : Command //Cambia el color del WallE
{
    string color;
    public Color(string color, int line, int column)
    {
        if (string.IsNullOrEmpty(color))
            throw new ArgumentException("Color cannot be null or empty");
        this.color = color;
        Line = line;
        Column = column;
    }
    public override void Execute(Context context)
    {
        context.WallE.SetColor(color);
        Debug.Log($"Brush color set to {color}");
    }
}

public class Size : Command //Modifica el tamaño del pincel
{
    ExpressionNode k;
    public Size(ExpressionNode k, int line, int column)
    {
        this.k = k;
        Line = line;
        Column = column;
    }
    public override void Execute(Context context)
    {
        int kValue = k.Evaluate(context).AsInt();
        int actualSize = kValue % 2 == 0 ? kValue - 1 : kValue;
        if (actualSize <= 0)
            throw new Exception("Brush size must be greater than 0");
        context.WallE.SetBrushSize(actualSize);
        Debug.Log($"Brush size set to {actualSize}");
    }
}

public class DrawLine : Command //Dibuja una linea desde la posicion de WallE y termina en la distancia "distance" en la direccion indicada
{
    ExpressionNode dirX, dirY, distance;

    public DrawLine(ExpressionNode dirX, ExpressionNode dirY, ExpressionNode distance, int line, int column)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.distance = distance;
        Line = line;
        Column = column;
    }
    public override void Execute(Context context)
    {
        int dirX = this.dirX.Evaluate(context).AsInt();
        int dirY = this.dirY.Evaluate(context).AsInt();
        int distance = this.distance.Evaluate(context).AsInt();

        if (context.WallE.currentColor == "Transparent")
        {
            context.WallE.Move(dirX * distance, dirY * distance);
            return;
        }

        if (!DrawingUtils.IsValidDirection(dirX, dirY))
            context.SetError(Line, "Invalid direction for drawing line");

        int endX = context.WallE.X + dirX * (distance - 1);
        int endY = context.WallE.Y + dirY * (distance - 1);

        DrawingUtils.DrawLineBresenham(context, context.WallE.X, context.WallE.Y, endX, endY);

        context.WallE.SetSpawnPoint(endX, endY);
        Debug.Log($"Drew a line from ({context.WallE.X},{context.WallE.Y}) to ({context.WallE.X + dirX * distance -1},{context.WallE.Y + dirY * distance-1}) with {context.WallE.currentColor}");
    }
}

public class DrawCircle : Command  //Dibuja un circulo con centro en la posicion de WallE y con radio "radius"
{
    ExpressionNode dirX, dirY, radius;
    public DrawCircle(ExpressionNode dirX, ExpressionNode dirY, ExpressionNode radius, int line, int column)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.radius = radius;
        Line = line;
        Column = column;
    }
    public override void Execute(Context context)
    {
        int dirX = this.dirX.Evaluate(context).AsInt();
        int dirY = this.dirY.Evaluate(context).AsInt();
        int radius = this.radius.Evaluate(context).AsInt();

        if (radius < 0)
            throw new ArgumentException("Radius must be positive");

        if (!DrawingUtils.IsValidDirection(dirX, dirY))
            if (context.WallE.currentColor == "Transparent")
            {
                context.WallE.Move(dirX * radius, dirY * radius);
                return;
            }

        int centerX = context.WallE.X + dirX * radius;
        int centerY = context.WallE.Y + dirY * radius;

        if(radius==0) DrawingUtils.DrawBrushAt(context, centerX, centerY);

        if (centerX < 0 || centerX >= context.GridManager.Width || centerY < 0 || centerY >= context.GridManager.Height)
        {
            context.SetError(Line, "Circle center out of bounds");
            return;
        }

        DrawingUtils.DrawCirclePoints(context, centerX, centerY, radius);
        context.WallE.SetSpawnPoint(centerX, centerY);
    }
}

public class DrawRectangle : Command // Dibuja un rectangulo con esquina superior izquierda en la posicion de WallE y con ancho "width" y alto "height"
{
    ExpressionNode dirX, dirY, distance, width, height;
    public DrawRectangle(ExpressionNode dirX, ExpressionNode dirY, ExpressionNode distance, ExpressionNode width, ExpressionNode height, int line, int column)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.distance = distance;
        this.width = width;
        this.height = height;
        Line = line;
        Column = column;
    }

    public override void Execute(Context context)
    {
        int dirX = this.dirX.Evaluate(context).AsInt();
        int dirY = this.dirY.Evaluate(context).AsInt();
        int distance = this.distance.Evaluate(context).AsInt();
        int width = this.width.Evaluate(context).AsInt();
        int height = this.height.Evaluate(context).AsInt();


        if (width < 0 || height < 0)
        {
            context.SetError(Line, "Rectangle dimensions must be positive");
            return;
        }

        if (context.WallE.currentColor == "Transparent")
        {
            context.WallE.Move(dirX * distance, dirY * distance);
            return;
        }

        int centerX = context.WallE.X + dirX * distance;
        int centerY = context.WallE.Y + dirY * distance;

        int halfWidth = width / 2;
        int halfHeight = height / 2;

        int left = centerX - halfWidth;
        int top = centerY - halfHeight;
        int right = centerX + halfWidth;
        int bottom = centerY + halfHeight;

        if (centerX < 0 || centerX >= context.GridManager.Width || centerY < 0 || centerY >= context.GridManager.Height)
        {
            context.SetError(Line, $"Rectangle center ({centerX},{centerY}) out of bounds");
            return;
        }

        DrawingUtils.DrawLineBetweenPoints(context, left, top, right, top);
        DrawingUtils.DrawLineBetweenPoints(context, right, top, right, bottom);
        DrawingUtils.DrawLineBetweenPoints(context, right, bottom, left, bottom);
        DrawingUtils.DrawLineBetweenPoints(context, left, bottom, left, top);

        context.WallE.SetSpawnPoint(centerX, centerY);
    }
}

public class Fill : Command // Pinta con el color actual todos los pixeles del mismo color que la posicion de WallE contiguos a este
{
    public Fill(int line, int column)
    {
        Line = line;
        Column = column;
    }
    public override void Execute(Context context)
    {
        if (context.WallE.currentColor == "Transparent")
            return;

        string targetColor = context.GridManager.GetPixelColorName(context.WallE.X, context.WallE.Y);

        if (targetColor == context.WallE.currentColor)
            return;

        DrawingUtils.FloodFill(context, context.WallE.X, context.WallE.Y, targetColor);
    }
}