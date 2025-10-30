using UnityEngine;

namespace NaughtyCharacter
{
	public class PlayerCamera : MonoBehaviour
    {
        [Header("Settings")]
        public float ControlRotationSensitivity = 2f;
        public float FollowSpeed = 10f;
        public Vector2 PitchMinMax = new Vector2(-35, 70);

        [Header("References")]
        public Transform Rig;     
        public Transform Pivot;   
        public Transform Target;  
        public Camera Camera;

        private Vector2 controlRotation;

        private void LateUpdate()
        {
            if (!Target) return;

            Vector3 targetPos = Vector3.Lerp(Rig.position, Target.position, FollowSpeed * Time.deltaTime);
            Rig.position = targetPos;

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            controlRotation.y += mouseX * ControlRotationSensitivity;
            controlRotation.x -= mouseY * ControlRotationSensitivity;
            controlRotation.x = Mathf.Clamp(controlRotation.x, PitchMinMax.x, PitchMinMax.y);

            Rig.localRotation = Quaternion.Euler(0, controlRotation.y, 0);
            Pivot.localRotation = Quaternion.Euler(controlRotation.x, 0, 0);
        }
    }
}