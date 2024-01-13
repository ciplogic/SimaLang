using MicroLang.Compiler.Lex.Tok;

namespace MicroLang.Compiler.Parser.FirstPassParser;

static class ReservedWordsFolder
{
    public static void FoldDeclarations(List<TreeNodeParse> foldableSection)
    {
        for (int i = 0; i < foldableSection.Count; i++)
        {
            Token tok = foldableSection[i].Tok;
            (string ReservedWord, bool IsBlockEnded)? match = IsMatchReservedWord(tok);
            if (match is null)
            {
                continue;
            }

            TreeNodeParse foldedDeclarationNodeParse = FoldReservedWord(foldableSection, i, match.Value.IsBlockEnded);
            if (foldedDeclarationNodeParse.Tok.Text == "fn")
            {
                FoldBodyDeclaration(foldedDeclarationNodeParse.Children[^1]);
            }
        }
    }
    
    private static (string ReservedWord, bool IsBlockEnded)[] Words =
    {
        ("enum", false),
        ("value", false),
        ("yield", false),
        ("return", false),
        ("interface", false),
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

    static void FoldBodyDeclaration(TreeNodeParse parentBlockNodeParse)
    {
        FoldDeclarations(parentBlockNodeParse.Children);
        
        //TODO: fix this condition to handle recursive blocks
        foreach (TreeNodeParse astNode in parentBlockNodeParse.Children)
        {
            if (astNode.Kind != AstNodeKind.Declaration)
            {
                continue;
            }

            if (astNode.Children.Count == 0)
            {
                continue;
            }

            TreeNodeParse lastDeclarationChild = astNode.Children[^1];
            if (lastDeclarationChild.Kind == AstNodeKind.Block && lastDeclarationChild.Tok.Text == "{")
            {
                FoldBodyDeclaration(lastDeclarationChild);
            }
        }
    }

    static int EndIndexOfDeclaration(List<TreeNodeParse> foldableSection, int startIndex, bool valueIsBlockEnded)
    {
        for (int i = startIndex + 1; i < foldableSection.Count; i++)
        {
            TreeNodeParse currentAstNodeParse = foldableSection[i];
            bool found = (valueIsBlockEnded && currentAstNodeParse.IsCurlyBlockNode()) || currentAstNodeParse.Tok.Kind == TokenKind.Eoln;
            if (found)
            {
                return i;
            }
        }

        return -1;
    }

    private static TreeNodeParse FoldReservedWord(List<TreeNodeParse> foldableSection, int index, bool valueIsBlockEnded)
    {
        int endDeclarationIndex = EndIndexOfDeclaration(foldableSection, index, valueIsBlockEnded);
        TreeNodeParse declarationNode = new TreeNodeParse(AstNodeKind.Declaration)
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