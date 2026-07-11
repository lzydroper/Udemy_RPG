using System;
using System.Linq;
using Core.Utils;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializableType))]
public class SerializableTypeDrawer : PropertyDrawer
{
    TypeFilterAttribute typeFilter;
    string[] typeNames, typeFullNames;

    void Initialize()
    {
        if (typeFullNames != null) return;
        
        typeFilter = (TypeFilterAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(TypeFilterAttribute));
        var filteredTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeFilter == null ? DefaultFilter(type) : typeFilter.Filter(type))
            .ToArray();
        typeNames = filteredTypes
            .Select(type => type.ReflectedType == null ? type.Name : $"type.ReflectedType.Name + type.Name").ToArray();
        typeFullNames = filteredTypes.Select(type => type.AssemblyQualifiedName).ToArray();
    }

    private static bool DefaultFilter(Type type)
    {
        return !type.IsAbstract && !type.IsInterface && !type.IsGenericType;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Initialize();
        var typeIdProperty = property.FindPropertyRelative("assemblyQualifiedName");

        if (string.IsNullOrEmpty(typeIdProperty.stringValue))
        {
            typeIdProperty.stringValue = typeFullNames.First();
            property.serializedObject.ApplyModifiedProperties();
        }
        
        var currentIndex = Array.IndexOf(typeFullNames, typeIdProperty.stringValue);
        var selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, typeNames);

        if (selectedIndex >= 0 & selectedIndex != currentIndex)
        {
            typeIdProperty.stringValue = typeFullNames[selectedIndex];
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}