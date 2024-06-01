namespace RoboZZle.Core;

/// <summary>
/// Represents status of puzzle cell
/// </summary>
public sealed class PuzzleCell {
    /// <summary>
    /// Current cell color
    /// </summary>
    public Color? Color { get; set; }
    /// <summary>
    /// True, if cell has a star
    /// </summary>
    public bool Star { get; set; }

    /// <summary>
    /// Creates a structural copy of the cell.
    /// </summary>
    /// <returns>A copy of the cell.</returns>
    public PuzzleCell Clone() => new() { Color = this.Color, Star = this.Star };
}