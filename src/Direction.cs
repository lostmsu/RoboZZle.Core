namespace RoboZZle.Core;

using System.Diagnostics.Contracts;

public struct Direction: IEquatable<Direction> {
    readonly int value;

    public Direction(int value) {
        this.value = value;
    }

    public static readonly Direction Right = new(0);
    public static readonly Direction Down = new(1);
    public static readonly Direction Left = new(2);
    public static readonly Direction Top = new(3);

    [Pure]
    public Direction TurnLeft() => new(this.value - 1);

    [Pure]
    public Direction TurnRight() => new(this.value + 1);

    [Pure]
    public Direction Normalize() => new(this.value & 3);

    public static explicit operator int(Direction direction) => direction.value;

    /// <summary>
    /// Finds number of _right_ turns, needed to convert the second direction into the first direction.
    /// </summary>
    public static int operator -(Direction a, Direction b) => a.value - b.value;

    static readonly Dictionary<Direction, Tuple<int, int>> Deltas =
        new() {
            { Down, new Tuple<int, int>(0, 1) },
            { Right, new Tuple<int, int>(1, 0) },
            { Top, new Tuple<int, int>(0, -1) },
            { Left, new Tuple<int, int>(-1, 0) },
        };

    /// <summary>
    /// Returns vector of length 1 with the current direction.
    /// </summary>
    public Tuple<int, int> ToVector() => Deltas[this.Normalize()];

    /// <summary>
    /// Checks if two directions are equal
    /// </summary>
    public static bool operator ==(Direction a, Direction b) {
        a = a.Normalize();
        b = b.Normalize();
        return a.value == b.value;
    }

    /// <summary>
    /// Checks if two directions are not equal
    /// </summary>
    public static bool operator !=(Direction a, Direction b) => !(a == b);

    public bool Equals(Direction other) => this == other;
    public override bool Equals(object? obj) => obj is Direction other && this.Equals(other);
    public override int GetHashCode() => this.Normalize().value;
}