namespace MicroLang.Utils;

public record struct Res<T>(T Value, string ErrorMessage)
{
    public bool IsOk => string.IsNullOrEmpty(ErrorMessage);
};