using System;
using AppCore;
using AppCore.InputManagement;
using Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat.Bear {
    public class BearController : MonoBehaviour {
        [SerializeField] internal float idleWalkSpeed = 5f;
        [SerializeField] internal float swipeWalkSpeed = 2f;
        
        internal Vector2 Velocity;
        internal float WalkSpeed = 0;
        
        internal Animator Animator;
        internal Rigidbody2D Rigidbody2D;
        internal BearStateMachine StateMachine;

        private void Awake() {
            TryGetComponent(out Animator);
            TryGetComponent(out Rigidbody2D);
        }

        private void OnEnable() {
            App.Get<InputManager>().OnBearMovement += OnMovement;
        }

        private void OnDisable() {
            App.Get<InputManager>().OnBearMovement -= OnMovement;
        }
        
        private void OnMovement(Vector2 velocity) {
            Velocity = velocity;
        }

        private void Update() {
            Rigidbody2D.linearVelocity = Velocity * WalkSpeed;
            Animator.SetFloat(Constants.Animator.Bear.SpeedX, Velocity.x);
            Animator.SetFloat(Constants.Animator.Bear.SpeedY, Velocity.y);
        }
    }
}