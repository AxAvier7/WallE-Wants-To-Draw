using UnityEngine;

namespace WallE_Class
{
    public abstract class Expression
    {
        public virtual int Execute(Wall_E wall_E, GridManager gridManager)
        {
            return int.MaxValue;
        }
    }

    public class GetActualX : Expression
    {
        public override int Execute(Wall_E wall_E, GridManager gridManager)
        {
            return wall_E.GetActualX();
        }
    }

    public class GetActualY : Expression
    {
        public override int Execute(Wall_E wall_E, GridManager gridManager)
        {
            return wall_E.GetActualY();
        }
    }

    public class GetCanvasSize : Expression
    {
        public override int Execute(Wall_E wall_E, GridManager gridManager)
        {
            return gridManager.Width;
        }
    }

    public class GetColorCount : Expression
    {
        string color;
        int x1, y1, x2, y2;

        public GetColorCount(string color, int x1, int y1, int x2, int y2)
        {
            this.color = color;
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }
        public override int Execute(Wall_E wall_E, GridManager gridManager)
        {
            int minX = Mathf.Min(x1, x2);
            int maxX = Mathf.Min(x1, x2);
            int minY = Mathf.Min(y1, y2);
            int maxY = Mathf.Min(y1, y2);

            minX = Mathf.Clamp(minX, 0, gridManager.Width - 1);
            maxX = Mathf.Clamp(maxX, 0, gridManager.Width - 1);
            minY = Mathf.Clamp(minY, 0, gridManager.Height - 1);
            maxY = Mathf.Clamp(maxY, 0, gridManager.Height - 1);

            int count = 0;

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    if (gridManager.GetPixelColorName(x, y) == color)
                        count++;
                }
            }
            return count;
        }
    }

    public class IsBrushColor : Expression
    {
        string color;
        public IsBrushColor(string color)
        {
            this.color = color;
        }
        public override int Execute(Wall_E wall_E, GridManager gridManager)
        {
            return wall_E.currentColor == color? 1 : 0;
        }
    }

    public class IsBrushSize : Expression
    {
        int size;
        public IsBrushSize(int size)
        {
            this.size = size;
        }
        public override int Execute(Wall_E wall_E, GridManager gridManager)
        {
            return wall_E.currentBrushSize == size ? 1 : 0;
        }
    }

    public class IsCanvasColor : Expression
    {
        string color;
        int vertical, horizontal;
        public IsCanvasColor(string color, int vertical, int horizontal)
        {
            this.color = color;
            this.vertical = vertical;
            this.horizontal = horizontal;
        }
        public override int Execute(Wall_E wall_E, GridManager gridManager)
        {
            int checkX = wall_E.X + horizontal;
            int checkY = wall_E.Y + vertical;
            if (checkX < 0 || checkY < 0 || checkX >= gridManager.Width || checkY >= gridManager.Height)
                return 0;
            return gridManager.GetPixelColorName(checkX, checkY) == color ? 1 : 0;
        }
        
    }
}