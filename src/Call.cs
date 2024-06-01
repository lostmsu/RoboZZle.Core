namespace RoboZZle.Core;

public sealed class Call: Action {
    readonly int function;

    public int Function {
        get => this.function;
        init {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Function must be >= 0");

            this.function = value;
        }
    }

    public override bool Equals(object? obj) {
        var call = obj as Call;

        if (call == null)
            return false;

        return this.Function == call.Function;
    }

    public override int GetHashCode() {
        return this.Function.GetHashCode();
    }

    public override char ToChar() {
        return (char)(this.function + '1');
    }

    /// <summary>
    /// Gets all possible call <see cref="Action">actions</see>
    /// </summary>
    public new static Call[] GetAll() =>
        Enumerable.Range(0, 5).Select(f => new Call { Function = f }).ToArray();
}