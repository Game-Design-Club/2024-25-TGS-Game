using System;
using UnityEngine;

namespace Tools {
    public class TimeScaleChanger : MonoBehaviour {
        [SerializeField] private bool active = false;
        [SerializeField] private float timeScale = 1f;

        private void Update() {
            Time.timeScale = timeScale;
        }
    }
}