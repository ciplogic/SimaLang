using MicroLang.Compiler.Lexer.Tok;
using MicroLang.Compiler.Semantic;

namespace MicroLang.Compiler.HighLevelParser;

public class FunctionDeclareEvaluator
{
    public static TreeNode EvalAsTreeNode(Scanner scanner)
    {
        scanner.Advance("fn");
        string name = scanner.Move().Text;
        TreeNode resultNode = new TreeNode("fn");
        resultNode["name"] = name;

        if (scanner.MoveIf("<"))
        {
            GenericsListEvaluator.EvalAsTreeNode(scanner, resultNode);
        }

        if (scanner.MoveIf("("))
        {
            EvalParamsAsTreeNode(scanner, resultNode);
        }

        if (scanner.MoveIf(":"))
        {
            EvalFunctionReturnType(scanner, resultNode);
        }

        EvalFunctionBody(scanner, resultNode);

        return resultNode;
    }

    private static void EvalFunctionBody(Scanner scanner, TreeNode resultNode)
    {
        while (true)
        {
            TreeNode functionBody = FunctionBodyEvaluator.EvalAsTreeNode(scanner);
            resultNode.Children.Add(functionBody);
            
            
        }
        
    }

    private static void EvalFunctionReturnType(Scanner scanner, TreeNode resultNode)
    {
        var tokenWords = new List<string>();
        while (!scanner.Peek("{"))
        {
            tokenWords.Add(scanner.Move().Text);
        }

        var retType = new TreeNode("ReturnType");
        foreach (var tw in tokenWords)
        {
            var typeComponent = new TreeNode("TypeComponent");
            typeComponent["id"] = tw;
            retType.Children.Add(typeComponent);
        }
        resultNode.Children.Add(retType);
    }

    private static void EvalParamsAsTreeNode(Scanner scanner, TreeNode resultNode)
    {
        var argsNode = resultNode.Child("Args");
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