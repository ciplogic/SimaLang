using System.Text;

namespace MicroLang.Compiler.OutputWriter;

public static class CodeWriter
{
    public static void BuildCmakeList(string outFileName, string projectName, string cmakeMinVer, int cppStandard)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"cmake_minimum_required(VERSION {cmakeMinVer})");
        sb.AppendLine($"project({projectName})");
        sb.AppendLine($"set(CMAKE_CXX_STANDARD {cppStandard})");
        sb.AppendLine($"add_executable(OutputCode main.cpp)");
        string fullCode = sb.ToString();
        File.WriteAllText(outFileName, fullCode);
    }

    public static void WriteCode(string outFile, List<SemanticDeclaration> declarations)
    {
        
    }
}