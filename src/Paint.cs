namespace RoboZZle.Core;

public sealed class Paint: Action {
    public Color Color { get; init; }

    public override bool Equals(object? obj) {
        var paint = obj as Paint;

        if (paint == null)
            return false;

        return this.Color == paint.Color;
    }

    public override int GetHashCode() => this.Color.GetHashCode();

    public override char ToChar() => this.Color.ToChar();

    /// <summary>
    /// Gets all possible paint <see cref="Action">actions</see>
    /// </summary>
    public new static Paint[] GetAll()
        => new[] { Color.RED, Color.BLUE, Color.GREEN, }
           .Select(color => new Paint { Color = color }).ToArray();
}