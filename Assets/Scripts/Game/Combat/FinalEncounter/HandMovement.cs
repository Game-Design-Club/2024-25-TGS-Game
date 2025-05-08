using System;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Combat.FinalEncounter {
    public class HandMovement : MonoBehaviour {
        [SerializeField] private float moveIntensity = 1f;
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float speedRandomization = .5f;
        [SerializeField] private bool randomizeCycle = false;

        private float _startingRotation;
        private float _randomOffset;

        private void Awake() {
            _startingRotation = transform.rotation.eulerAngles.z;
            _randomOffset = randomizeCycle ? UnityEngine.Random.Range(0, 360) : 0;
            speedRandomization = UnityEngine.Random.Range(-speedRandomization, speedRandomization);
        }

        private void Update() {
            transform.rotation = quaternion.Euler(0, 0, _startingRotation + Mathf.Sin(Time.time * moveSpeed * speedRandomization + _randomOffset) * moveIntensity);
        }
    }
}