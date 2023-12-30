using System;
using GSharpProject.Parsing;

namespace GSharpProject
{
    internal class FunctionReference : GSharpExpression
    {
        private readonly Func<EvalScope, object> function;
        public FunctionReference(Func<EvalScope, object> function, ExpressionType expressionType)
        {
            this.function = function;
            ExpressionType = expressionType;
        }
        public override TokenType TokenType => TokenType.FunctionReference;

        public override void CheckType(TypedScope typedScope) => ExpressionType = ExpressionType;

        public object Eval(EvalScope param)
        {
            return function(param);
        }
    }
}