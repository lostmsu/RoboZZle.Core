namespace RoboZZle.Core;

public sealed class Robot {
    public int X { get; set; }
    public int Y { get; set; }
    public Direction Direction { get; set; }

    /// <summary>
    /// Creates a structural copy of the robot.
    /// </summary>
    /// <returns>A copy of the robot.</returns>
    public Robot Clone() => new() { X = this.X, Y = this.Y, Direction = this.Direction };
}