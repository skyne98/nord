using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;

namespace Nord.Compiler.Ast
{
    public class AstNode
    {
    }

    public class AstDestructuringPatternNode : AstNode
    {
    }

    public class AstDestructuringPatternBindingElement : AstNode
    {
        public AstDestructuringPatternBindingElement(string name, Either<string, AstDestructuringPatternNode> @alias)
        {
            Name = name;
            Alias = alias;
        }

        public string Name { get; private set; }
        public Option<Either<string, AstDestructuringPatternNode>> Alias { get; private set; }
    }

    public class AstDestructuringArrayPatternNode : AstDestructuringPatternNode
    {
        public AstDestructuringArrayPatternNode(string[] names, string rest)
        {
            Names = names;
            Rest = rest;
        }

        public string[] Names { get; private set; }
        public Option<string> Rest { get; private set; }
    }

    public class AstDestructuringTuplePatternNode : AstDestructuringPatternNode
    {
        public AstDestructuringTuplePatternNode(string[] names, string rest)
        {
            Names = names;
            Rest = rest;
        }

        public string[] Names { get; private set; }
        public Option<string> Rest { get; private set; }
    }

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
