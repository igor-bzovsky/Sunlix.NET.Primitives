namespace Sunlix.NET.Primitives.Internals
{
    internal static class ExceptionMessages
    {
        public static class Enumeration<T>
        {
            const int ShortenedNameLength = 20;
            const string ShortenedNameSuffix = "...";

            public static string InvalidValue(int value)
                => $"Value '{value}' is invalid. Enumeration: {typeof(T).Name}";

            public static string InvalidName(string name)
                => $"Name '{name}' is invalid. Enumeration: {typeof(T).Name}";

            public static string NullOrEmptyName() => $"Name should not be null or empty. Enumeration: {typeof(T).Name}";

            public static string NameLengthExceeded(string name)
                => $"Name '{GetShortenedName(name)}...' length exceeds maximum length. Enumeration: {typeof(T).Name}";

            public static string DuplicateValue(int value)
                => $"Duplicate value detected. Enumeration: {typeof(T).Name}. Value: {value}";

            public static string DuplicateName(string name)
                => $"Duplicate name detected. Enumeration: {typeof(T).Name}. Name: '{name}'";

            private static string GetShortenedName(string name)
            {
                if (name.Length <= ShortenedNameLength + ShortenedNameSuffix.Length) return name;

                return name.Substring(0, ShortenedNameLength);
            }
        }
    }
}
