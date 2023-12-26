using MicroLang.Compiler.Lexer.Tok;
using MicroLang.Compiler.Semantic;

namespace MicroLang.Compiler.HighLevelParser;

public class FunctionBodyEvaluator
{
    public static TreeNode EvalAsTreeNode(Scanner scanner)
    {
        var resultNode = new TreeNode("CodeBlock");
        scanner.Advance("{");
        while (true)
        {


            if (scanner.Peek(TokenKind.ReservedWord))
            {
                string reservedWord = scanner.PeekText();
                switch (reservedWord)
                {
                    case "if":
                    case "while":
                    case "for":

                        break;

                    case "return":
                        AddPerLineExpression("return", scanner, resultNode);
                        break;
                }

            }

            while (scanner.MoveIf(TokenKind.Eoln))
            {
            }

            if (scanner.MoveIf("}"))
            {
                return resultNode;
            }
        }
    }

    private static void AddPerLineExpression(string statementType, Scanner scanner, TreeNode resultNode)
    {
        //skip the reserved word
        scanner.Move();
        var statementNodeType = new TreeNode(statementType);
        resultNode.Children.Add(statementNodeType);
        var exprNode = statementNodeType.Child("Expression");
        while (!scanner.Peek(TokenKind.Eoln))
        {
            var exprItem = new TreeNode("ExprItem");
            var currToken = scanner.Move();
            exprItem["Text"] = currToken.Text;
            exprItem["Kind"] = currToken.Kind.ToString();
            exprNode.Children.Add(exprItem);
        }

        //skip the final eoln
        scanner.Move();
    }
}