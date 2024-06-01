namespace RoboZZle.Core;

using System.Collections.ObjectModel;

public enum Color {
    RED,
    GREEN,
    BLUE,
}

/// <summary>
/// Helpers to work with Color enumeration
/// </summary>
public static class Colors {
    /// <summary>
    /// Gets command to paint this color (as CommandSet)
    /// </summary>
    /// <param name="value">Color</param>
    /// <returns>Command from CommandSet</returns>
    public static CommandSet PaintCommand(this Color value) {
        return value switch {
            Color.BLUE => CommandSet.PAINT_BLUE,
            Color.GREEN => CommandSet.PAINT_GREEN,
            Color.RED => CommandSet.PAINT_RED,
            _ => throw new InvalidCastException()
        };
    }

    /// <summary>
    /// Collection of all colors
    /// </summary>
    public static IEnumerable<Color> All { get; } =
        new ReadOnlyCollection<Color>(new[] { Color.BLUE, Color.GREEN, Color.RED });

    static readonly Dictionary<Color, char> ColorChars =
        new() {
            { Color.RED, 'r' },
            { Color.GREEN, 'g' },
            { Color.BLUE, 'b' },
        };

    public static char ToChar(this Color color) => ColorChars[color];

    public static char ToChar(this Color? color) {
        return color == null
            ? '_'
            : ColorChars[color.Value];
    }
}