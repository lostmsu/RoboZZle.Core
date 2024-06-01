namespace RoboZZle.Core;

public static class RoboZZleCoreHelpers {
    /// <summary>
    /// Checks if value lies inside specified bounds (inclusive).
    /// </summary>
    /// <typeparam name="T">Type of value</typeparam>
    /// <param name="value">Original value</param>
    /// <param name="lower">Lower bound</param>
    /// <param name="upper">Upper bound</param>
    public static bool Between<T>(this T value, T lower, T upper)
        where T : IComparable<T> {
        return value.CompareTo(lower) >= 0 && value.CompareTo(upper) <= 0;
    }

    /// <summary>
    /// Concatenates all items in two-dimensional enumerable
    /// </summary>
    /// <typeparam name="T">Type of item</typeparam>
    /// <param name="value">Two-dimensional enumerable</param>
    /// <returns>Enumeration of all elements in 2D enumerable</returns>
    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> value) {
        return value.SelectMany(enumerable => enumerable);
    }

    /// <summary>
    /// <para>Initializes two-dimensional rectangular jagged array.</para>
    /// <para>Sample use: new T[l1][].Rect(l2)</para>
    /// </summary>
    /// <typeparam name="T">Type of element</typeparam>
    /// <param name="jagged">Jagged array to fill</param>
    /// <param name="secondDimension">Second dimension size</param>
    public static T[][] Rect<T>(this T[][] jagged, int secondDimension) {
        for (int i = 0; i < jagged.Length; i++) {
            jagged[i] = new T[secondDimension];
        }

        return jagged;
    }

    /// <summary>
    /// <para>Initializes two-dimensional rectangular jagged array.</para>para>
    /// <para>Calls constructor for each item.</para>
    /// <para>Sample use: new T[l1][].RectInit(l2)</para>
    /// </summary>
    /// <typeparam name="T">Type of element</typeparam>
    /// <param name="jagged">Jagged array to fill</param>
    /// <param name="secondDimension">Second dimension size</param>
    public static T[][] RectInit<T>(this T[][] jagged, int secondDimension)
        where T : class, new() {
        jagged.Rect(secondDimension);
        foreach (T[] row in jagged)
            for (int j = 0; j < secondDimension; j++) {
                row[j] = new T();
            }

        return jagged;
    }

    public static EventArgs<T> ToEventArgs<T>(this T value) {
        return new EventArgs<T>(value);
    }
}