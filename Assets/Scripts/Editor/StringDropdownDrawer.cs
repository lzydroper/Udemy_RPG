using System;
using System.Linq;
using System.Reflection;
using Core.Utils;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StringDropdownAttribute))]
public class StringDropdownDrawer : PropertyDrawer
{
    private string[] values;

    private void Initialize()
    {
        if (values != null)
            return;

        var stringDropdownAttribute = (StringDropdownAttribute)this.attribute;

        values = stringDropdownAttribute.ProviderType
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f =>
                f.FieldType == typeof(string) &&
                f.IsLiteral &&
                !f.IsInitOnly)
            .Select(f => (string)f.GetRawConstantValue())
            .ToArray();

        if (values.Length == 0)
        {
            values = new[] { "<Empty>" };
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Initialize();

        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "StringDropdown only supports string.");
            return;
        }

        var current = property.stringValue;

        int index = Array.IndexOf(values, current);

        if (index < 0)
            index = 0;

        int newIndex = EditorGUI.Popup(position, label.text, index, values);

        if (newIndex != index)
        {
            property.stringValue = values[newIndex];
        }
    }
}