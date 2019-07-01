using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Misaka.Config
{
    public class TypeProvider
    {
        public static TypeProvider Instance { get; } = new TypeProvider();

        private readonly Dictionary<Type, TypeInfo> _loadedTypes = new Dictionary<Type, TypeInfo>();

        private TypeProvider()
        {

        }

        public void LoadFromAssemblies(Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    _loadedTypes[type] = type.GetTypeInfo();
                }
            }
        }

        public Type[] FindTypes(Func<Type, bool> predicate)
        {
            return _loadedTypes.Keys.Where(predicate).ToArray();
        }

        public TypeInfo[] FindTypeInfos(Func<TypeInfo, bool> predicate)
        {
            return _loadedTypes.Values.Where(predicate).ToArray();
        }
    }
}
