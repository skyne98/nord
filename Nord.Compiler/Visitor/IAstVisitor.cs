using Nord.Compiler.Ast;
using Nord.Compiler.Generated.Ast;

namespace Nord.Compiler.Visitor
{
    public interface IAstVisitor
    {
        void OnNode(SyntaxNode node);
    }
}