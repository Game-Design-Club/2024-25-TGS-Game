using System;
using AppCore;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Exploration.Enviornment.Farm
{
    public class HayBale : MonoBehaviour
    {
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
        
        public void Roll(float direction)
        {
            if (isRolling) return;
            isRolling = true;
            this.direction = Mathf.Clamp(direction, -1, 1);
            rollingStarted = Time.time;
        }
        private void Update()
        {
            if (!isRolling) return;
            float velocity = speedRamp.Evaluate(timeRolling) * maxSpeed * direction * Time.deltaTime;
            transform.position += new Vector3(vertical ? velocity : 0, vertical ? 0 : velocity, 0);
        }

        private void Stop()
        {
            isRolling = false;
            direction = 0;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("H");
            Corn corn = other.gameObject.GetComponent<Corn>();
            Debug.Log(corn);
            if (corn != null) corn.Squish();
        }
    }
}