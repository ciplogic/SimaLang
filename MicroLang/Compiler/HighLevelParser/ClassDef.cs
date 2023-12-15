namespace MicroLang.Compiler.HighLevelParser;

public record ClassDef(
    string Name,
    int TypeId,
    bool IsByRef,
    string[] GenericArgs,
    PropertyDef[] Properties
    );

public record struct PropertyDef(string Name, int TypeId);