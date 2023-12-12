namespace MicroLang.Utils;

public static class ResUtils
{
    public static Res<T> Ok<T>(this T val) => new Res<T>(val, string.Empty);
    public static Res<T> Fail<T>(string err) => new Res<T>(default, err);
}