using System;
using UnityEngine;

namespace Core.Utils
{
    /// <summary>
    /// 让 string 在 Inspector 中显示为下拉框，
    /// 可选择指定 static 类中的所有 public const string。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class StringDropdownAttribute : PropertyAttribute
    {
        public Type ProviderType { get; }

        public StringDropdownAttribute(Type providerType)
        {
            ProviderType = providerType;
        }
    }
}