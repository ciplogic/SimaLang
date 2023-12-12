namespace MicroLang.Compiler.Semantic;

record TreeNode(string Name, List<TreeNode> Children, Dictionary<string, string> Props)
{
    internal TreeNode(string name)
        : this(name, new List<TreeNode>(), new Dictionary<string, string>())
    {
        
    }
    internal TreeNode this[string childKeyed]
    {
        get
        {
            TreeNode? first = Children.FirstOrDefault(c => c.Name == childKeyed);
            if (first is null)
            {
                first = new TreeNode(childKeyed);
                Children.Add(first);
            }

            return first;
        }
    }
}