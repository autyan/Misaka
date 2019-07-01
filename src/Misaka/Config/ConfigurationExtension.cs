using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Misaka.Config
{
    public static class ConfigurationExtension
    {
        public static Configuration LoadComponent(this Configuration config, Assembly[] assemblies)
        {
            TypeProvider.Instance.LoadFromAssemblies(assemblies);
            return config;
        }

        public static Configuration LoadComponent(this Configuration config, string prefix)
        {
            var files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException());
            var validFiles = files.Where(f => f.EndsWith(".dll") && Path.GetFileName(f).StartsWith(prefix));
            TypeProvider.Instance.LoadFromAssemblies(validFiles.Select(Assembly.LoadFile).ToArray());
            return config;
        }
    }
}
