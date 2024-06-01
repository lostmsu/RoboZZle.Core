namespace RoboZZle.Core;

public static class InstructionPointerExtensions {
    /// <summary>
    /// Returns instruction global index (-1 for null)
    /// </summary>
    /// <param name="pointer">Self</param>
    /// <param name="program"><see cref="Program"/> to calculate global index for</param>
    public static int ToGlobal(this InstructionPointer? pointer, Program program) {
        if (program == null)
            throw new ArgumentNullException(nameof(program));

        if (pointer == null)
            return -1;

        int index = 0;
        for (int i = 0; i < pointer.Function; i++)
            index += program.Functions[i].Commands.Length;
        if (pointer.Command >= program.Functions[pointer.Function].Commands.Length
         && pointer.Function + 1 >= program.Functions.Length) {
            return -1;
        }

        index += Math.Min(pointer.Command, program.Functions[pointer.Function].Commands.Length - 1);
        return index;
    }
}