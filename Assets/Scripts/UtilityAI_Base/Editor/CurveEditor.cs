using UnityEditor;
using UnityEngine;
using UtilityAI_Base.ResponseCurves;

namespace UtilityAI_Base.Editor
{
    public class CurveEditor : EditorWindow
    {
        private static ResponseCurve _currentCurve;

        public static void ShowWindow() {
            EditorWindow.GetWindow<CurveEditor>().Show();
        }

        public static void Open(ResponseCurve curve) {
            _currentCurve = curve;
            ShowWindow();
        }

        private void OnGUI() {
            if (_currentCurve.responseCurveType != CurveType.Animation) {
                OnSuppliedCurveGui();
            }
        }
        
        private void OnSuppliedCurveGui() {
            var horizontalSplit = position.width - position.width / 3f;
            var rectOutline = new Rect(0, 0, horizontalSplit, position.height);
            var rect = new Rect(10, 10, horizontalSplit - 20, position.height - 20);

            EditorGUILayout.BeginHorizontal();

            DrawAxis(rectOutline, rect);

            if (_currentCurve != null) {
                DrawCurve(rect);
                var lhsRect = new Rect(horizontalSplit + 10, 10, position.width / 3f - 20,
                    EditorGUIUtility.singleLineHeight);

                EditorGUI.BeginChangeCheck();

                DrawLeftSideSliders(lhsRect);

                EditorGUI.EndChangeCheck();
            }

            EditorGUILayout.EndHorizontal();
        }
        
        
        private void DrawAxis(Rect outline, Rect rect) {
            EditorGUI.DrawRect(outline, Color.black);
            EditorGUI.DrawRect(rect, Color.black);

            var start = new Vector3(0f, rect.y);
            var end = new Vector3(0f, rect.height + 10);
            for (var i = 0f; i < rect.width; i += rect.width / 15f) {
                start.x = i;
                end.x = i;
                Handles.DrawLine(start, end);
            }
            start = new Vector3(rect.x, 0);
            end = new Vector3(rect.width + 10, 0);
            for (var i = 0f; i < rect.height; i += rect.height / 15f) {
                start.y = i;
                end.y = i;
                Handles.DrawLine(start, end);
            }
        }

        private void DrawLines(Rect rect, Vector3 start, Vector3 end) {
            for (var i = 0f; i < rect.width; i += rect.width / 15f) {
                start.y = i;
                end.y = i;
                Handles.DrawLine(start, end);
            }
        }


        private void DrawLeftSideSliders(Rect lhsRect) {
            EditorGUI.LabelField(lhsRect, new GUIContent("Slope"));
            lhsRect.y += EditorGUIUtility.singleLineHeight;
            _currentCurve.slope.Value = EditorGUI.Slider(lhsRect, _currentCurve.slope.Value,
                _currentCurve.slope.MinValue, _currentCurve.slope.MaxValue);
            lhsRect.y += 2 * EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(lhsRect, new GUIContent("Exponent"));
            lhsRect.y += EditorGUIUtility.singleLineHeight;

            _currentCurve.exponent.Value = EditorGUI.Slider(lhsRect, _currentCurve.exponent.Value,
                _currentCurve.exponent.MinValue, _currentCurve.exponent.MaxValue);

            lhsRect.y += 2 * EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(lhsRect, new GUIContent("HShift"));
            lhsRect.y += EditorGUIUtility.singleLineHeight;

            _currentCurve.horizontalShift.Value = EditorGUI.Slider(lhsRect, _currentCurve.horizontalShift.Value,
                _currentCurve.horizontalShift.MinValue, _currentCurve.horizontalShift.MaxValue);

            lhsRect.y += 2 * EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(lhsRect, new GUIContent("VShift"));
            lhsRect.y += EditorGUIUtility.singleLineHeight;

            _currentCurve.verticalShift.Value = EditorGUI.Slider(lhsRect, _currentCurve.verticalShift.Value,
                _currentCurve.verticalShift.MinValue, _currentCurve.verticalShift.MaxValue);
            lhsRect.y += 2 * EditorGUIUtility.singleLineHeight;

            if (GUI.Button(lhsRect, "default")) {
                _currentCurve.SetDefaults();
            }
        }

        private void DrawCurve(Rect rect) {
            const float minYAxes = 0;
            const float maxYAxes = 1;

            var step = 1 / position.width;

            var previousPosition = new Vector3(0, _currentCurve.CurveFunction(0), 0);
            for (var x = step; x < 1; x += step) {
                var pos = new Vector3(x, _currentCurve.CurveFunction(x), 0);
                var start = new Vector3(rect.xMin + previousPosition.x * rect.width,
                    rect.yMax - ((previousPosition.y - minYAxes) / (maxYAxes - minYAxes)) * rect.height, 0);
                var end = new Vector3(rect.xMin + pos.x * rect.width,
                    rect.yMax - ((pos.y - minYAxes) / (maxYAxes - minYAxes)) * rect.height, 0);

                Handles.DrawBezier(start, end, start, end, Color.green, null, 10);

                previousPosition = pos;
            }
        }
    }
}