using Game.Exploration.Enviornment.River;
using Tools;
using UnityEngine;

namespace Game.Exploration.Child {
    public class PlayerPointCollision {
        public readonly Collider2D[] Colliders = new Collider2D[20];
        public Vector2 Point;
        public bool TouchingGround = false;
        public bool TouchingRiver = false;
        public RiverChunk River;

        public PlayerPointCollision(Vector2 point) {
            Point = point;
            Physics2D.OverlapPoint(Point, new ContactFilter2D().NoFilter(), Colliders);
                
            foreach (Collider2D collider in Colliders) {
                if (collider is null) {
                    continue;
                }

                if (collider.CompareTag(Tags.River)) {
                    River = collider.GetComponent<RiverChunk>();
                    // Debug.Log("Rive!r: " + River);
                    TouchingRiver = true;
                } else if (collider.CompareTag(Tags.Ground)) {
                    TouchingGround = true;
                }
            }
        }
    }
}