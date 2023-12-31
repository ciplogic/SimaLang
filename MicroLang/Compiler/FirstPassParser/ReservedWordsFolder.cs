using MicroLang.Compiler.Lex.Tok;

namespace MicroLang.Compiler.FirstPassParser;

static class ReservedWordsFolder
{

    private static (string ReservedWord, bool IsBlockEnded)[] Words =
    {
        ("enum", false),
        ("value", false),
        ("fn", true)
    };

    static (string ReservedWord, bool IsBlockEnded)? IsMatchReservedWord(Token token)
    {
        if (token.Kind != TokenKind.ReservedWord)
        {
            return null;
        }
        foreach ((string ReservedWord, bool IsBlockEnded) word in Words)
        {
            if (word.ReservedWord == token.Text)
            {
                return word;
            }
        }

        return null;
    }
    public static void FoldDeclarations(List<PassOneAstNode> foldableSection)
    {
        for (var i = 0; i < foldableSection.Count; i++)
        {
            var tok = foldableSection[i].Tok;
            var match = IsMatchReservedWord(tok);
            if (match is null)
            {
                continue;
            }

            i = FoldReservedWord(foldableSection, i, match.Value.IsBlockEnded);
        }
    }

    static int EndIndexOfDeclaration(List<PassOneAstNode> foldableSection, int startIndex, bool valueIsBlockEnded)
    {
        for (var i = startIndex + 1; i < foldableSection.Count; i++)
        {
            PassOneAstNode currentAstNode = foldableSection[i];
            var found = (valueIsBlockEnded && currentAstNode.Kind == AstNodeKind.Block && currentAstNode.Tok.Text == "{")
                        || currentAstNode.Tok.Kind == TokenKind.Eoln;
            if (found)
            {
                return i;
            }
        }

        return -1;
    }

    private static int FoldReservedWord(List<PassOneAstNode> foldableSection, int index, bool valueIsBlockEnded)
    {
        var endDeclarationIndex = EndIndexOfDeclaration(foldableSection, index, valueIsBlockEnded);
        var declarationNode = new PassOneAstNode(AstNodeKind.Declaration)
        {
            Tok = foldableSection[index].Tok
        };
        int length =
            valueIsBlockEnded
                ? endDeclarationIndex - index
                : endDeclarationIndex - index - 1;
        declarationNode.Children.AddRange(foldableSection.Skip(index+1).Take(length));
        foldableSection.RemoveRange(index+1, endDeclarationIndex - index);
        foldableSection[index] = declarationNode;
        return index;
    }
}