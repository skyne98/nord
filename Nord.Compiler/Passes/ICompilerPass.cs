using Nord.Compiler.Ast;
using Nord.Compiler.Generated.Ast;

namespace Nord.Compiler.Pass
{
    public interface ICompilerPass
    {
        SyntaxNode Run(Context context);
    }
}