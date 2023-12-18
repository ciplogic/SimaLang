using MicroLang.Compiler.Lexer.Tok;
using MicroLang.Compiler.Semantic;

namespace MicroLang.Compiler.HighLevelParser;

public class FunctionDeclareEvaluator
{
    public static TreeNode EvalAsTreeNode(Scanner scanner)
    {
        scanner.Advance("fn");
        string name = scanner.Move().Text;
        TreeNode resultNode = new TreeNode("value");
        resultNode["name"] = name;

        if (scanner.MoveIf("<"))
        {
            GenericsListEvaluator.EvalAsTreeNode(scanner, resultNode);
        }

        if (scanner.MoveIf("("))
        {
            EvalParamsAsTreeNode(scanner, resultNode);
        }

        return resultNode;
    }

    private static void EvalParamsAsTreeNode(Scanner scanner, TreeNode resultNode)
    {
        var argsNode = resultNode.Child("Args");
        List<string> paramDef = new();
        while (true)
        {
            if (scanner.MoveIf(")"))
            {
                break;
            }

            var paramNames = GetVarNames(scanner);
            var paramDefs = GetParamDefs(scanner);
            foreach (string paramName in paramNames)
            {
                TreeNode paramNode = new TreeNode();
                paramNode["name"] = paramName;
                paramNode["type"] = string.Join(" ", paramDefs);
                argsNode.Children.Add(paramNode);
            }
        }

    }

    static List<string> GetVarNames(Scanner scanner)
    {
        List<string> paramNames = new ();
        while (!scanner.Peek(")"))
        {
            while (scanner.Peek(TokenKind.Identifier))
            {
                paramNames.Add(scanner.Move().Text);
                scanner.MoveIf(",");
            }

            if (scanner.MoveIf(":"))
            {
                return paramNames;
            }
        }

        return paramNames;
    }
    static List<string> GetParamDefs(Scanner scanner)
    {
        List<string> paramNames = new ();
        while (!scanner.Peek(")"))
        {
            while (!scanner.Peek(")"))
            {
                paramNames.Add(scanner.Move().Text);
            }
        }

        return paramNames;
    }
}