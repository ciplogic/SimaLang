using MicroLang.Compiler.Semantic;

namespace MicroLang.Compiler.HighLevelParser.Classes;

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
            TreeNode genericsNode = resultNode.Child("Generics");
            GenericsListEvaluator.EvalAsTreeNode(scanner, genericsNode);
            //handle generics
        }

        TreeNode argumentsNode = resultNode.Child("Params");

        if (scanner.MoveIf("("))
        {
            ArgListEvaluator.EvalAsTreeNode(scanner, argumentsNode);
            //handle properties

        }


        
        //ClassDef classDef = new ClassDef(name, isRef, )

        return resultNode;
    }
}