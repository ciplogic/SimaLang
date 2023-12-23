using System.Diagnostics;
using MicroLang.Compiler.HighLevelParser.Common;
using MicroLang.Compiler.Lexer.Tok;
using MicroLang.Compiler.Semantic;
using MicroLang.Utils;

namespace MicroLang.Compiler.HighLevelParser;

internal class HighLevelParse
{
    internal TreeNode ParseFileHighLevel(Token[] tokensArray)
    {
        Slice<Token> tokens = Slice<Token>.Build(tokensArray);
        TreeNode result = new("ParsedFile");
        TreeNode body = result.Child("Declarations");
        while (tokens.Len > 0)
        {
            Token firstToken = tokens[0];
            if (firstToken.Kind == TokenKind.Eoln)
            {
                tokens = tokens.SubSlice(1);
                continue;
            }

            if (firstToken.Kind == TokenKind.ReservedWord)
            {
                tokens = HandleReservedWord(tokens, body);
                
            }
            
        }
        return result;
    }

    private Slice<Token> HandleReservedWord(Slice<Token> tokens, TreeNode body)
    {
        int endWithBlock = tokens.IndexOf(tok => tok.Kind == TokenKind.Eoln || tok.Text == "{");
        if (endWithBlock == -1)
        {
            throw new InvalidDataException($"Cannot handle tokens:{tokens}");
        }

        if (tokens[endWithBlock].Kind == TokenKind.Eoln)
        {
            Slice<Token> simpleDeclarationSpan = tokens.SubSlice(0, endWithBlock-1);
            ParseOneRowDeclaration(simpleDeclarationSpan, body);

            return tokens.SubSlice(endWithBlock + 1);
        }

        int matchBlock = MatchBlock(tokens, "{", "}", body);
        Debug.Assert(matchBlock!=-1);

        return default;
    }

    private void ParseOneRowDeclaration(Slice<Token> oneLineDeclaration, TreeNode body)
    {
        switch (oneLineDeclaration[0].Text)
        {
            case "value":
            {
                var scanner = new Scanner(oneLineDeclaration);
                var treeNode = ValueEvaluator.EvalAsTreeNode(scanner);
                body.Children.Add(treeNode);
            }
                break;
            case "enum":
                body.Children.Add(EnumEvaluator.EvalAsTreeNode(oneLineDeclaration));
                break;
        }
    }

    int MatchBlock(Slice<Token> tokens, string openToken, string closeToken, TreeNode body)
    {
        Scanner scanner = new (tokens);
        switch (scanner.PeekText())
        {
            case "fn":
            {
                TreeNode declaration = FunctionDeclareEvaluator.EvalAsTreeNode(scanner);
                body.Children.Add(declaration);
                Debug.Assert(scanner.Peek(openToken));
                break;
            }
        }

        return -1;
    }
}