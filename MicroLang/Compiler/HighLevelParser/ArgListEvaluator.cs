using MicroLang.Compiler.Semantic;

namespace MicroLang.Compiler.HighLevelParser.Classes;

class ArgListEvaluator
{
    internal static void EvalAsTreeNode(Scanner scanner, TreeNode argumentsNode)
    {
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