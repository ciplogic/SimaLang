namespace MicroLang.Compiler.Semantic;

class SemanticTree
{
    private TreeNode _root = new ("Program");

    internal TreeNode GetLibrary(string libraryName)
    {
        TreeNode targetLibrary = _root.Child("Libs").Child(libraryName);
        return targetLibrary; }
}
