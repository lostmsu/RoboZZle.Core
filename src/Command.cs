﻿namespace RoboZZle.Core;

using System.Collections;

public sealed class Command: IStructuralEquatable {
    /// <summary>
    /// String representation of an empty command
    /// </summary>
    public const string EMPTY_STRING = "__";

    public Color? Condition { get; init; }
    public Action? Action { get; init; }

    public Command With(Action? action) => new() { Action = action, Condition = this.Condition };
    public Command With(Color? condition) => new() { Action = this.Action, Condition = condition };

    /// <summary>
    /// Clones command
    /// </summary>
    /// <param name="original">Command to clone</param>
    public static Command? Clone(Command? original) {
        return original == null
            ? null
            : new Command { Action = original.Action, Condition = original.Condition };
    }

    /// <summary>
    /// Parses <see cref="Command"/> at the specified position in a string.
    /// </summary>
    public static Command? Parse(string programText, int position) {
        var action = Action.Parse(programText[position + 1]);
        if (action == null)
            return null;

        var condition = ParseCondition(programText[position]);
        return new Command { Action = action, Condition = condition };
    }

    /// <summary>
    /// Parses <see cref="Command"/> at the specified position in a string.
    /// </summary>
    public static Command? Parse(char action, char condition) {
        var parsedAction = Action.Parse(action);
        if (parsedAction == null)
            return null;

        var parsedCondition = ParseCondition(condition);
        return new Command { Action = parsedAction, Condition = parsedCondition };
    }

    static readonly Dictionary<char, Color?> Conditions =
        new() {
            { '_', null },
            { 'r', Color.RED },
            { 'g', Color.GREEN },
            { 'b', Color.BLUE },
        };

    static Color? ParseCondition(char condition) => Conditions[condition];

    char ActionChar() => this.Action?.ToChar() ?? '·';

    /// <summary>
    /// Converts command to its string representation.
    /// </summary>
    public override string ToString() => "" + this.Condition.ToChar() + this.ActionChar();

    public bool Equals(object? other, IEqualityComparer comparer) {
        if (comparer == null)
            throw new ArgumentNullException(nameof(comparer));

        if (other == null && this.Action == null)
            return true;

        if (other is not Command otherCommand)
            return false;

        return comparer.Equals(otherCommand.Action, this.Action)
            && comparer.Equals(otherCommand.Condition, this.Condition);
    }

    public int GetHashCode(IEqualityComparer comparer) {
        if (comparer == null)
            throw new ArgumentNullException(nameof(comparer));

        return comparer.GetHashCode(this.Condition.ToChar()) * 117 +
               comparer.GetHashCode(this.ActionChar());
    }
}