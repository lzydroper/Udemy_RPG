using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Utils;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StringDropdownAttribute))]
public class StringDropdownDrawer : PropertyDrawer
{
    private string[] values;
    private string[] displayValues;

    private void Initialize()
    {
        if (values != null)
            return;

        var stringDropdownAttribute = (StringDropdownAttribute)this.attribute;

        var list = stringDropdownAttribute.ProviderType
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f =>
                f.FieldType == typeof(string) &&
                f.IsLiteral &&
                !f.IsInitOnly)
            .Select(f => (string)f.GetRawConstantValue())
            .ToList();

        // 默认插入一个空值选项，在下拉框中显示为 "<None>"，对应底层的 string.Empty
        list.Insert(0, string.Empty);
        values = list.ToArray();
        displayValues = list.Select(v => string.IsNullOrEmpty(v) ? "<None>" : v).ToArray();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Initialize();

        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "StringDropdown only supports string.");
            return;
        }

        var current = property.stringValue ?? string.Empty;

        int index = Array.IndexOf(values, current);

        // 如果在列表中找不到当前值（例如被删除了），默认选中 "<None>"
        if (index < 0)
            index = 0;

        int newIndex = EditorGUI.Popup(position, label.text, index, displayValues);

        if (newIndex != index || property.stringValue != values[newIndex])
        {
            property.stringValue = values[newIndex];
        }
    }
}