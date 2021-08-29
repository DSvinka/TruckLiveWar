using UnityEditor;
using UnityEngine;
using Code.Utils.VehicleTools.Debug;

namespace Code.Utils.VehicleTools.Editor
{
    [CustomEditor(typeof(GraphOverlay))]
    internal sealed class GraphOverlayEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var myTarget = (GraphOverlay) target;

            myTarget.vehicleBody = (Rigidbody) EditorGUILayout.ObjectField("Транспорт", myTarget.vehicleBody, typeof(Rigidbody), true);

            if (!myTarget.vehicleBody)
                return;

            myTarget.timeTravel = EditorGUILayout.FloatField("Отображаемый в диограмме промежуток времени", myTarget.timeTravel);

            myTarget.width = EditorGUILayout.Slider("Ширина", myTarget.width, 0, 1);
            myTarget.height = EditorGUILayout.Slider("Высота", myTarget.height, 0, 1);

            myTarget.widthSeconds = EditorGUILayout.FloatField("Ширина в секундах", myTarget.widthSeconds);
            myTarget.heightMeters = EditorGUILayout.FloatField("Высота в метрах", myTarget.heightMeters);

            myTarget.bgColor = EditorGUILayout.ColorField("Цвет фона", myTarget.bgColor);
            myTarget.forwardColor = EditorGUILayout.ColorField("Цвет передвижения", myTarget.forwardColor);
            myTarget.sidewaysColor = EditorGUILayout.ColorField("Цвет дрифта", myTarget.sidewaysColor);
            myTarget.zeroColor = EditorGUILayout.ColorField("Цвет нулевого значения", myTarget.zeroColor);

            if (myTarget.vehicleBody)
            {
                foreach (var wheelConfig in myTarget.wheelConfigs)
                {
                    EditorGUILayout.LabelField(wheelConfig.collider.name);
                    wheelConfig.visible = EditorGUILayout.Toggle("Выключатель", wheelConfig.visible);
                }
            }
        }
    }
}