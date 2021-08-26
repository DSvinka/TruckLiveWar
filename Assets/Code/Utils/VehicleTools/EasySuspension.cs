using UnityEngine;

namespace Code.Utils.VehicleTools
{
    [ExecuteInEditMode]
    internal sealed class EasySuspension : MonoBehaviour
    {
        [Range(0.1f, 20f)]
        [Tooltip("Собственная частота пружин подвески. Описывает упругость подвески.")]
        [SerializeField] private float m_naturalFrequency = 10;

        [Range(0f, 3f)]
        [Tooltip("Коэффициент демпфирования пружин подвески. Описывает, как быстро пружина возвращается в исходное положение после отскока.")]
        [SerializeField] private float m_dampingRatio = 0.8f;

        [Range(-1f, 1f)]
        [Tooltip("Расстояние по оси Y до точки приложения сил подвески смещеное ниже центра масс.")]
        [SerializeField] private float m_forceShift = 0.03f;

        [Tooltip("Регулировка длинны пружин подвески в соответствии с частотой и коэффициентом демпфирования. В выключенном состоянии может вызвать нереалистичные отскоки подвески.")]
        [SerializeField] private bool m_setSuspensionDistance = true;

        private Rigidbody m_Rigidbody;

        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            foreach (var wc in GetComponentsInChildren<WheelCollider>())
            {
                var spring = wc.suspensionSpring;

                var sqrtWcSprungMass = Mathf.Sqrt(wc.sprungMass);
                spring.spring = sqrtWcSprungMass * m_naturalFrequency * sqrtWcSprungMass * m_naturalFrequency;
                spring.damper = 2f * m_dampingRatio * Mathf.Sqrt(spring.spring * wc.sprungMass);

                wc.suspensionSpring = spring;

                var wheelRelativeBody = transform.InverseTransformPoint(wc.transform.position);
                var distance = m_Rigidbody.centerOfMass.y - wheelRelativeBody.y + wc.radius;

                wc.forceAppPointDistance = distance - m_forceShift;

                if (spring.targetPosition > 0 && m_setSuspensionDistance)
                    wc.suspensionDistance = wc.sprungMass * Physics.gravity.magnitude / (spring.targetPosition * spring.spring);
            }
        }

        // Uncomment this to observe how parameters change.
        /*
        void OnGUI()
        {
            foreach (WheelCollider wc in GetComponentsInChildren<WheelCollider>()) {
                GUILayout.Label (string.Format("{0} sprung: {1}, k: {2}, d: {3}", wc.name, wc.sprungMass, wc.suspensionSpring.spring, wc.suspensionSpring.damper));
            }

            GUILayout.Label ("Inertia: " + m_Rigidbody.inertiaTensor);
            GUILayout.Label ("Mass: " + m_Rigidbody.mass);
            GUILayout.Label ("Center: " + m_Rigidbody.centerOfMass);
        }
        */

    }
}