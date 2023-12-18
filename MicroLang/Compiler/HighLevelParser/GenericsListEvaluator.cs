using MicroLang.Compiler.Semantic;

namespace MicroLang.Compiler.HighLevelParser.Classes;

public class GenericsListEvaluator
{
    public static void EvalAsTreeNode(Scanner scanner, TreeNode genericsNode)
    {
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