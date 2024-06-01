namespace RoboZZle.Core;

/// <summary>
/// Supported robot movements
/// </summary>
public enum MovementKind {
    /// <summary>
    /// Move forward one cell
    /// </summary>
    MOVE,

    /// <summary>
    /// Turn 90 degrees to the left (CCW)
    /// </summary>
    TURN_LEFT,

    /// <summary>
    /// Turn 90 degrees to the right (CW)
    /// </summary>
    TURN_RIGHT,
}