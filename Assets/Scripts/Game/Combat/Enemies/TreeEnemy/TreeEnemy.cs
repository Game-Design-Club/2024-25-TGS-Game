using System.Collections.Generic;
using Game.Combat.Bear;
using Tools;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Combat.Enemies.TreeEnemy {
    public class TreeEnemy : EnemyBase {
        [SerializeField] internal float damageSpeed = .1f;
        [SerializeField] internal float reachSpeed = 5f;
        [SerializeField] private FloatRange reachTurbulenceDistance = new(0.5f, 1.5f);
        [SerializeField] private float reachTurbulenceAngle = 10f;
        [SerializeField] private float childRange = 1f;
        [SerializeField] internal float dieRange = 0.5f;
        [SerializeField] internal float attackRange = 0.5f;
        [FormerlySerializedAs("maxSegments")] [SerializeField] private int maxChunks = 10;
        [SerializeField] private int chunksPerSegment = 5;
        [SerializeField] private LineRenderer debugLineRenderer;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform hand;
        [SerializeField] private Transform spritePivot;

        protected override EnemyState StartingState => new Reach(this);
        
        private readonly List<Vector3> _points = new();
        internal float CurrentDistance = 0;
        
        private Vector2 _endPoint;
        private float _endDirection;
        
        private float _maxDistance = -1;

        private new void Start() {
            CalculatePoints();
            SetDistance(CurrentDistance);
            SetRotation();
            base.Start();
            stunObject.SetActive(false);
        }

        private void SetRotation() {
            Vector2 direction = Child.transform.position - transform.position;
            direction.Normalize();
            // if to right of child, flip
            // spriteRenderer.flipX = direction.x > 0;
            // spritePivot.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1)
            transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
        }

        private void CalculatePoints() {
            Vector2 direction = Child.transform.position - transform.position;
            direction.Normalize();
            
            Vector2 currentPoint = transform.position;
            _points.Add(currentPoint);
            
            AddPoints(currentPoint, 1);
            
            debugLineRenderer.positionCount = _points.Count;
            debugLineRenderer.SetPositions(_points.ToArray());
        }

        private void AddPoints(Vector2 startPoint, int startIndex) {
            if (_points.Count > startIndex + 1) {
                _points.RemoveRange(startIndex + 1, _points.Count - (startIndex + 1));
            }
            
            Vector2 currentPoint = startPoint;
            
            AddSegments();

            _points.Add(Child.transform.position);
            
            CalculateMaxDistance();

            void AddSegments() {
                for (int i = 0; i < maxChunks - _points.Count; i++) {
                    Vector2 difToChild = (Vector2)Child.transform.position - currentPoint;
                    Vector2 direction = Quaternion.Euler(0, 0,
                        Random.Range(-reachTurbulenceAngle, reachTurbulenceAngle)) * difToChild.normalized;
                    direction.Normalize();

                    float distance = reachTurbulenceDistance.Random();

                    float currentDistance = 0;
                    float chunkLength = distance / chunksPerSegment;
                    while (currentDistance < distance) {
                        currentDistance += chunkLength;
                        Vector2 nextPoint = currentPoint + direction * chunkLength;
                        _points.Add(nextPoint);
                        currentPoint = nextPoint;

                        difToChild = (Vector2)Child.transform.position - currentPoint;

                        if (difToChild.magnitude < childRange) {
                            return;
                        }
                    }
                }
            }
        }
        

        private void CalculateMaxDistance() {
            float distance = 0;
            for (int i = 0; i < _points.Count - 1; i++) {
                distance += (_points[i + 1] - _points[i]).magnitude;
            }
            _maxDistance = distance;
        }
        
        private int GetMaxIndex(float distance) {
            float currentDistance = 0;
            for (int i = 0; i < _points.Count - 1; i++) {
                Vector2 segment = _points[i + 1] - _points[i];
                if (currentDistance + segment.magnitude > distance) {
                    return i + 1;
                }
                currentDistance += segment.magnitude;
            }
            return _points.Count - 1;
        }
        
        private void SetLength(float distance) {
            CurrentDistance = distance;
            
            List<Vector3> newPoints = new();
            float currentDistance = 0;
            
            for (int i = 0; i < _points.Count - 1; i++) {
                Vector3 point = _points[i];
                newPoints.Add(point);
                
                Vector2 segment = _points[i + 1] - point;
                
                if (currentDistance + segment.magnitude > CurrentDistance) {
                    // We need to add a point in the middle of this segment
                    float distanceToTravel = CurrentDistance - currentDistance;
                    
                    Vector3 direction = segment.normalized;
                    newPoints.Add(point + direction * distanceToTravel);
                    
                    break;
                }

                if (i == _points.Count - 2) {
                    newPoints = _points;
                    break;
                }
                currentDistance += segment.magnitude;
            }
            
            _endPoint = newPoints[^1];
            newPoints[^1] = new Vector3(_endPoint.x, _endPoint.y, 0);
            _endPoint = newPoints[^1];

            _endDirection = Mathf.Atan2(_endPoint.y - newPoints[^2].y, _endPoint.x - newPoints[^2].x);
            
            lineRenderer.positionCount = newPoints.Count;
            lineRenderer.SetPositions(newPoints.ToArray());
            
            hand.position = _endPoint;
            hand.rotation = Quaternion.Euler(0, 0, _endDirection * Mathf.Rad2Deg - 90);
        }

        internal void ChangeDistance(float distance) {
            SetDistance(CurrentDistance + distance);
        }
        
        public void SetDistance(float distance) {
            if (distance > _maxDistance + attackRange) {
                distance = _maxDistance;
                TransitionToState(new Attack(this));
            }
            if (distance < 0) {
                Die();
                return;
            }
            SetLength(distance);
        }

        internal void Die() {
            HandleDeath();
        }

        public override void OnHitByBear(BearDamageData data) {
            if (data.DamageType == BearDamageType.Swipe) {
                TransitionToState(new Retract(this));
            } else {
                TransitionToState(new Retract(this, .1f, 2));
            }
        }

        internal void CreateNewPoints() {
            Vector2 direction = Child.transform.position - (Vector3)_endPoint;
            direction.Normalize();
            int i = GetMaxIndex(CurrentDistance);
            AddPoints(_points[i], i);
        }

        public void HandleAppendageHit(BearDamageData data) {
            if (data.DamageType == BearDamageType.Swipe) {
                TransitionToState(new Retract(this));
            } else {
                TransitionToState(new Retract(this, .1f, 2));
            }
        }

        public float GetDistance() {
            return CurrentDistance;
        }
    }
}