using Nord.Compiler.Ast;

namespace Nord.Compiler.Visitor
{
    public interface IAstVisitor
    {
        void OnNode(AstNode node);
    }
}