using MicroLang.Compiler.Parser.DeclarationsParser.Declarations;
using MicroLang.Compiler.Parser.DeclarationsParser.Declarations.Common;
using MicroLang.Compiler.Parser.FirstPassParser;
using MicroLang.Utils;

namespace MicroLang.Compiler.Parser.DeclarationsParser;

public class DeclarationParsing
{
    internal static void Execute(ModuleDeclarations moduleDeclarations, string libNameSpace, TreeNodeParse perFileTree)
    {
        TreeNodeParse[] parentDeclarations = perFileTree
            .Children
            .Where(node => node.Kind == AstNodeKind.Declaration)
            .ToArray();

        foreach (TreeNodeParse declarationNode in parentDeclarations)
        {
            Slice<TreeNodeParse> slice = Slice<TreeNodeParse>.Build(declarationNode.Children.ToArray());
            NamedDeclaration? namedDeclaration = AddDeclaration(moduleDeclarations, libNameSpace, declarationNode.Tok.Text, slice);
            if (namedDeclaration != null)
            {
                namedDeclaration.NameSpace = libNameSpace;
                moduleDeclarations.Declarations.Add(namedDeclaration);
            }
        }
    }

    private static NamedDeclaration? AddDeclaration(ModuleDeclarations moduleDeclarations, string libNameSpace, string tokText,
        Slice<TreeNodeParse> children)
    {
        switch (tokText)
        {
            case "interface":
                return new InterfaceDeclaration(children);
            case "value":
                return new ValueDeclaration(children);
            case "enum":
                return new EnumDeclaration(children);
            default:
                return default;
        }
    }
}