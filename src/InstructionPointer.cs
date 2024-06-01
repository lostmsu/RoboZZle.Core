namespace RoboZZle.Core;

public sealed class InstructionPointer {
    public int Function { get; set; }
    public int Command { get; set; }

    /// <summary>
    /// Returns <see cref="InstructionPointer"/> from its global index in program
    /// </summary>
    /// <param name="program"><see cref="Program"/> to calculate <see cref="InstructionPointer"/> for</param>
    /// <param name="globalIndex">Global index of instruction to calculate <see cref="InstructionPointer"/> for</param>
    public static InstructionPointer? FromGlobal(Program program, int globalIndex) {
        if (globalIndex < 0)
            return null;

        for (int fun = 0; fun < program.Functions.Length; fun++) {
            var function = program.Functions[fun];
            if (globalIndex >= function.Commands.Length)
                globalIndex -= function.Commands.Length;
            else
                return new InstructionPointer { Function = fun, Command = globalIndex };
        }

        return null;
    }

    /// <summary>
    /// Creates a clone of this instance
    /// </summary>
    public InstructionPointer Clone() {
        return new InstructionPointer {
            Command = this.Command,
            Function = this.Function,
        };
    }

    /// <summary>
    /// Gets the pointer to the command immediately following command under this pointer.
    /// Does not skip <c>null</c> commands
    /// </summary>
    /// <param name="program"><see cref="Program"/> to calculate next command for</param>
    public InstructionPointer? Next(Program program) {
        var result = this.Clone();
        if (this.Command + 1 >= program.Functions[this.Function].Commands.Length) {
            if (this.Function + 1 >= program.Functions.Length)
                return null;

            result.Function++;
            result.Command = 0;
        } else
            result.Command++;

        return result;
    }

    public override int GetHashCode() =>
        this.Function.GetHashCode() + this.Command.GetHashCode() * 757;

    public override bool Equals(object? obj) {
        var other = obj as InstructionPointer;
        if (other == null)
            return false;
        return other.Command == this.Command && other.Function == this.Function;
    }

    public override string ToString() => $"{this.Function}:{this.Command}";
}