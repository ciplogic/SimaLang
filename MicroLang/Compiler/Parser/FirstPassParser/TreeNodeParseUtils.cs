using MicroLang.Compiler.Lex.Tok;

namespace MicroLang.Compiler.Parser.FirstPassParser;

public static class TreeNodeParseUtils
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

    public static bool IsCurlyBlockNode(this TreeNodeParse currentAstNodeParse) 
        => currentAstNodeParse.Kind == AstNodeKind.Block && currentAstNodeParse.Tok.Text == "{";
}