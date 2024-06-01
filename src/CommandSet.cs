namespace RoboZZle.Core;

[Flags]
public enum CommandSet {
    DEFAULT = 0,
    PAINT_RED = 1,
    PAINT_GREEN = 2,
    PAINT_BLUE = 4,
    PAINT_ALL = PAINT_RED | PAINT_GREEN | PAINT_BLUE,
}