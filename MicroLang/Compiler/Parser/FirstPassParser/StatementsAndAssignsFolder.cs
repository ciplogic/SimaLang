using MicroLang.Compiler.Lex.Tok;

namespace MicroLang.Compiler.Parser.FirstPassParser;

public class StatementsAndAssignsFolder
{
    public static void FoldStatements(TreeNodeParse parent)
    {
        List<TreeNodeParse> foldableSection = parent.Children;
        for (int i = 0; i < foldableSection.Count; i++)
        {
            TreeNodeParse astToken = foldableSection[i];
            Token tok = astToken.Tok;
            switch (astToken.Kind)
            {
                case AstNodeKind.Terminal:
                {
                    if (astToken.Kind == AstNodeKind.Terminal && tok.Kind == TokenKind.Identifier)
                    {
                        TreeNodeParse foldedDeclarationNodeParse = FoldStatementWord(foldableSection, i);
                    }

                    break;
                }
                case AstNodeKind.Declaration:
                {
                    if (astToken.Children.Count == 0)
                    {
                        break;
                    }
                    TreeNodeParse lastNode = astToken.Children[^1];
                    if (lastNode.Tok.Text == "{")
                    {
                        FoldStatements(lastNode);
                    }
                    break;
                }
            }
        }
    }

    private static TreeNodeParse FoldStatementWord(List<TreeNodeParse> foldableSection, int startPos)
    {
        TreeNodeParse result = new TreeNodeParse(AstNodeKind.Statement);
        int endIndex = -1;
        for (int index = startPos; index < foldableSection.Count; index++)
        {
            if (foldableSection[index].Tok.Kind == TokenKind.Eoln)
            {
                endIndex = index;
                break;
            }
            
            result.Children.Add(foldableSection[index]);
        }

        if (endIndex == -1)
        {
            endIndex = foldableSection.Count;
        }
        foldableSection.RemoveRange(startPos+1, endIndex - startPos);
        foldableSection[startPos] = result;
        return result;
    }
}