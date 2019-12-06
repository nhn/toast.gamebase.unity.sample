using System;
using System.Linq;
using System.Text;

namespace Toast.Internal
{
    public static class ToastUri
    {
        private const string Scheme = "toast";

        public class BaseSegment
        {
            public BaseSegment(string name)
            {
                Name = name;
                IsLowerCase = true;
            }

            protected string Name { get; set; }
            protected bool IsLowerCase { get; set; }

            public static implicit operator BaseSegment(string path)
            {
                return new BaseSegment(path);
            }

            public override string ToString()
            {
                return IsLowerCase ? Name.ToLower() : Name;
            }
        }

        public class VariableSegment : BaseSegment
        {
            public VariableSegment(string name) : base(name)
            {
                IsLowerCase = false;
            }
        }

        public static string Create(string serviceName)
        {
            var builder = new UriBuilder(Scheme, serviceName);
            return builder.ToString().TrimEnd('/').ToLower();
        }

        /// <summary>
        /// Create URI for toast scheme to communicate platform-specific services. Return lower-case URI
        /// </summary>
        /// <param name="serviceName">Serice name for communicating</param>
        /// <param name="paths">Path for method or resources</param>
        /// <returns>Created lower-case URI</returns>
        public static string Create(string serviceName, params string[] paths)
        {
            var builder = new UriBuilder(Scheme, serviceName)
            {
                Path = CreateUriPath(paths)
            };
            return builder.ToString().TrimEnd('/').ToLower();
        }

        public static string Create(string serviceName, params BaseSegment[] paths)
        {
            var builder = new UriBuilder(Scheme, serviceName)
            {
                Path = CreateUriPath(paths.Select(s => s.ToString()).ToArray())
            };
            return builder.ToString().TrimEnd('/');
        }

        private static string CreateUriPath(params string[] paths)
        {
            var builder = new StringBuilder();
            foreach (var path in paths)
            {
                builder.Append(path);
                builder.Append("/");
            }

            return builder.ToString().TrimEnd('/');
        }
    }
}