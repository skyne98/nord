﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using LanguageExt;

namespace Nord.Compiler.Ast
{
    [Serializable]
    public class AstTypeNode: AstNode
    {

    }

    [Serializable]
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

    [Serializable]
    public class AstTypeReferenceNode : AstTypeNode
    {
        public AstTypeReferenceNode(string name, AstTypeReferenceNode[] arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public string Name { get; private set; }
        public AstTypeReferenceNode[] Arguments { get; private set; }
    }

    [Serializable]
    public class AstTypeParameterNode : AstTypeNode
    {
        public AstTypeParameterNode(string name, AstTypeReferenceNode constraint = null)
        {
            Name = name;
            Constraint = constraint;
        }

        public string Name { get; private set; }
        public Option<AstTypeReferenceNode> Constraint { get; private set; }
    }
}
