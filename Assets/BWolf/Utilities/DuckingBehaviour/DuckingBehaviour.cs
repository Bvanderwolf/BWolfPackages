// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using BWolf.Utilities.LerpValue;
using UnityEngine;

namespace BWolf.Utilities.DuckingBehaviour
{
    public class DuckingBehaviour : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField, Tooltip("keyboard key to duck")]
        private KeyCode key = KeyCode.LeftControl;

        [Header("Settings")]
        [SerializeField, Tooltip("time from start position to lowest position")]
        private float duckTime = 0.5f;

        [SerializeField, Tooltip("curve aplied to duck movement. default is no curve")]
        private Curve curve = Curve.Default;

        [SerializeField, Tooltip("update of movement in fixed update or update")]
        private bool updateInFixedFrames = true;

        [Header("References")]
        [SerializeField, Tooltip("transform of object to move. if this is null it will use this objects transform")]
        private Transform duckTransform = null;

        [SerializeField, Tooltip("offset of start position downwards to duck")]
        private float duckOffset = 1.25f;

        private Vector3 duckOffsetVector;
        private Vector3 duckOffsetStart;
        private Vector3 startLocalPosition;

        private LerpValue<Vector3> move;
        private LerpSetting duckSetting;

        private void Start()
        {
            if (duckTransform == null)
            {
                duckTransform = transform;
            }

            switch (curve)
            {
                case Curve.Default:
                    duckSetting = LerpSettings.Default;
                    break;

                case Curve.Sine:
                    duckSetting = LerpSettings.Sine;
                    break;

                case Curve.Cosine:
                    duckSetting = LerpSettings.Cosine;
                    break;
            }

            duckOffsetVector = duckTransform.localPosition + Vector3.down * duckOffset;
            startLocalPosition = duckTransform.localPosition;
        }

        private void Update()
        {
            bool duck = Input.GetKeyDown(key);
            bool stand = Input.GetKeyUp(key);
            if (duck || stand)
            {
                //start new lerp movement when ducking is started on stopped
                duckOffsetStart = duckTransform.localPosition;
                move = new LerpValue<Vector3>(duckOffsetStart, duck ? duckOffsetVector : startLocalPosition, duckTime, duckSetting);
            }

            if (!updateInFixedFrames)
            {
                Duck();
            }
        }

        private void FixedUpdate()
        {
            if (updateInFixedFrames)
            {
                Duck();
            }
        }

        private void Duck()
        {
            if (duckOffsetStart != Vector3.zero)
            {
                //if duckoffset is not zero, continue moving towards end position
                if (move.Continue(Time.deltaTime))
                {
                    duckTransform.localPosition = Vector3.Lerp(move.Start, move.End, move.Perc);
                }

                if (duckTransform.localPosition == startLocalPosition)
                {
                    //if the localposition of the object is back to start, set duckofsetstart to zero
                    duckOffsetStart = Vector3.zero;
                }
            }
        }

        [System.Serializable]
        private enum Curve
        {
            Default,
            Sine,
            Cosine
        }
    }
}