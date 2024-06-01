namespace RoboZZle.Core;

/// <summary>
/// Represents possible robot action
/// </summary>
public abstract class Action {
    static readonly Dictionary<char, Action?> Actions =
        new() {
            { 'F', new Movement { Kind = MovementKind.MOVE } },
            { 'R', new Movement { Kind = MovementKind.TURN_RIGHT } },
            { 'L', new Movement { Kind = MovementKind.TURN_LEFT } },
            { 'r', new Paint { Color = Color.RED } },
            { 'g', new Paint { Color = Color.GREEN } },
            { 'b', new Paint { Color = Color.BLUE } },
            { '_', null },
            { '\0', null },
        };

    static Action() {
        for (int i = 0; i < 10; i++) {
            Actions[(char)(i + '1')] = new Call { Function = i };
        }
    }

    /// <summary>
    /// Parses <see cref="Action"/> from its string representation
    /// </summary>
    public static Action? Parse(char actionSymbol) => Actions[actionSymbol];

    /// <summary>
    /// Converts this action to its string representation
    /// </summary>
    public abstract char ToChar();

    /// <summary>
    /// Compares two actions for equality. Comparison is structural.
    /// </summary>
    public static bool operator ==(Action? a, Action? b) {
        if (ReferenceEquals(a, b))
            return true;

        if (ReferenceEquals(a, null))
            return false;

        return a.Equals(b);
    }

    /// <summary>
    /// Compares two actions for inequality. Comparison is structural.
    /// </summary>
    public static bool operator !=(Action? a, Action? b) => !(a == b);

    /// <summary>
    /// Checks if this action structurally equals to supplied object.
    /// </summary>
    public override bool Equals(object? obj) {
        throw new InvalidOperationException(
            $"Class {this.GetType().FullName} must override Equals");
    }

    /// <summary>
    /// Gets action's hash code.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() {
        throw new InvalidOperationException(
            $"Class {this.GetType().FullName} must override GetHashCode");
    }

    /// <summary>
    /// Gets all possible <see cref="Action">actions</see>
    /// </summary>
    public static Action[] GetAll() => Movement.GetAll()
                                               .Concat<Action>(Paint.GetAll())
                                               .Concat(Call.GetAll())
                                               .ToArray();
}