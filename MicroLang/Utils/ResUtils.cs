namespace MicroLang.Utils;

internal static class ResUtils
{
    internal static Res<T> Ok<T>(this T val) => new(val, string.Empty);

    internal static Res<T> Fail<T>(string err) =>
        throw new InvalidOperationException(err);
    // TODO: modify code to this in production
        //new(default, err);
    
    internal static bool IsOk<T>(this Res<T> res) => string.IsNullOrEmpty(res.ErrorMessage);
}