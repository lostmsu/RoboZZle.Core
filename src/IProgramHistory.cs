namespace RoboZZle.Core;

/// <summary>
/// Represents puzzle program version history
/// </summary>
public interface IProgramHistory: IEnumerable<Program> {
    /// <summary>
    /// Index of current program's version
    /// </summary>
    int CurrentVersion { get; }

    /// <summary>
    /// Index of the latest program's version
    /// </summary>
    int LatestVersion { get; }

    /// <summary>
    /// Retrieves current program
    /// </summary>
    Program CurrentProgram { get; }

    /// <summary>
    /// Adds specified program version to version history.
    /// Any other version after the current one are discarded. (You can't Redo after Add)
    /// </summary>
    /// <param name="version">Program version to add to editing history</param>
    void Add(Program version);

    /// <summary>
    /// Undoes last edit operation, returning previous program version.
    /// </summary>
    /// <returns>Previous program version.</returns>
    Program Undo();

    /// <summary>
    /// Redoes the last Undo operation in LIFO order.
    /// </summary>
    /// <returns>Restored program version.</returns>
    Program Redo();

    /// <summary>
    /// Overwrites history file with current instance data
    /// </summary>
    void Save();
}