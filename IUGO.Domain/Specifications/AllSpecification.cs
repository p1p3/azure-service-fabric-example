namespace IUGO.Domain.Specifications
{
    public class AllSpecification<T> : CompositeSpecification<T>
    {

        public override bool IsSatisfiedBy(T o)
        {
            return true;
        }
    }
}

