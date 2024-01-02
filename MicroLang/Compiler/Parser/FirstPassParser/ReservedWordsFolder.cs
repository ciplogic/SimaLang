using MicroLang.Compiler.Lex.Tok;

namespace MicroLang.Compiler.Parser.FirstPassParser;

static class ReservedWordsFolder
{

    private static (string ReservedWord, bool IsBlockEnded)[] Words =
    {
        ("enum", false),
        ("value", false),
        ("return", false),
        ("for", true),
        ("if", true),
        ("elif", true),
        ("else", true),
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

    static void FoldBodyDeclaration(PassOneAstNode parentBlockNode)
    {
        FoldDeclarations(parentBlockNode.Children);
        
        //TODO: fix this condition to handle recursive blocks
        foreach (PassOneAstNode astNode in parentBlockNode.Children)
        {
            if (astNode.Kind != AstNodeKind.Declaration)
            {
                continue;
            }

            var lastDeclarationChild = astNode.Children[^1];
            if (lastDeclarationChild.Kind == AstNodeKind.Block && lastDeclarationChild.Tok.Text == "{")
            {
                FoldBodyDeclaration(lastDeclarationChild);
            }
        }
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

            PassOneAstNode foldedDeclarationNode = FoldReservedWord(foldableSection, i, match.Value.IsBlockEnded);
            if (foldedDeclarationNode.Tok.Text == "fn")
            {
                FoldBodyDeclaration(foldedDeclarationNode.Children[^1]);
            }
        }
    }


    static int EndIndexOfDeclaration(List<PassOneAstNode> foldableSection, int startIndex, bool valueIsBlockEnded)
    {
        for (var i = startIndex + 1; i < foldableSection.Count; i++)
        {
            PassOneAstNode currentAstNode = foldableSection[i];
            var found = (valueIsBlockEnded && currentAstNode.IsCurlyBlockNode()) || currentAstNode.Tok.Kind == TokenKind.Eoln;
            if (found)
            {
                return i;
            }
        }

        return -1;
    }

    private static PassOneAstNode FoldReservedWord(List<PassOneAstNode> foldableSection, int index, bool valueIsBlockEnded)
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
        return declarationNode;
    }
}