namespace IUGO.Domain.Specifications
{
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        readonly ISpecification<T> _leftSpecification;
        readonly ISpecification<T> _rightSpecification;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this._leftSpecification = left;
            this._rightSpecification = right;
        }

        public override bool IsSatisfiedBy(T o)
        {
            return this._leftSpecification.IsSatisfiedBy(o)
                   && this._rightSpecification.IsSatisfiedBy(o);
        }
    }
}