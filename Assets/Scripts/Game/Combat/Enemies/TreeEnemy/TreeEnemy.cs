using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Combat.Enemies.TreeEnemy {
    public class TreeEnemy : EnemyBase {
        [SerializeField] internal float reachSpeed = 5f;
        [SerializeField] private FloatRange reachTurbulenceDistance = new(0.5f, 1.5f);
        [SerializeField] private float reachTurbulenceAngle = 10f;
        [SerializeField] private float childRange = 1f;
        [SerializeField] private int maxSegments = 10;
        [SerializeField] private LineRenderer debugLineRenderer;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform hand;

        protected override EnemyState StartingState => new Reach(this);
        
        private readonly List<Vector3> _points = new();
        internal float CurrentDistance = 0;
        
        private Vector2 _endPoint;
        private float _endDirection;
        
        private float _maxDistance = -1;

        private new void Start() {
            CalculatePoints();
            base.Start();
            SetDistance(CurrentDistance);
        }
        
        private void CalculatePoints() {
            Vector2 direction = CombatManager.Child.transform.position - transform.position;
            direction.Normalize();
            
            Vector2 currentPoint = transform.position;
            
            AddPoints(currentPoint, direction);
            
            debugLineRenderer.positionCount = _points.Count;
            debugLineRenderer.SetPositions(_points.ToArray());
        }

        private void AddPoints(Vector2 currentPoint, Vector2 direction) {
            // TODO REMOVE OLD POITNS
            // TODO add by chunky so can do radius tingies
            bool done = false;
            _points.Add(currentPoint);
            
            for (int i = 0; i < maxSegments - _points.Count; i++) {
                // Go in direction until Controller<TreeEnemy>().reachTurnDistance.Random(), add a new point
                // Turn a random amount of degrees, repeat process
                // If within distance of child, add a point at the child and break
                
                float distance = reachTurbulenceDistance.Random();
                Vector2 nextPoint = currentPoint + direction * distance;
                _points.Add(nextPoint);
                currentPoint = nextPoint;
                
                Vector2 difToChild = (Vector2)CombatManager.Child.transform.position - currentPoint;
                direction = Quaternion.Euler(0, 0, 
                    Random.Range(-reachTurbulenceAngle, reachTurbulenceAngle)) * difToChild.normalized;
                direction.Normalize();
                
                if (difToChild.magnitude < childRange) {
                    break;
                }
            }
            
            _points.Add(CombatManager.Child.transform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0));
            
            
            CalculateMaxDistance();
        }
        
        private void CalculateMaxDistance() {
            float distance = 0;
            for (int i = 0; i < _points.Count - 1; i++) {
                distance += (_points[i + 1] - _points[i]).magnitude;
            }
            _maxDistance = distance;
        }
        
        // private int GetMaxIndex(float distance) {
        //     float currentDistance = 0;
        //     for (int i = 0; i < _points.Count - 1; i++) {
        //         Vector2 segment = _points[i + 1] - _points[i];
        //         if (currentDistance + segment.magnitude > distance) {
        //             return i + 1;
        //         }
        //         currentDistance += segment.magnitude;
        //     }
        //     return _points.Count - 1;
        // }
        
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

            _endDirection = (float)Math.Atan2(_endPoint.y - newPoints[^2].y, _endPoint.x - newPoints[^2].x);
            
            lineRenderer.positionCount = newPoints.Count;
            lineRenderer.SetPositions(newPoints.ToArray());
            
            hand.position = _endPoint;
            hand.rotation = Quaternion.Euler(0, 0, _endDirection * Mathf.Rad2Deg - 90);
        }

        internal void ChangeDistance(float distance) {
            SetDistance(CurrentDistance + distance);
        }
        
        public void Hit() {
            TransitionToState(new Retract(this));
        }

        public void SetDistance(float distance) {
            if (distance > _maxDistance) {
                distance = _maxDistance;
            }
            if (distance < 0) {
                OnHit(100000, Vector2.zero, 0);
                return;
            }
            SetLength(distance);
        }
    }
}