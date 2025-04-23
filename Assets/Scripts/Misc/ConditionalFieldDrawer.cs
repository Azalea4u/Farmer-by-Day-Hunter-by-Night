#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
public class ConditionalFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditional.FieldToCheck);

        if (conditionProperty != null && conditionProperty.boolValue == conditional.DesiredValue)
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditional.FieldToCheck);

        if (conditionProperty != null && conditionProperty.boolValue == conditional.DesiredValue)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        return 0; // Hide the field
    }
}
#endif
