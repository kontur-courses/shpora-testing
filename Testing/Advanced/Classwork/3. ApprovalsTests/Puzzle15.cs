using System.Drawing;

namespace Advanced.Classwork.ApprovalsTests;

public class Puzzle15
{
    private readonly int[,] map = new int[4, 4];
    private Point empty;

    public Puzzle15(int[,] map)
    {
        if (map.GetLength(0) != 4 || map.GetLength(1) != 4)
            throw new ArgumentException("should be 4x4", nameof(map));
        this.map = (int[,])map.Clone();
    }

    public Puzzle15()
    {
        var i = 0;
        empty = new Point(0, 0);
        for (int y = 0; y < 4; y++)
            for (int x = 0; x < 4; x++)
                map[y, x] = i++;
    }

    public int this[Point pos]
    {
        get => map[pos.Y, pos.X];
        set { map[pos.Y, pos.X] = value; }
    }

    public override string ToString() =>
        string.Join("\r\n", Enumerable.Range(0, 4).Select(FormatLine));

    private string FormatLine(int y)
    {
        var cells = Enumerable.Range(0, 4).Select(x => map[y, x].ToString().PadLeft(2));
        return string.Join(" ", cells);
    }

    public void MoveLeft() => Move(-1, 0);
    public void MoveRight() => Move(1, 0);
    public void MoveUp() => Move(0, -1);
    public void MoveDown() => Move(0, 1);

    public void Move(int dx, int dy)
    {
        var newEmpty = empty + new Size(dx, dy);
        if (newEmpty.X >= 0 && newEmpty.X < 4 &&
            newEmpty.Y >= 0 && newEmpty.Y < 4)
        {
            var t = this[empty];
            this[empty] = this[newEmpty];
            this[newEmpty] = t;
            empty = newEmpty;
        }
    }
}
