using MicroLang.Compiler.Lex.Tok;
using MicroLang.Compiler.Parser.FirstPassParser;
using MicroLang.Utils;

namespace MicroLang.Compiler.Parser.DeclarationsParser.Declarations.Common;

abstract class GenericsNamedDeclaration : NamedDeclaration
{
    public string[] GenericTypeParams = Array.Empty<string>();
    protected Slice<TreeNodeParse> ExtractGenerics(Slice<TreeNodeParse> sliceTokens)
    {
        var targetAstNode = sliceTokens[0];
        if (targetAstNode.Tok.Text != "[")
        {
            return sliceTokens;
        }

        string[] tokenChildren =
            targetAstNode
                .Children
                .Select(node => node.Tok)
                .Where(tok => tok.Kind == TokenKind.Identifier)
                .Select(tok => tok.Text)
                .ToArray();
        
        GenericTypeParams = tokenChildren;
        return sliceTokens;
    }
}