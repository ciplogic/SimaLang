using MicroLang.Compiler.Lex.Tok;

namespace MicroLang.Compiler.Parser.FirstPassParser;

public class PassOneAstNode(AstNodeKind Kind)
{
    public AstNodeKind Kind { get; } = Kind;
    public List<PassOneAstNode> Children = new();
    public Token Tok;
}
