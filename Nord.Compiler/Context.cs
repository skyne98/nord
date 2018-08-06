using System;
using System.Collections.Generic;
using System.Data;
using LanguageExt;
using Nord.Compiler.Ast;
using Nord.Compiler.Pass;

namespace Nord.Compiler
{
    public class Context
    {
        private long _variableCounter = 0;
        private long _typeCounter = 0;
        
        public Context(AstNode ast)
        {
            Ast = ast;
            _cache = new Dictionary<Type, AstNode>();
        }

        public string GenerateVariableName()
        {
            string name = $"__var{_variableCounter}";
            _variableCounter += 1;
            return name;
        }

        public string GenerateTypeName()
        {
            string name = $"__Type{_typeCounter}";
            _typeCounter += 1;
            return name;
        }

        public AstNode Require<T>() where T: ICompilerPass
        {
            var passType = typeof(T);
            if (_cache.ContainsKey(passType))
                return _cache[passType];

            var activatedPass = Activator.CreateInstance<T>();
            var result = activatedPass.Run(this);
            _cache.Add(passType, result);
            return result;
        }

        public AstNode Ast { get; set; }

        private Dictionary<Type, AstNode> _cache;
    }
}