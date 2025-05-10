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
        private bool isShifting = false;
        private Vector2 shiftPos = Vector2.zero;
        private float shiftingStarted;
        [SerializeField] private float shiftTime = 0.25f;

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
            if (isShifting) return;
            if (isRolling) return;

            direction /= Mathf.Abs(direction);

            if (ColliderInDirection(direction, !vertical)) return;

            isShifting = true;
            shiftingStarted = Time.time;
            shiftPos = (Vector2)transform.position + new Vector2(vertical ? transform.localScale.x * direction : 0, !vertical ? transform.localScale.x * direction : 0);
        }
        
        public void Roll(float direction)
        {
            if (isShifting) return;
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

        private float TimeRolling()
        {
            return isRolling ? Time.time - rollingStarted : 0;
        }
        
        private void Update()
        {
            var position = transform.position;
            if (isRolling)
            {
                float velocity = speedRamp.Evaluate(TimeRolling()) * maxSpeed * direction * Time.deltaTime;
                position += new Vector3(vertical ? 0 : velocity, vertical ? velocity : 0, 0);
            }else if (isShifting)
            {
                float shiftPercent = (Time.time - shiftingStarted) / shiftTime;
                position = Vector2.Lerp(position, shiftPos, shiftPercent);

                if (shiftPercent > 1)
                {
                    isShifting = false;
                    position = shiftPos;
                }
            }
            
            transform.position = position;
        }

        private void Stop()
        {
            isRolling = false;
            direction = 0;
            isShifting = false;
        }

        private void HayVsHay(HayBale bale)
        {
            // Debug.Log("HAY");
            if (TimeRolling() == 0) return;
            // Debug.Log($"ROL: {bale.gameObject.transform.position - gameObject.transform.position}");
            bale.RealHit(bale.gameObject.transform.position - gameObject.transform.position);
            Stop();
        }

        private void HayVsCow(Cow cow)
        {
            cow.Tip();
            Stop();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer != 8 && other.gameObject.layer != 9) return;

            if (other.gameObject.TryGetComponent(out Corn corn)) corn.Squish(new Vector2(vertical ? 0 : direction, vertical ? direction : 0));
            else if (other.gameObject.TryGetComponent(out HayBale hay)) HayVsHay(hay);
            else if (other.gameObject.TryGetComponent(out Cow cow)) HayVsCow(cow);
            else Stop();
        }
        
        

        // private void OnCollisionStay2D(Collision2D other)
        // {
        //     if (!isRolling) return;
        //     if (other.gameObject.TryGetComponent(out HayBale hay)) HayVsHay(hay);
        // }
        
        //TODO right next to work
        //TODO add shake
        //TODO add sound

        public void RealHit(Vector2 hitDirection)
        {
            // String s = "HIt:\n";
            bool xIsGreaterThanY = Mathf.Abs(hitDirection.x) > Mathf.Abs(hitDirection.y);
            // s += $"X>Y: {xIsGreaterThanY} -> {hitDirection} V: {vertical}\n";

            if (xIsGreaterThanY != vertical)
            {
                Roll(vertical ? hitDirection.y : hitDirection.x);
                // s += "Roll\n";
            }
            else
            {
                Shift(vertical ? hitDirection.x : hitDirection.y);
                // s += "Shift\n";
            }
            // Debug.Log(s);
        }

        public void Hit(Vector2 hitDirection)
        {
            hitDirection = LevelManager.GetCurrentLevel().child.LastDirection;
            RealHit(hitDirection);
        }
        
    }
}