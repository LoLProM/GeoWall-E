namespace GSharpProject;

internal class Undefined //tipo undefined
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