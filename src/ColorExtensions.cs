namespace RoboZZle.Core;

public static class ColorExtensions {
    /// <summary>
    /// Checks if puzzle state meets condition.
    /// </summary>
    /// <param name="condition">Condtion to check</param>
    /// <param name="state">Puzzle state to check</param>
    /// <returns>True, if condition is met, otherwise false.</returns>
    public static bool IsMet(this Color? condition, PuzzleState state) {
        var cellColor = state.Cells[state.Robot.X][state.Robot.Y].Color;

        if (cellColor == null)
            throw new InvalidOperationException("Can't check command condition: robot failed.");

        if (condition == null)
            return true;

        return condition.Value == cellColor.Value;
    }
}