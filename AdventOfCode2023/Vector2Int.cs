using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2023;

public struct Vector2Int(int x, int y) : IEquatable<Vector2Int>
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public Vector2Int() : this(0, 0) { }

    public static Vector2Int operator -(Vector2Int v) => new(-v.X, -v.Y);
    public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new(a.X + b.X, a.Y + b.Y);

    public readonly override int GetHashCode() => (X, Y).GetHashCode();

    public readonly bool Equals(Vector2Int v) => X == v.X && Y == v.Y;
    public static bool operator ==(Vector2Int lhs, Vector2Int rhs) => lhs.Equals(rhs);
    public static bool operator !=(Vector2Int lhs, Vector2Int rhs) => !(lhs == rhs);
    public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Vector2Int v && Equals(v);

    public static readonly Vector2Int Zero = new(0, 0);
    public static readonly Vector2Int North = new(0, -1);
    public static readonly Vector2Int South = new(0, 1);
    public static readonly Vector2Int East = new(1, 0);
    public static readonly Vector2Int West = new(-1, 0);

    public static IEnumerable<Vector2Int> Directions
    {
        get
        {
            yield return North;
            yield return South;
            yield return East;
            yield return West;
        }
    }
}
