using MicroLang.Compiler.Parser.DeclarationsParser.Declarations.Common;
using MicroLang.Compiler.Parser.FirstPassParser;
using MicroLang.Utils;

namespace MicroLang.Compiler.Parser.DeclarationsParser.Declarations;

internal class ValueDeclaration : GenericsNamedDeclaration
{
    public ValueDeclaration(Slice<TreeNodeParse> slice)
    {
        if (slice[0].Tok.Text == "&")
        {
            IsRef = true;
            slice = slice.Skip(1);
        }

        Setup(slice[0].Tok.Text, NamedDeclarationKind.Value);
    }

    public bool IsRef { get; set; }
}