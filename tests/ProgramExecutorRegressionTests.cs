namespace RoboZZle.Core;

[TestClass]
public class ProgramExecutorRegressionTests {
    [TestMethod]
    public void StraightOnRed() {
        var puzzle = TestPuzzles.MakeBlue([4]);
        puzzle.InitialState.Cells[0][0].Star = true;
        var program = new Program(puzzle);
        Command value = new Command {
            Action = new Movement(),
            Condition = Color.RED,
        };
        program.Functions[0]!.Commands.AsSpan().Fill(value);
        var executor = new ProgramExecutor(program, puzzle);
        while (!executor.IsTerminated()) {
            executor.Step();
            Assert.IsTrue(executor.ProgramState.CurrentInstruction.ToGlobal(program) <=
                          program.TotalSlots);
        }
    }

    [TestMethod]
    public void FunWithPaintMustPaint() {
        var puzzle = TestPuzzles.MakeBlue([4]);
        var program = new Program(puzzle);
        var f0 = program.Functions[0]!;
        f0.Commands[0] = new Command { Action = new Paint { Color = Color.RED } };
        var executor = new ProgramExecutor(program, puzzle);
        executor.Step();
        var state = executor.PuzzleState;
        var robot = state.Robot;
        Assert.IsTrue(executor.PuzzleState.Cells[robot.X][robot.Y].Color == Color.RED);
    }
}