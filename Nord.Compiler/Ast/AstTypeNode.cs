using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;

namespace Nord.Compiler.Ast
{
    public class AstTypeNode: AstNode
    {

    }

    public class AstTypeDeclaratorNode : AstNode
    {
        public AstTypeDeclaratorNode(string name, AstTypeNode type = null)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; private set; }
        public Option<AstTypeNode> Type { get; private set; }
    }

    public class AstTypeAnnotationNode : AstTypeNode
    {
        public AstTypeAnnotationNode(string name, AstTypeAnnotationNode[] parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public string Name { get; private set; }
        public AstTypeAnnotationNode[] Parameters { get; private set; }
    }
}
