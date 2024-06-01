namespace RoboZZle.Core;

using System.Text;

public sealed class Program {
    Program(int functionCount) {
        if (functionCount is <= 0 or > MAX_FUNCTIONS)
            throw new ArgumentOutOfRangeException(nameof(functionCount));

        this.Functions = new Function[functionCount];
    }

    Program(Function?[] functions) {
        this.Functions = functions ?? throw new ArgumentNullException(nameof(functions));
    }

    /// <summary>
    /// Creates empty program for the specified puzzle
    /// </summary>
    /// <param name="puzzle"><see cref="Puzzle"/> to create program for</param>
    public Program(Puzzle puzzle): this(puzzle.SubLengths.Length) {
        for (int i = 0; i < puzzle.SubLengths.Length; i++) {
            this.Functions[i] = new Function(puzzle.SubLengths[i]);
        }
    }

    public const int MAX_FUNCTIONS = 5;

    public Function?[] Functions { get; private set; }

    /// <summary>
    /// Returns length of the program (count of non-empty commands)
    /// </summary>
    public int GetLength() {
        return this.Functions.Sum(fun => fun.Commands.Count(c => c != null));
    }

    #region Serialization

    /// <summary>
    /// Encodes program into its string representation.
    /// </summary>
    /// <param name="collapseEmptyCommands">If true, empty commands cells won't be encoded.</param>
    /// <returns>String representation of the program.</returns>
    public string Encode(bool collapseEmptyCommands) {
        var result = new StringBuilder();

        foreach (var function in this.Functions) {
            foreach (var command in function.Commands) {
                if (command == null) {
                    if (!collapseEmptyCommands)
                        result.Append("__");
                } else {
                    result.Append(command.Condition.ToChar());
                    result.Append(command.Action.ToChar());
                }
            }

            result.Append(FUNCTION_SEPARATOR);
        }

        return result.ToString();
    }

    /// <summary>
    /// Parses program from its string representation.
    /// </summary>
    /// <param name="programText">Program text.</param>
    /// <param name="puzzle">Puzzle, the program is for.</param>
    /// <returns>Program object model</returns>
    public static Program Parse(string programText, Puzzle puzzle) {
        var program = new Program(puzzle);
        int textPos = 0;
        foreach (var function in program.Functions) {
            function.Parse(programText, ref textPos);
            if (textPos >= programText.Length)
                break;
        }

        return program;
    }

    /// <summary>
    /// Parses program from its text representation, ignoring original puzzle's program structure
    /// </summary>
    /// <param name="programText">Program text</param>
    public static Program Parse(string programText) {
        var functions = new List<Function?>();
        for (int textPos = 0; textPos < programText.Length;) {
            var function = Function.ParseNew(programText, ref textPos);
            functions.Add(function);
        }

        return new Program(functions.ToArray());
    }

    internal const char FUNCTION_SEPARATOR = '|';

    #endregion

    /// <summary>
    /// Clones program for specified puzzle
    /// </summary>
    /// <returns>Identical clone of the program</returns>
    public Program Clone() => new(this.Functions.Select(f => f.Clone()).ToArray());

    /// <summary>
    /// Creates an emtpy program of the same size
    /// </summary>
    public Program Empty() => new(this.Functions.Select(f => f.Empty()).ToArray());

    /// <summary>
    /// Total number of command slots in this program
    /// </summary>
    public int TotalSlots => this.Functions.Sum(f => f.Commands.Length);
}