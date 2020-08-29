using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tinker
{
    public class Touch
    {
        public int fingerId { get; set; }
        public Vector2 position { get; set; }
        public Vector2 rawPosition { get; set; }
        public Vector2 deltaPosition { get; set; }
        public float deltaPositionMagnitude { get; set; }
        public float deltaTime { get; set; }
        public int tapCount { get; set; }
        public TouchPhase touchPhase { get; set; }
        public float pressure { get; set; }
        public float maximumPossiblePressure { get; set; }
        public TouchType type { get; set; }
        public float altitudeAngle { get; set; }
        public float azimuthAngle { get; set; }
        public float radius { get; set; }
        public float radiusVariance { get; set; }
        public string debugMessage { get; set; }
    }

}