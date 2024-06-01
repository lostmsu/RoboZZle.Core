namespace RoboZZle.Core;

using System.Diagnostics;
using System.Globalization;

public static class DebugEx {
    static DateTime? start;

    /// <summary>
    /// Writes formatted message to debug listeners
    /// </summary>
    [Conditional("DEBUG")]
    public static void WriteLine(string format, params object[] args) {
        string message = string.Format(CultureInfo.InvariantCulture, format, args);
        WriteLine(message);
    }

    /// <summary>
    /// Writes message to debug listeners
    /// </summary>
    [Conditional("DEBUG")]
    public static void WriteLine(string message) {
        start ??= DateTime.UtcNow;

        Debug.WriteLine("[{0}] {1}", DateTime.UtcNow - start, message);
    }
}