namespace Calculator.NativeMethods;

public static class Types
{
    public static long I64(dynamic value) => (long)value;

    public static int I32(dynamic value) => (int)value;

    public static short I16(dynamic value) => (short)value;

    public static short I8(dynamic value) => (byte)value;

    public static double F64(dynamic value) => (double)value;

    public static float F32(dynamic value) => (float)value;
}
