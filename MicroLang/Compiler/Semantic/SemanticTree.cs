namespace MicroLang.Compiler.Semantic;

class SemanticTree
{
    private TreeNode Root = new ("Program");

    internal TreeNode GetLibrary(string libraryName)
    {
        TreeNode targetLibrary = Root["Libs"][libraryName];
        return targetLibrary; }
}
