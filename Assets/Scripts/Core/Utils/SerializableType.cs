using System;
using UnityEngine;

namespace Core.Utils
{
    [Serializable]
    public class SerializableType : ISerializationCallbackReceiver
    {
        [SerializeField] string assemblyQualifiedName = string.Empty;
        public Type Type { get; private set; }

        public void OnBeforeSerialize()
        {
            assemblyQualifiedName = Type?.AssemblyQualifiedName ?? assemblyQualifiedName;
        }

        public void OnAfterDeserialize()
        {
            if (TryGetType(assemblyQualifiedName, out var type))
            {
                Type = type;
            }
            else
            {
                Debug.LogError($"Type {assemblyQualifiedName} could not be found.");
            }
        }

        private static bool TryGetType(string typeString, out Type type)
        {
            type = Type.GetType(typeString);
            return type != null;
        }
    }
}