// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using BWolf.Utilities;
using System.Collections;
using UnityEngine;

namespace BWolf.Behaviours
{
    public class ShakeBehaviour : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Tooltip("Offset of shake from start position")]
        private float strength = 1f;

        [SerializeField]
        private float speed = 1f;

        [SerializeField]
        private int count = 1;

        [SerializeField, Tooltip("update local or world position")]
        private bool local = false;

        [SerializeField, Tooltip("update of movement in fixed update or update")]
        private bool updateInFixedFrames = true;

        [SerializeField, Tooltip("axis on which to shake")]
        private Axis axis = Axis.x;

        private bool isShaking;

        private readonly WaitForFixedUpdate waitFixed = new WaitForFixedUpdate();

        [ContextMenu("Shake")]
        public void Shake()
        {
            if (!isShaking)
            {
                StartCoroutine(local ? ShakeLocal() : ShakeWorld());
            }
        }

        private IEnumerator ShakeLocal()
        {
            isShaking = true;
            PingPongValue shake = new PingPongValue(-strength, strength, count, 0.5f);
            Vector3 start = transform.localPosition;
            int axisIndex = (int)axis;

            while (shake.Continue(Time.deltaTime * speed))
            {
                Vector3 position = transform.localPosition;
                position[axisIndex] = start[axisIndex] + shake.value;
                transform.localPosition = position;
                yield return updateInFixedFrames ? waitFixed : null;
            }

            transform.localPosition = start;
            isShaking = false;
        }

        private IEnumerator ShakeWorld()
        {
            isShaking = true;
            PingPongValue shake = new PingPongValue(-strength, strength, count, 0.5f);
            Vector3 start = transform.position;
            int axisIndex = (int)axis;

            while (shake.Continue(Time.deltaTime * speed))
            {
                Vector3 position = transform.position;
                position[axisIndex] = start[axisIndex] + shake.value;
                transform.position = position;
                yield return updateInFixedFrames ? waitFixed : null;
            }

            transform.position = start;
            isShaking = false;
        }

        [SerializeField]
        private enum Axis
        {
            x = 0,
            y,
            z
        }
    }
}