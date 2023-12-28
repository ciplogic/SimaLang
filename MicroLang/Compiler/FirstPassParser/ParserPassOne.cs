using System.Diagnostics;
using MicroLang.Compiler.Lex.Tok;
using MicroLang.Utils;

namespace MicroLang.Compiler.FirstPassParser;

enum AstNodeKind
{
    Terminal,
    Program,
    Block,
}
class PassOneAstNode
{
    public List<PassOneAstNode> Children = new();
    public Token Tok;
    public AstNodeKind Kind;
}

public class ParserPassOne
{
    static int IsOpeningToken(Token tok) 
        => tok.Text switch
        {
            "{" => 1,
            "(" => 2,
            "[" => 3,
            _ => 0
        };

    static int IsClosingToken(Token tok) 
        => tok.Text switch
        {
            "}" => 1,
            ")" => 2,
            "]" => 3,
            _ => 0
        };

    static void FoldParens(List<PassOneAstNode> foldableSection)
    {
        Stack<(int index, int openType)> OpeningTokPos = new Stack<(int index, int openType)>();
        for (var i = 0; i < foldableSection.Count; i++)
        {
            var tok = foldableSection[i].Tok;
            int openIndex = IsOpeningToken(tok);
            int closeIndex = IsClosingToken(tok);
            if (openIndex == 0 && closeIndex == 0)
            {
                continue;
            }

            if (openIndex != 0)
            {
                OpeningTokPos.Push((i, openIndex));
                continue;
            }

            (int index, int openType) openTokStart = OpeningTokPos.Pop();
            Debug.Assert(openTokStart.openType == closeIndex);
            i = FoldParenRange(foldableSection, tok, openTokStart.index, i);
        }
        
        Debug.Assert(OpeningTokPos.Count == 0);
    }

    private static int FoldParenRange(List<PassOneAstNode> foldableSection, Token tok, int startIndex, int endIndexInclusive)
    {
        var foldableTok = new PassOneAstNode()
        {
            Kind = AstNodeKind.Block,
            Tok = tok
        };
        for (var i = startIndex + 1; i < endIndexInclusive; i++)
        {
            var terminalChild = foldableSection[i];
            foldableTok.Children.Add(terminalChild);
        }
        foldableSection.RemoveRange(startIndex+1, endIndexInclusive - startIndex);
        foldableSection[startIndex] = foldableTok;
        return startIndex;
    }


    internal static PassOneAstNode Parse(Slice<Token> tokens)
    {
        var program = new PassOneAstNode()
        {
            Kind = AstNodeKind.Program
        };
        for (var i = 0; i<tokens.Len; i++)
        {
            program.Children.Add(new PassOneAstNode()
            {
                Tok = tokens[i],
                Kind = AstNodeKind.Terminal
            });
        }

        FoldParens(program.Children);
        

        return program;

    } 
}
