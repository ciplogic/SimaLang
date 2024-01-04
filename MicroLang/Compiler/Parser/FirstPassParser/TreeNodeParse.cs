using MicroLang.Compiler.Lex.Tok;

namespace MicroLang.Compiler.Parser.FirstPassParser;

public class TreeNodeParse(AstNodeKind kind)
{
    public AstNodeKind Kind { get; } = kind;
    public List<TreeNodeParse> Children = new();
    public Token Tok = new (TokenKind.None, string.Empty);
}
