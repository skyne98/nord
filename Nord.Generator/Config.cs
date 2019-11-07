using System.IO;

namespace Nord.Generator
{
    public class Config
    {
        public static readonly string AstClassesDirectory = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../../Nord.Compiler/Generated/Ast/"));
        public static readonly string VisitorDirectory = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../../Nord.Compiler/Generated/Visitor/"));
    }
}