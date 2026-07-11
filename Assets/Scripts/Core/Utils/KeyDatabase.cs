using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Utils
{
    [CreateAssetMenu(fileName = "KeyDatabase", menuName = "Data/KeyDatabase", order = 0)]
    public class KeyDatabase : ScriptableObject
    {
        public string OutputPath =
            "Assets/Scripts/Generated/Key.cs";

        public string Namespace =
            "Generated";
        public List<KeyCategory> categories = new();
    }
    
    [Serializable]
    public class KeyCategory
    {
        public string category;
        public List<string> keys = new();
    }
}