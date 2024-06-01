namespace RoboZZle.Core;

/// <summary>
/// Generic <see cref="EventArgs"/> descendant, providing single argument to pass
/// </summary>
/// <typeparam name="T">Type of the event argument</typeparam>
public sealed class EventArgs<T>: EventArgs {
    /// <summary>
    /// Creates new instance of this class
    /// </summary>
    public EventArgs() { }

    /// <summary>
    /// Creates new instance of this class
    /// </summary>
    /// <param name="argumentValue">Value of the event argument</param>
    public EventArgs(T argumentValue) {
        this.Argument = argumentValue;
    }

    /// <summary>
    /// Contains event argument
    /// </summary>
    public T Argument { get; set; }
}