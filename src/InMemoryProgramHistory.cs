namespace RoboZZle.Core;

using System.Collections;

/// <summary>
/// Represents in-memory program edit history
/// </summary>
public sealed class InMemoryProgramHistory: IProgramHistory {
    readonly List<Program> history;

    /// <summary>
    /// Creates new instance of <see cref="InMemoryProgramHistory"/>
    /// with single program version.
    /// </summary>
    /// <param name="startingProgram"></param>
    public InMemoryProgramHistory(Program startingProgram) {
        if (startingProgram == null)
            throw new ArgumentNullException(nameof(startingProgram));

        this.history = new List<Program>(new[] { startingProgram });
    }

    /// <summary>
    /// Returns current version of a program
    /// </summary>
    public Program CurrentProgram => this.history[this.CurrentVersion].Clone();
    /// <summary>
    /// Gets current program version number
    /// </summary>
    public int CurrentVersion { get; private set; }
    /// <summary>
    /// Gets the latest version of the program, which is available in the history
    /// </summary>
    public int LatestVersion => this.history.Count - 1;

    /// <summary>
    /// Adds program to history, and increments current version.
    /// If history had versions after current, they are discarded.
    /// </summary>
    public void Add(Program version) {
        this.Trim();
        this.history.Add(version.Clone());
        this.CurrentVersion++;
    }

    /// <summary>
    /// Rewinds history forward after doing <see cref="Undo"/>
    /// </summary>
    public Program Redo() {
        if (this.CurrentVersion == this.LatestVersion)
            throw new InvalidOperationException();

        this.CurrentVersion++;
        return this.CurrentProgram;
    }

    /// <summary>
    /// Rewinds last editing action.
    /// Does not remove current version from history to be accessible in <see cref="Redo"/>.
    /// </summary>
    /// <returns></returns>
    public Program Undo() {
        if (this.CurrentVersion == 0)
            throw new InvalidOperationException();

        this.CurrentVersion--;
        return this.CurrentProgram;
    }

    /// <summary>
    /// Not supported.
    /// </summary>
    public void Save() => throw new NotSupportedException();

    #region Enumerable

    /// <summary>
    /// Gets enumerator for this history from its start til the current version
    /// </summary>
    public IEnumerator<Program> GetEnumerator() {
        for (int version = 0; version <= this.CurrentVersion; version++)
            yield return this.history[version];
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    #endregion

    private void Trim() {
        if (this.CurrentVersion < this.LatestVersion) {
            this.history.RemoveRange(this.CurrentVersion + 1,
                                     this.LatestVersion - this.CurrentVersion);
        }
    }
}