using MicroLang.Compiler.Lexer.Tok;
using MicroLang.Compiler.Semantic;
using MicroLang.Utils;

namespace MicroLang.Compiler.HighLevelParser.Common;

public static class EnumEvaluator
{
    internal static TreeNode EvalAsTreeNode(Scanner scanner)
    {
        var ev = Eval(scanner);
        return ToTreeNode(ev.Fields, ev.Name);
    }

    private static TreeNode ToTreeNode(List<(string Key, long Value)> fields, string name)
    {
        TreeNode result = new TreeNode("enum");
        result["name"] = name;
        TreeNode fieldsNodes = result.Child("fields");
        foreach ((string Key, long Value) field in fields)
        {
            TreeNode fieldNode = fieldsNodes.Child(field.Key);
            fieldNode["Value"] = field.Value.ToString();
        }
        
        return result;
    }
    internal static (string Name, List<(string Key, long Value)> Fields) Eval(Scanner scanner)
    {
        string enumName = scanner.Advance("enum").Move().Text;
        scanner.Advance("(");
        
        List<(string Key, long Value)> fields = new();
        Slice<Token>[] enumBody = scanner
            .TakeWhile(tok => tok.Text != ")")
            .SplitWhen(tok => tok.Text == ",");
        foreach (Slice<Token> slice in enumBody)
        {
            long evalValueSlice = EvalFieldValueSlice(slice, fields); 
            fields.Add((slice[0].Text, evalValueSlice));    
        }
        
        return (enumName, fields);
    }

    private static long EvalFieldValueSlice(Slice<Token> slice, List<(string Key, long Value)> enumFields)
    {
        Slice<Token>[] splitEquals = slice.SplitWhen(t => t.Text == "=");
        if (splitEquals.Length == 1)
        {
            if (enumFields.Count == 0)
            {
                return 0;
            }

            return enumFields.Last().Value + 1;
        }

        //TODO: Eval more expressions
        Slice<Token> afterEquals = splitEquals[1];
        return long.Parse(afterEquals[0].Text);
    }
}