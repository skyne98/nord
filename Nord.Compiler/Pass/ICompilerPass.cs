using Nord.Compiler.Ast;

namespace Nord.Compiler.Pass
{
    public interface ICompilerPass
    {
        AstNode Run(Context context);
    }
}