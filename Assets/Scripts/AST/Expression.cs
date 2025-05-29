    using System;

namespace WallE_Class
{
    public abstract class Expression
    {
        public virtual int Execute()
        {
            return int.MaxValue;
        }
    }

    public class GetActualX : Expression
    {
        public override int Execute()
        {
            throw new NotImplementedException();
        }
    }

    public class GetActualY : Expression
    {
        public override int Execute()
        {
            throw new NotImplementedException();
        }
    }

    public class GetCanvasSize : Expression
    {
        public override int Execute()
        {
            throw new NotImplementedException();
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
        public override int Execute()
        {
            throw new NotImplementedException();
        }
    }

    public class IsBrushColor : Expression
    {
        string color;
        public IsBrushColor(string color)
        {
            this.color = color;
        }
        public override int Execute()
        {
            throw new NotImplementedException();
        }
    }

    public class IsBrushSize : Expression
    {
        string size;
        public IsBrushSize(string size)
        {
            this.size = size;
        }
        public override int Execute()
        {
            throw new NotImplementedException();
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
        public override int Execute()
        {
            throw new NotImplementedException();
        }
    }
}