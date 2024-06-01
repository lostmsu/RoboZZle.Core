namespace RoboZZle.Core;

public sealed class Function {
    internal Function(int commandCount) {
        if (commandCount is < 0 or > MAX_COMMANDS)
            throw new ArgumentOutOfRangeException(nameof(commandCount));

        this.Commands = new Command[commandCount];
    }

    Function(Command?[] commands) {
        if (commands == null)
            throw new ArgumentNullException(nameof(commands));
        if (commands.Length > MAX_COMMANDS)
            throw new ArgumentOutOfRangeException(nameof(commands));

        this.Commands = commands;
    }

    const int MAX_COMMANDS = 10;

    public Command?[] Commands { get; }

    public int Length => this.Commands.Length;

    /// <summary>
    /// Checks if function is empty
    /// </summary>
    public bool IsEmpty() {
        return this.Commands.All(command => command == null);
    }

    internal void Parse(string programText, ref int position) {
        for (int cmdIdx = 0; cmdIdx < this.Commands.Length; cmdIdx++) {
            if (position >= programText.Length)
                return;

            if (programText[position] == Program.FUNCTION_SEPARATOR) {
                position++;
                break;
            }

            this.Commands[cmdIdx] = Command.Parse(programText, position);
            position += 2;
        }

        if (position < programText.Length && programText[position] == Program.FUNCTION_SEPARATOR) {
            position++;
        }
    }

    /// <summary>
    /// Parses next function in program text. Ignores puzzle defined function sizes.
    /// </summary>
    /// <param name="programText">Program text</param>
    /// <param name="position">Position in program text, where function to be parsed starts</param>
    public static Function? ParseNew(string programText, ref int position) {
        if (position < 0)
            throw new IndexOutOfRangeException();

        var commands = new List<Command?>();
        for (; position < programText.Length;) {
            if (programText[position] == Program.FUNCTION_SEPARATOR) {
                position++;
                return new Function(commands.ToArray());
            }

            var command = Command.Parse(programText, position);
            commands.Add(command);
            position += 2;
        }

        return commands.Count > 0
            ? new Function(commands.ToArray())
            : null;
    }

    /// <summary>
    /// Makes a copy of current function
    /// </summary>
    public Function Clone() => new(this.Commands.Select(Command.Clone).ToArray());

    /// <summary>
    /// Creates new <see cref="Function"/> of the same size
    /// </summary>
    public Function Empty() => new(this.Commands.Length);
}