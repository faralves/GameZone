namespace GameZone.Core.DomainObjects
{
    public static class Contract
    {
        public static void ArgumentNotNull(object arg, string argumentName)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void ArgumentNotEmpty(string arg, string argumentName)
        {
            if (string.IsNullOrEmpty(arg))
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void ArgumentPositive<T>(T arg, string argumentName)
            where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default(T)) <= 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void ArgumentNotNegative<T>(T arg, string argumentName)
            where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default(T)) < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void Invariant(bool test)
        {
            if (!test)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
