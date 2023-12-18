namespace MicroLang.Compiler.HighLevelParser.Classes;

public record ClassDef(
    string Name,
    bool IsByRef,
    string[] GenericArgs,
    PropertyDef[] Properties
    );

public record struct PropertyDef(string Name, int TypeId);