namespace RoboZZle.Core;

static class TestPuzzles {
    static readonly PuzzleCell[] BlueColumn =
        Enumerable.Repeat(new PuzzleCell { Color = Color.BLUE }, Puzzle.HEIGHT).ToArray();

    public static Puzzle MakeBlue(int[] subLengths) {
        var puzzle = new Puzzle {
            Title = "Blue",
            ID = 43,
            CommandSet = CommandSet.PAINT_ALL,
            InitialState = new PuzzleState {
                Robot = new Robot
                    { Direction = Direction.Top, X = Puzzle.WIDTH / 2, Y = Puzzle.HEIGHT / 2 }
            },
            SubLengths = subLengths,
        };
        puzzle.InitialState.Cells = Enumerable.Repeat(0, Puzzle.WIDTH)
                                              .Select(_ => (PuzzleCell[])BlueColumn.Clone())
                                              .ToArray();

        return puzzle;
    }
}