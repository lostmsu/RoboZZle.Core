namespace RoboZZle.Core;

/// <summary>
/// Represents state of the puzzle field
/// </summary>
public sealed class PuzzleState {
    /// <summary>
    /// Creates a new instance of puzzle state
    /// </summary>
    public PuzzleState() {
        this.Robot = new Robot();
        this.Cells = new PuzzleCell[Puzzle.WIDTH][].Rect(Puzzle.HEIGHT);
    }

    /// <summary>
    /// Represents robot location
    /// </summary>
    public Robot Robot { get; set; }

    /// <summary>
    /// Represents puzzle cell states
    /// </summary>
    public PuzzleCell[][] Cells { get; set; }

    /// <summary>
    /// Gets cell at specified location.
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <returns>Cell, or null if coordinates are out of bounds.</returns>
    public PuzzleCell? GetCell(int x, int y) {
        if (x < 0 || y < 0 || x >= Puzzle.WIDTH || y >= Puzzle.HEIGHT)
            return null;

        return this.Cells[x][y];
    }

    /// <summary>
    /// Collect star at specified location.
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    public void CollectStar(int x, int y) {
        if (x is < 0 or >= Puzzle.WIDTH)
            throw new ArgumentOutOfRangeException(nameof(x));
        if (y is < 0 or >= Puzzle.HEIGHT)
            throw new ArgumentOutOfRangeException(nameof(y));

        if (!this.Cells[x][y].Star)
            throw new InvalidOperationException();

        this.Cells[x][y].Star = false;
    }

    /// <summary>
    /// Returns true, if current state means victory
    /// </summary>
    public bool IsVictory() {
        return this.Cells.Flatten().All(cell => !cell.Star);
    }

    /// <summary>
    /// Returns true, if current state means failure
    /// </summary>
    public bool IsFail() {
        bool isOutOfFieldBounds =
            !this.Robot.X.Between(0, Puzzle.WIDTH - 1)
         || !this.Robot.Y.Between(0, Puzzle.HEIGHT - 1);
        if (isOutOfFieldBounds)
            return true;

        bool isOutOfColoredCells =
            !this.Cells[this.Robot.X][this.Robot.Y].Color.HasValue;
        return isOutOfColoredCells;
    }

    /// <summary>
    /// Creates a structural copy of the puzzle state.
    /// </summary>
    /// <returns>A copy of the current instance.</returns>
    public PuzzleState Clone() {
        var result = new PuzzleState { Robot = this.Robot.Clone() };

        // copy cells
        for (int x = 0; x < Puzzle.WIDTH; x++)
        for (int y = 0; y < Puzzle.HEIGHT; y++) {
            result.Cells[x][y] = this.Cells[x][y].Clone();
        }

        return result;
    }

    public int Width => this.Cells.Length;
    public int Height => this.Cells[0].Length;
}