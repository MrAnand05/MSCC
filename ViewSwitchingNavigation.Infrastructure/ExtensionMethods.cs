namespace ViewSwitchingNavigation.Infrastructure
{
    public static class ExtensionMethods
    {
            public static bool In<T>(this T t, params T[] values)
            {
                foreach (T value in values)
                {
                    if (t.Equals(value))
                    {
                        return true;
                    }
                }
                return false;
            }
    }
}
