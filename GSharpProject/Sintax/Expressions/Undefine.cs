namespace GSharpProject;

internal class Undefined
{
    private Undefined() {}
    private static Undefined undef = null!;
    public static Undefined Value {
        get {
            undef ??= new Undefined();
            return undef;
        }
    }
}