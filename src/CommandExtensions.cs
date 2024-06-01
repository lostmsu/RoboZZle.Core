namespace RoboZZle.Core;

/// <summary>
/// Provides extensions for <see cref="Command"/>.
/// </summary>
public static class CommandExtensions {
    /// <summary>
    /// Converts command to its string representation.
    /// For <c>null</c>: returns <see cref="Command.EMPTY_STRING"/>
    /// </summary>
    public static string AsString(this Command? command) =>
        command?.ToString() ?? Command.EMPTY_STRING;

    /// <summary>
    /// Clones this command. Supports <c>null</c>-valued commands
    /// </summary>
    public static Command? Clone(this Command? original) =>
        original == null
            ? null
            : new Command { Action = original.Action, Condition = original.Condition };
}