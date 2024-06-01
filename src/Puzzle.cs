namespace RoboZZle.Core;

public sealed class Puzzle {
    public const int WIDTH = 16;
    public const int HEIGHT = 12;
    public const int MAX_FUNCTIONS = 5;
    public const int MAX_COMMANDS_PER_FUNCTION = 10;

    public int ID { get; set; }

    public string Title { get; set; }
    public string About { get; set; }
    public CommandSet CommandSet { get; set; }
    public int[] SubLengths { get; set; }
    /// <summary>
    /// Range: 0 - 100
    /// </summary>
    public int? Difficulty { get; set; }
    public int DifficultyVoteCount { get; set; }
    public bool Featured { get; set; }
    public int Liked { get; set; }
    public int Disliked { get; set; }
    public string Author { get; set; }
    public DateTime SubmittedDate { get; set; }

    public PuzzleState InitialState { get; set; }

    public int ActionLimit { get; set; } = 1000;

    /// <summary>
    /// Checks if specified color can appear in this puzzle's state
    /// </summary>
    /// <param name="color">Color</param>
    /// <returns>true, if color can appear in this puzzle's state</returns>
    public bool IsAvailable(Color color) {
        var paintCommand = color.PaintCommand();
        if ((this.CommandSet & paintCommand) == paintCommand)
            return true;

        return this.InitialState.Cells.Flatten().Any(c => color.Equals(c.Color));
    }

    /// <summary>
    /// List of all colors, which can appear in this puzzle's state,
    /// including null
    /// </summary>
    public IEnumerable<Color?> AvailableConditions {
        get {
            var result = new List<Color?> { null };
            result.AddRange(Colors.All.Where(this.IsAvailable).Select(c => (Color?)c));
            return result;
        }
    }
}