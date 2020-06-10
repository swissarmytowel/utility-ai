using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Selectors;
using UtilityAI_Base.Selectors.Factories;

namespace UtilityAI_Base.Editor
{
    [CustomEditor(typeof(AtomicUtilityAction))]
    public class AtomicUtilityActionInspector : UnityEditor.Editor
    {
        private ReorderableList _considerationsDisplay;
        private static readonly float VerticalSpacing = 2 * EditorGUIUtility.singleLineHeight;
        
        private AtomicUtilityAction SelectedAction => target as AtomicUtilityAction;

        private void SetConsiderationsListDrawCallback() {
            _considerationsDisplay.drawElementCallback = ConsiderationsListDrawCallback;
        }

        private void ConsiderationsListDrawCallback(Rect rect, int index, bool isactive, bool isfocused) {
            var consideration = _considerationsDisplay.serializedProperty.GetArrayElementAtIndex(index);
            var quarterW = EditorGUIUtility.currentViewWidth / 4;

            EditorGUI.BeginChangeCheck();

            EditorGUI.PropertyField(rect, consideration);

            var r  = EditorGUI.PrefixLabel(
                new Rect(rect.x + 10, rect.y + 2 * VerticalSpacing, rect.width - quarterW, 2 * EditorGUIUtility.singleLineHeight),
                new GUIContent("Utility Curve"));
            SelectedAction.considerations[index].utilityCurve = EditorGUI.CurveField(
                r,
                SelectedAction.considerations[index].utilityCurve);
            EditorUtility.SetDirty(target);

            SelectedAction.considerations[index].evaluatedContextVariable = (AiContextVariable)EditorGUI.EnumPopup(
                new Rect(rect.x + 10, rect.y + 3.2f * VerticalSpacing, rect.width - quarterW,
                    EditorGUIUtility.singleLineHeight), new GUIContent("Target Variable"),
                SelectedAction.considerations[index].evaluatedContextVariable);
            if(EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(target);
        }

        private void OnEnable() {
            if (_considerationsDisplay == null) {
                _considerationsDisplay = new ReorderableList(serializedObject,
                    serializedObject.FindProperty("considerations"), true, true,
                    true, true)
                {
                    drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "", EditorStyles.boldLabel); },
                    elementHeight =  EditorGUIUtility.singleLineHeight * 8f,
                    onAddCallback = AddItem,
                    onRemoveCallback = RemoveItem,
                    headerHeight = 10
                };
                SetConsiderationsListDrawCallback();
            }
        }
        
        private void AddItem(ReorderableList list) {
            SelectedAction.considerations.Add(new ContextConsideration());
            EditorUtility.SetDirty(target);
        }

        private void RemoveItem(ReorderableList list) {
            SelectedAction.considerations.RemoveAt(list.index);
            EditorUtility.SetDirty(target);
        }

        private void ShowProperties() {
            EditorGUILayout.LabelField(new GUIContent("Utility Action Parameters:"), EditorStyles.boldLabel);
            EditorGUILayout.Separator();

            EditorGUI.BeginChangeCheck();
            SelectedAction.qualifierType =
                (QualifierType) EditorGUILayout.EnumPopup(new GUIContent("Considerations Qualifier"),
                    SelectedAction.qualifierType);

            if (EditorGUI.EndChangeCheck()) {
                SelectedAction.qualifier =
                    ConsiderationsQualifierFactory.GetQualifier(SelectedAction.qualifierType);
                EditorUtility.SetDirty(target);
            }
            
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();

            SelectedAction.CooldownTime = Mathf.Clamp(
                EditorGUILayout.FloatField(new GUIContent("Cooldown (s)"), SelectedAction.CooldownTime),
                0f,
                100f);
            EditorGUILayout.Separator();

            SelectedAction.InertiaTime = Mathf.Clamp(
                EditorGUILayout.FloatField(new GUIContent("Inertia time (s)"), SelectedAction.InertiaTime),
                0f,
                100f);
            EditorGUILayout.Separator();
            
            SelectedAction.ActionWeight =
                EditorGUILayout.FloatField(new GUIContent("Weight"), SelectedAction.ActionWeight);

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField(new GUIContent("Action To Be Performed:"), EditorStyles.boldLabel);
            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("actionTask"));
            if (EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(target);
            }
        }

        public void ShowDescription() {
            EditorGUILayout.LabelField(new GUIContent("Description:"), EditorStyles.boldLabel);
            EditorGUILayout.Separator();
            SelectedAction.description = EditorGUILayout.TextArea(SelectedAction.description, GUILayout.Height(100));
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            ShowProperties();
            EditorGUILayout.LabelField(new GUIContent("Considerations List:"), EditorStyles.boldLabel);
            EditorGUILayout.Separator();

            _considerationsDisplay.DoLayoutList();

            ShowDescription();
            serializedObject.ApplyModifiedProperties();
        }
    }
}