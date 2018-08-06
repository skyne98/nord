using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;

namespace Nord.Compiler.Ast
{
    [Serializable]
    public class AstNode
    {
    }

    [Serializable]
    public class AstDocumentNode : AstNode
    {
        public AstDocumentNode(AstStatementTopLevelNode[] statements)
        {
            Statements = statements;
        }

        public AstStatementTopLevelNode[] Statements { get; private set; }
    }

    [Serializable]
    public class AstDestructuringPatternNode : AstNode
    {
    }

    [Serializable]
    public class AstDestructuringPatternBindingElement : AstNode
    {
        public AstDestructuringPatternBindingElement(string name, Either<string, AstDestructuringPatternNode>? @alias)
        {
            Name = name;

            if (@alias != null)
                Alias = alias.Value;
            else
                Alias = name;
        }

        public string Name { get; private set; }
        public Either<string, AstDestructuringPatternNode> Alias { get; private set; }
    }

    [Serializable]
    public class AstDestructuringArrayPatternNode : AstDestructuringPatternNode
    {
        public AstDestructuringArrayPatternNode(Either<string, AstDestructuringPatternNode>[] aliases, string rest)
        {
            Aliases = aliases;
            Rest = rest;
        }

        public Either<string, AstDestructuringPatternNode>[] Aliases { get; private set; }
        public Option<string> Rest { get; private set; }
    }

    [Serializable]
    public class AstDestructuringTuplePatternNode : AstDestructuringPatternNode
    {
        public AstDestructuringTuplePatternNode(Either<string, AstDestructuringPatternNode>[] aliases, string rest)
        {
            Aliases = aliases;
            Rest = rest;
        }

        public Either<string, AstDestructuringPatternNode>[] Aliases { get; private set; }
        public Option<string> Rest { get; private set; }
    }

    [Serializable]
    public class AstDestructuringObjectPetternNode : AstDestructuringPatternNode
    {
        public AstDestructuringObjectPetternNode(AstDestructuringPatternBindingElement[] bindingElements, string rest)
        {
            BindingElements = bindingElements;
            Rest = rest;
        }

        public AstDestructuringPatternBindingElement[] BindingElements { get; private set; }
        public Option<string> Rest { get; private set; }
    }
}
