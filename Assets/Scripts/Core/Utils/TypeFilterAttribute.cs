using System;
using UnityEngine;

namespace Core.Utils
{
    public class TypeFilterAttribute : PropertyAttribute
    {
        public Func<Type, bool> Filter { get; }

        public TypeFilterAttribute(Type typeFilter)
        {
            // we need to filter the type that can be instanced and is
            // truly inheriting or implementing from the give type
            Filter = type => !type.IsAbstract &&
                             !type.IsInterface &&
                             !type.IsGenericType &&
                             type.IsInheritsOrImplements(typeFilter);
        }
    }
}