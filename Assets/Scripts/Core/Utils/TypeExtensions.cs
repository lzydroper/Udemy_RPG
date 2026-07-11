using System;
using System.Linq;

namespace Core.Utils
{
    public static class TypeExtensions
    {
        public static bool IsInheritsOrImplements(this Type type, Type baseType)
        {
            // make sure the generic type would not disturb
            type = ResolveGenericType(type);
            baseType = ResolveGenericType(baseType);

            // find type from itself up to it's every parent
            while (type != typeof(object))
            {
                // if type itself or one of it's parent, is equal to baseType which means inherits,
                // or baseType is a interface and type implements baseType
                // we must use || because type might not implement baseType but its parent did
                if (type == baseType || HasAnyInterface(type, baseType)) return true;
                
                type = ResolveGenericType(type.BaseType);
                if (type == null) return false;
            }
            
            return false;
        }

        // make the type if it is a genericType return it's original version
        // something like List<int> => List<T>
        private static Type ResolveGenericType(Type type)
        {
            // pattern match can resolve the null problem, at least the rider won't warn again
            if (type is not { IsGenericType: true }) return type;
            var genericType = type.GetGenericTypeDefinition();
            // it may get some wrong if we GetGenericTypeDefinition() to a open one like List<T>
            // so if the same, we try to use the original one to avoid problem
            return genericType != type ? genericType : type;
        }

        private static bool HasAnyInterface(Type type, Type interfaceType)
        {
            return type.GetInterfaces().Any(i => ResolveGenericType(i) == interfaceType);
        }
    }
}