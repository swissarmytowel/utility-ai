using UnityEditor;
using UnityEngine;
using UtilityAI_Base.Intellect;
using UtilityAI_Base.Selectors.Factories;

namespace UtilityAI_Base.Editor
{
    [CustomEditor(typeof(NpcIntellect))]
    public class IntellectInspector : UnityEditor.Editor
    {
        private static readonly string[] DontIncludeMe = new string[] {"m_Script"};

        private NpcIntellect Intellect => target as NpcIntellect;

        private void DrawProperties() {
            Intellect.UpdateTimesPerSecond =
                EditorGUILayout.FloatField(new GUIContent("Updates p/s"), Intellect.UpdateTimesPerSecond);
            EditorGUILayout.Separator();
            
            Intellect.actionSelectorType =
                (ActionSelectorType) EditorGUILayout.EnumPopup(new GUIContent("Action Selector"), 
                    Intellect.actionSelectorType);
            EditorGUILayout.Separator();

            Intellect.fallbackSelectorType =
                (ActionSelectorType) EditorGUILayout.EnumPopup(new GUIContent("Fallback Action Selector"), 
                    Intellect.fallbackSelectorType);
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            
            DrawProperties();

            EditorGUILayout.Separator();
            
            DrawPropertiesExcluding(serializedObject, DontIncludeMe);

            serializedObject.ApplyModifiedProperties();
        }
    }
}