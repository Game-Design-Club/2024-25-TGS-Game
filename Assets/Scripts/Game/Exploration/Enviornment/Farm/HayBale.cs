using System;
using AppCore;
using Game.Exploration.Child;
using Game.GameManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Exploration.Enviornment.Farm
{
    public class HayBale : MonoBehaviour, IChildHittable
    {
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private bool vertical = true;
        private bool isRolling = false;
        private float direction = 0;
        [SerializeField] private float maxSpeed = 0;
        [SerializeField] private AnimationCurve speedRamp = new AnimationCurve();
        private float rollingStarted;
        private float timeRolling => Time.time - rollingStarted;


        private void OnValidate()
        {
            transform.rotation = Quaternion.Euler(0, 0, vertical ? 0 : 90);
        }

        [ContextMenu("Roll Negative")]
        public void RollN()
        {
            Roll(-1);
        }
        
        [ContextMenu("Roll Positive")]
        public void RollP()
        {
            Roll(1);
        }

        private void Shift(float direction)
        {
            if (isRolling) return;

            direction /= Mathf.Abs(direction);

            if (ColliderInDirection(direction, !vertical)) return;
            
            transform.position += new Vector3(vertical ? transform.localScale.x * direction : 0, !vertical ? transform.localScale.x * direction : 0, 0);
        }
        
        public void Roll(float direction)
        {
            if (isRolling) return;
            
            direction /= Mathf.Abs(direction);

            if (ColliderInDirection(direction, vertical)) return;
                
            isRolling = true;
            this.direction = direction;
            rollingStarted = Time.time;
        }

        private bool ColliderInDirection(float direction, bool vertical)
        {
            Collider2D[] colliders = new Collider2D[100];
            int num = body.GetContacts(colliders);

            for (int i = 0; i < num; i++)
            {
                Collider2D collider = colliders[i];
                if (collider.gameObject.TryGetComponent(out Corn corn)) continue;
                
                Vector2 dir = collider.gameObject.transform.position - transform.position;
                float colliderDirection = vertical ? dir.y : dir.x;
                colliderDirection /= Mathf.Abs(colliderDirection);
                if (Math.Abs(colliderDirection - direction) < 0.1f) return true;
            }

            return false;
        }
        
        private void Update()
        {
            if (!isRolling) return;
            float velocity = speedRamp.Evaluate(timeRolling) * maxSpeed * direction * Time.deltaTime;
            transform.position += new Vector3(vertical ? 0 : velocity, vertical ? velocity : 0, 0);
        }

        private void Stop()
        {
            isRolling = false;
            direction = 0;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer != 8) return;

            if (other.gameObject.TryGetComponent(out Corn corn)) corn.Squish();
            else Stop();
        }

        public void Hit(Vector2 hitDirection)
        {
            bool xIsGreaterThanY = Mathf.Abs(hitDirection.x) > Mathf.Abs(hitDirection.y);
            
            if (xIsGreaterThanY != vertical) Roll(vertical ? hitDirection.y : hitDirection.x);
            else
            {
                Shift(vertical ? hitDirection.x : hitDirection.y);
            }
        }
    }
}