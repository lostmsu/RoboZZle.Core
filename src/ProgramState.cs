namespace RoboZZle.Core;

public sealed class ProgramState {
    public ProgramState() {
        this.stack.Push(new InstructionPointer());
        // TODO set this value to the actual one
        this.MaxStackSize = 1024;
    }

    readonly Stack<InstructionPointer> stack = new();

    /// <summary>
    /// Maximum size of the program's stack
    /// </summary>
    public int MaxStackSize { get; private set; }
    /// <summary>
    /// Checks if program is in stack overflow state
    /// </summary>
    public bool StackOverflow { get; private set; }
    /// <summary>
    /// Gets instuctions pointer in the stack
    /// </summary>
    public IEnumerable<InstructionPointer> Stack => this.stack.Select(ip => ip.Clone());

    /// <summary>
    /// Position of the next instruction to be executed.
    /// </summary>
    public InstructionPointer? CurrentInstruction
        => this.stack.Count == 0 ? null : this.stack.Peek();

    /// <summary>
    /// Exits the current function, and returns control to the caller.
    /// </summary>
    public void Return() => this.stack.Pop();

    /// <summary>
    /// Enters specified function.
    /// </summary>
    /// <param name="function">Function index to call</param>
    public void Call(int function) {
        if (this.stack.Count >= this.MaxStackSize)
            this.StackOverflow = true;
        else
            this.stack.Push(new InstructionPointer { Function = function });
    }
}