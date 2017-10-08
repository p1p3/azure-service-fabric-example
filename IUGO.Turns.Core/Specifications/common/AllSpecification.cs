namespace IUGO.Turns.Core.Specifications.common
{
    public class AllSpecification<T> : CompositeSpecification<T>
    {

        public override bool IsSatisfiedBy(T o)
        {
            return true;
        }
    }
}

