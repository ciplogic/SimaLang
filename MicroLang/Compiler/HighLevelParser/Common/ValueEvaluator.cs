using MicroLang.Compiler.Semantic;

namespace MicroLang.Compiler.HighLevelParser.Common;

public class ValueEvaluator
{
    // value &? ValueName <CommaSeparatedGenericTypes>? (( params ))
    internal static TreeNode EvalAsTreeNode(Scanner scanner)
    {
        scanner.Advance("value");
        TreeNode resultNode = new TreeNode("value");
        bool isRef = scanner.MoveIf("?");
        resultNode["isRef"] = ""+isRef;
        string name = scanner.Move().Text;
        resultNode["name"] = name;
        
        if (scanner.MoveIf("<"))
        {
            GenericsListEvaluator.EvalAsTreeNode(scanner, resultNode);
        }
        
        if (scanner.MoveIf("("))
        {
            EvalAsTreeNode(scanner, resultNode);
        }
        return resultNode;
    }
    
    private static void EvalAsTreeNode(Scanner scanner, TreeNode resultNode)
    {
        TreeNode argumentsNode = resultNode.Child("Params");
        List<string> paramNames = new List<string>();
        List<string> paramsTypes = new List<string>();
        bool readParamNames = true;
        
        while (true)
        {
            if (scanner.MoveIf(")"))
            {
                return;
            }

            if (readParamNames)
            {
                if (scanner.MoveIf(":"))
                {
                    readParamNames = false;
                    continue;
                }

                var paramName = scanner.Move();
                paramNames.Add(paramName.Text);
                scanner.MoveIf(",", " ");
            }
            else
            {
                var paramType = scanner.Move();
                paramsTypes.Add(paramType.Text);

                if (scanner.MoveIf(",") || scanner.Peek(")"))
                {
                    foreach (string paramName in paramNames)
                    {
                        var child = new TreeNode("ParamItem");
                        child["id"] = paramName;
                        child["fieldType"] = string.Join("", paramsTypes);
                        argumentsNode.Children.Add(child);
                        
                    }
                    paramNames.Clear();
                    paramsTypes.Clear();
                    readParamNames = true;
                }
            }
        }
    }
}