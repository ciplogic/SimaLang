using MicroLang.Compiler.Lex.Tok;
using MicroLang.Compiler.Parser.DeclarationsParser.Declarations.Common;
using MicroLang.Compiler.Parser.FirstPassParser;
using MicroLang.Utils;

namespace MicroLang.Compiler.Parser.DeclarationsParser.Declarations;

internal class EnumDeclaration : NamedDeclaration
{
    public List<(string Key, long Value)> Fields { get; private set; }

    public EnumDeclaration(Slice<TreeNodeParse> declarationNode)
    {
        Setup(declarationNode[0].Tok.Text, NamedDeclarationKind.Enum);
        ExtractValues(declarationNode[1].Children);
    }

    private void ExtractValues(List<TreeNodeParse> treeNodes)
    {
        Token[] filteredNodes = treeNodes
            .Select(node => node.Tok)
            .Where(tok => tok.Kind != TokenKind.Eoln)
            .ToArray();

        Slice<Token> tokensSlice = Slice<Token>.Build(filteredNodes);

        Slice<Token>[] enumBody = tokensSlice
            .SplitWhen(tok => tok.Text == ",");

        List<(string Key, long Value)> fields = new();
        foreach (Slice<Token> slice in enumBody)
        {
            long evalValueSlice = EvalFieldValueSlice(slice, fields);
            fields.Add((slice[0].Text, evalValueSlice));
        }

        Fields = fields;
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