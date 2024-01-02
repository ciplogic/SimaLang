using MicroLang.Compiler.Lex.Tok;

namespace MicroLang.Compiler.Parser.FirstPassParser;

public static class PassOneAstNodeUtils
{
    public static int IsOpeningToken(this Token tok) 
        => tok.Text switch
        {
            "{" => 1,
            "(" => 2,
            "[" => 3,
            _ => 0
        };

    public static int IsClosingToken(this Token tok) 
        => tok.Text switch
        {
            "}" => 1,
            ")" => 2,
            "]" => 3,
            _ => 0
        };

    public static bool IsCurlyBlockNode(this PassOneAstNode currentAstNode) 
        => currentAstNode.Kind == AstNodeKind.Block && currentAstNode.Tok.Text == "{";
}