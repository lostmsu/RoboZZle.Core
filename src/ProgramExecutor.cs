namespace RoboZZle.Core;

using System.Diagnostics;

/// <summary>
/// Executes a <see cref="Program"/> against a <see cref="Puzzle"/>.
/// </summary>
public sealed class ProgramExecutor {
    /// <summary>
    /// Creates new instance of <see cref="ProgramExecutor"/>
    /// </summary>
    public ProgramExecutor(Program program, Puzzle puzzle) {
        if (puzzle == null)
            throw new ArgumentNullException(nameof(puzzle));
        this.program = program ?? throw new ArgumentNullException(nameof(program));
        this.PuzzleState = puzzle.InitialState.Clone();
    }

    /// <summary>
    /// Creates new instance of <see cref="ProgramExecutor"/>
    /// </summary>
    public ProgramExecutor(Program program, PuzzleState puzzleState) {
        this.program = program ?? throw new ArgumentNullException(nameof(program));
        this.PuzzleState = puzzleState ?? throw new ArgumentNullException(nameof(puzzleState));
    }

    /// <summary>
    /// Maximum number of steps robot can take to collect all stars
    /// </summary>
    public const int DEFAULT_MAX_MOVES = 1000;

    /// <summary>
    /// Occurs, when program state is changed.
    /// </summary>
    public event EventHandler? ProgramStateChanged;
    /// <summary>
    /// Occurs, when robot position or direction is changed.
    /// </summary>
    public event EventHandler<EventArgs<MovementKind>>? RobotPositionChanged;
    /// <summary>
    /// Occurs, when anything happens on some cell.
    /// </summary>
    public event EventHandler<PositionedEventArgs>? CellChanged;
    /// <summary>
    /// State of the program execution
    /// </summary>
    public ProgramState ProgramState { get; } = new();
    /// <summary>
    /// State of the puzzle field
    /// </summary>
    public PuzzleState PuzzleState { get; }
    /// <summary>
    /// Maximum allowed number of robot steps
    /// </summary>
    public int MaxMoves { get; set; } = DEFAULT_MAX_MOVES;

    /// <summary>
    /// Number of program steps executed.
    /// </summary>
    public int Steps { get; private set; }
    /// <summary>
    /// Number of robot moves.
    /// </summary>
    public int Moves { get; private set; }

    /// <summary>
    /// Performs single program step. Puzzle must not be failed yet, otherwise behavior is undefined.
    /// </summary>
    public void Step() {
        var ip = this.ProgramState.CurrentInstruction;
        if (ip == null)
            return;

        var function = this.program.Functions[ip.Function]!;

        if (ip.Command >= function.Commands.Length) {
            this.Return();
            return;
        }

        this.Perform(function.Commands[ip.Command]);
    }

    /// <summary>
    /// Checks, if program successed
    /// </summary>
    /// <returns>True, if all stars were collected</returns>
    public bool IsVictory() {
        return this.PuzzleState.IsVictory();
    }

    /// <summary>
    /// Checks, if program terminated
    /// </summary>
    /// <returns>True, if program is terminated</returns>
    public bool IsTerminated() {
        return this.PuzzleState.IsVictory() || this.PuzzleState.IsFail()
                                            || this.Moves >= this.MaxMoves ||
                                               this.ProgramState.StackOverflow
                                            || this.ProgramState.CurrentInstruction == null;
    }

    void Perform(Command? command) {
        if (command == null) {
            this.NoOp();
            // automatically execute next command
            this.Step();
            return;
        }

        this.Steps++;

        if (!command.Condition.IsMet(this.PuzzleState)) {
            this.NoOp();
            return;
        }

        this.Perform(command.Action);
    }

    readonly Program program;

    void Perform(Action action) {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        bool performed = false;
        performed |= this.Move(action as Movement);
        performed |= this.Paint(action as Paint);
        performed |= this.Call(action as Call);

        if (!performed)
            throw new NotSupportedException("Action type is not supported: " + action.GetType());

        this.OnProgramStateChanged();
    }

    bool Move(Movement? moveCommand) {
        if (moveCommand == null)
            return false;

        moveCommand.Move(this.PuzzleState.Robot);

        this.OnRobotPositionChanged(moveCommand.Kind);

        this.EatStar(this.PuzzleState.Robot);

        this.ProgramState.CurrentInstruction.Command++;
        this.Moves++;
        return true;
    }

    void EatStar(Robot robot) {
        var newCell = this.PuzzleState.GetCell(robot.X, robot.Y);
        if (newCell != null && newCell.Star) {
            this.PuzzleState.CollectStar(robot.X, robot.Y);
            this.OnCellChanged(robot.X, robot.Y);
        }
    }

    bool Paint(Paint? paintCommand) {
        if (paintCommand == null)
            return false;

        var currentCell =
            this.PuzzleState.Cells[this.PuzzleState.Robot.X][this.PuzzleState.Robot.Y];
        if (currentCell.Color == null)
            throw new InvalidOperationException("Can't paint: robot is not on any cell");

        currentCell.Color = paintCommand.Color;

        this.OnCellChanged(this.PuzzleState.Robot.X, this.PuzzleState.Robot.Y);

        this.ProgramState.CurrentInstruction.Command++;
        this.Moves++;
        return true;
    }

    bool Call(Call? callCommand) {
        if (callCommand == null)
            return false;

        // verify command sanity
        Debug.Assert(callCommand.Function < this.program.Functions.Length,
                     "Can't call non-existent function F" + callCommand.Function);

        var function = this.program.Functions[callCommand.Function];

        this.ProgramState.CurrentInstruction.Command++;

        bool hasFunction =
            function != null
            // ignore empty functions
         && function.Commands.Any(command => command != null);

        if (hasFunction)
            this.ProgramState.Call(callCommand.Function);

        return true;
    }

    void Return() {
        this.ProgramState.Return();
        this.OnProgramStateChanged();
        this.Step();
    }

    void NoOp() {
        this.ProgramState.CurrentInstruction.Command++;
        this.OnProgramStateChanged();
    }

    void OnProgramStateChanged() {
        this.ProgramStateChanged?.Invoke(this, EventArgs.Empty);
    }

    void OnRobotPositionChanged(MovementKind movement) {
        this.RobotPositionChanged?.Invoke(this, movement.ToEventArgs());
    }

    void OnCellChanged(int x, int y) {
        this.CellChanged?.Invoke(this, new PositionedEventArgs { X = x, Y = y });
    }
}