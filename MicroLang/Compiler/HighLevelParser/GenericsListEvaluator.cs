using MicroLang.Compiler.Semantic;

namespace MicroLang.Compiler.HighLevelParser;

public class GenericsListEvaluator
{
    public static void EvalAsTreeNode(Scanner scanner, TreeNode resultNode)
    {
        TreeNode genericsNode = resultNode.Child("Generics");
        while (scanner.CanMove)
        {
            if (scanner.MoveIf(","))
            {
                continue;
            }

            if (scanner.MoveIf(">"))
            {
                return;
            }

            var child = new TreeNode("FieldType");
            child["name"] = scanner.Move().Text;
            genericsNode.Children.Add(child);
        }
    }
}