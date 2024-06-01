namespace RoboZZle.Core;

/// <summary>
/// Action, that describes robot movement
/// </summary>
public sealed class Movement: Action {
    readonly MovementKind kind;
    public MovementKind Kind {
        get => this.kind;
        init {
            if (!Chars.ContainsKey(value))
                throw new ArgumentException("Invalid movement kind: " + value, nameof(value));

            this.kind = value;
        }
    }

    public override bool Equals(object? obj) {
        var move = obj as Movement;

        if (move == null)
            return false;

        return move.Kind == this.Kind;
    }

    public override int GetHashCode() {
        return this.Kind.GetHashCode();
    }

    static readonly Dictionary<MovementKind, char> Chars =
        new() {
            { MovementKind.MOVE, 'F' },
            { MovementKind.TURN_RIGHT, 'R' },
            { MovementKind.TURN_LEFT, 'L' },
        };

    public override char ToChar() {
        return Chars[this.Kind];
    }

    public void Move(Robot robot) {
        switch (this.Kind) {
        case MovementKind.MOVE:
            var delta = robot.Direction.ToVector();
            robot.X += delta.Item1;
            robot.Y += delta.Item2;
            break;
        case MovementKind.TURN_LEFT:
            robot.Direction = robot.Direction.TurnLeft();
            break;
        case MovementKind.TURN_RIGHT:
            robot.Direction = robot.Direction.TurnRight();
            break;
        }
    }

    /// <summary>
    /// Gets all possible movement <see cref="Action">actions</see>
    /// </summary>
    public new static Movement[] GetAll() => Chars.Keys
                                                  .Select(movementKind => new Movement
                                                              { Kind = movementKind }).ToArray();
}