using System;

namespace IUGO.Turns.Core.Specifications.common
{
    public class ExpressionSpecification<T> : CompositeSpecification<T>
    {
        private readonly Func<T, bool> _expression;
        public ExpressionSpecification(Func<T, bool> expression)
        {
            if (expression == null)
                throw new ArgumentNullException();
            else
                this._expression = expression;
        }

        public override bool IsSatisfiedBy(T o)
        {
            return this._expression(o);
        }
    }
}
