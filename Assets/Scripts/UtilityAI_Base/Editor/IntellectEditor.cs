using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UtilityAI_Base.Intellect;
using UtilityAI_Base.Selectors.Factories;

namespace UtilityAI_Base.Editor
{
    [CustomEditor(typeof(NpcIntellect))]
    public class IntellectEditor : UnityEditor.Editor
    {
        private NpcIntellect Intellect => target as NpcIntellect;

        private ReorderableList _actions;
        private ReorderableList _fallbackActions;

        private void ActionListDrawCallback(Rect rect, int index, bool isactive, bool isfocused) {
            var consideration = _actions.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.BeginChangeCheck();

            EditorGUI.PropertyField(rect, consideration);

            if (EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(target);
            }
        }

        private void OnEnable() {
            if (_actions == null) {
                _actions = new ReorderableList(serializedObject, serializedObject.FindProperty("actions"),
                    true, true, true, true)
                {
                    drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "Actions", EditorStyles.boldLabel); },
                    elementHeight = EditorGUIUtility.singleLineHeight,
                    onAddCallback = AddActionItem,
                    onRemoveCallback = RemoveActionItem,
                    drawElementCallback = ActionListDrawCallback
                };
            }
            
            // if (_fallbackActions == null) {
            //     _fallbackActions = new ReorderableList(serializedObject, serializedObject.FindProperty("fallBackActions"),
            //         true, true, true, true)
            //     {
            //         drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "FallbackActions", EditorStyles.boldLabel); },
            //         elementHeight = EditorGUIUtility.singleLineHeight,
            //         onAddCallback = AddFallbackActionItem,
            //         onRemoveCallback = RemoveFallbackActionItem,
            //         drawElementCallback = ActionListDrawCallback
            //     };
            // }
        }

        private void RemoveActionItem(ReorderableList list) {
            Debug.Log(Intellect.actions.Count);
            Intellect.actions.RemoveAt(list.index);
            EditorUtility.SetDirty(target);
            Debug.Log(Intellect.actions.Count);
        }

        private void AddActionItem(ReorderableList list) {
            Intellect.actions.Add(null);
            EditorUtility.SetDirty(target);
        }
        
        private void RemoveFallbackActionItem(ReorderableList list) {
            Intellect.fallBackActions.RemoveAt(list.index);
        }

        private void AddFallbackActionItem(ReorderableList list) {
            Intellect.fallBackActions.Add(null);
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            
            
            EditorGUILayout.Separator();
            Intellect.UpdateTimesPerSecond = EditorGUILayout.FloatField("Updates P/s", Intellect.UpdateTimesPerSecond);
            EditorGUILayout.Separator();
            
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Action Selector");
            Intellect.actionSelectorType = (ActionSelectorType)EditorGUILayout.EnumPopup(Intellect.actionSelectorType);
            EditorGUILayout.EndHorizontal();
            
            if (EditorGUI.EndChangeCheck()) {
                Intellect.selector = ActionSelectorFactory.GetSelector(Intellect.actionSelectorType);
                EditorUtility.SetDirty(target);
            }

            _actions.DoLayoutList();
            EditorGUILayout.Separator();
            
            // EditorGUI.BeginChangeCheck();
            //
            // EditorGUILayout.BeginHorizontal();
            // EditorGUILayout.PrefixLabel("Fallback Action Selector");
            // Intellect.fallbackSelectorType = (ActionSelectorType)EditorGUILayout.EnumPopup(Intellect.fallbackSelectorType);
            // EditorGUILayout.EndHorizontal();
            //             
            // if (EditorGUI.EndChangeCheck()) {
            //     Intellect.fallbackSelector = ActionSelectorFactory.GetSelector(Intellect.fallbackSelectorType);
            //     EditorUtility.SetDirty(target);
            // }
            //
            //
            // _fallbackActions.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}