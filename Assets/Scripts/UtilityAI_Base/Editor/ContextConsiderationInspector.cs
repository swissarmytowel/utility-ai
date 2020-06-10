using UnityEditor;
using UnityEngine;
using UtilityAI_Base.Considerations;


namespace UtilityAI_Base.Editor
{
    [CustomPropertyDrawer(typeof(ContextConsideration))]
    public class ContextConsiderationInspector : PropertyDrawer
    {
        private static readonly float VerticalSpacing = 2 * EditorGUIUtility.singleLineHeight;

        private void DisplayHeadLine(Rect position, SerializedProperty property, GUIContent label) {
            var quarterW = EditorGUIUtility.currentViewWidth / 4;
            var rect = new Rect(position.x, position.y, 10, EditorGUIUtility.singleLineHeight);
            
            var isEnabled = property.FindPropertyRelative("isEnabled").boolValue;
            property.FindPropertyRelative("isEnabled").boolValue = EditorGUI.Toggle(
                rect, GUIContent.none, isEnabled);
            rect.x += 25;
            rect.width =position.width - 1.5f * quarterW; 
            property.FindPropertyRelative("description").stringValue = EditorGUI.TextField(
                rect, property.FindPropertyRelative("description").stringValue);

            rect.x += rect.width + 10;
            rect.width = 100;
            EditorGUI.LabelField(rect,new GUIContent("Can Apply Veto"));
            
            rect.x += 100;
            rect.width = 10;
            property.FindPropertyRelative("canApplyVeto").boolValue = EditorGUI.Toggle(
                rect, property.FindPropertyRelative("canApplyVeto").boolValue);
            
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var quarterW = EditorGUIUtility.currentViewWidth / 4;
            position.x += 10;

            EditorGUI.BeginProperty(position, label, property);
            
            DisplayHeadLine(position, property, label);
            
            position.y += VerticalSpacing;
   
            EditorGUI.PropertyField(new Rect(position.x, position.y,
                    position.width,
                    EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("valueRange"),
                new GUIContent("Value normalization range"));

            EditorGUI.EndProperty();
        }
    }
}