using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools {
    public class PointCollisionHelper {
        public static IEnumerator MoveToInArea(BoxCollider2D boxCollider, Rigidbody2D Rigidbody, Action<Vector2> resultDirection, Func<PointCollision, bool> isTarget, Transform[] pointsMatch = null, float buffer = 0 ,Vector2 constraint = default) {
            float xSize = (boxCollider.size.x / 2) * boxCollider.transform.lossyScale.x - buffer;
            float ySize = (boxCollider.size.y / 2) * boxCollider.transform.lossyScale.y - buffer;       
            float xOffset = boxCollider.offset.x * boxCollider.transform.lossyScale.x;
            float yOffset = boxCollider.offset.y * boxCollider.transform.lossyScale.y;
            Vector2 offset = new Vector2(xOffset, yOffset);
            int i = 0;
            while (true) {
                Vector2 pos = Rigidbody.position;
                // PointCollision topLeft = new PointCollision(pos + new Vector2(-xSize, ySize), boxCollider);
                // PointCollision topRight = new PointCollision(pos + new Vector2(xSize, ySize), boxCollider);
                // PointCollision bottomLeft = new PointCollision(pos + new Vector2(-xSize, -ySize), boxCollider);
                // PointCollision bottomRight = new PointCollision(pos + new Vector2(xSize, -ySize), boxCollider);
                Vector2 topLeftPos = pos + new Vector2(-xSize, ySize) + offset;
                Vector2 topRightPos = pos + new Vector2(xSize, ySize) + offset;
                Vector2 bottomLeftPos = pos + new Vector2(-xSize, -ySize) + offset;
                Vector2 bottomRightPos = pos + new Vector2(xSize, -ySize) + offset;
                
                PointCollision topLeft = new PointCollision(topLeftPos, boxCollider);
                PointCollision topRight = new PointCollision(topRightPos, boxCollider);
                PointCollision bottomLeft = new PointCollision(bottomLeftPos, boxCollider);
                PointCollision bottomRight = new PointCollision(bottomRightPos, boxCollider);
                
                if (pointsMatch != null && pointsMatch.Length == 4) {
                    pointsMatch[0].position = topLeftPos;
                    pointsMatch[1].position = topRightPos;
                    pointsMatch[2].position = bottomLeftPos;
                    pointsMatch[3].position = bottomRightPos;
                }
            
                if (isTarget(topLeft) && isTarget(topRight) && isTarget(bottomLeft) && isTarget(bottomRight)) {
                    break;
                }
                
                if (!topLeft.TouchingLand && !topRight.TouchingLand && !bottomLeft.TouchingLand && !bottomRight.TouchingLand) {
                    break;
                }
                
                Vector2 direction = Vector2.zero;
                if (isTarget(topLeft)) {
                    direction += new Vector2(-1, 1);
                }
                if (isTarget(topRight)) {
                    direction += new Vector2(1, 1);
                }
                if (isTarget(bottomLeft)) {
                    direction += new Vector2(-1, -1);
                }
                if (isTarget(bottomRight)) {
                    direction += new Vector2(1, -1);
                }
                
                if (constraint != default) {
                    direction *= constraint;
                }

                if (direction == Vector2.zero) {
                    // Debug.LogWarning("No force direction");
                    // break;
                }
                
                direction.Normalize();
                resultDirection(direction);
                yield return new WaitForFixedUpdate();
                i++;
                if (i > 500) {
                    break;
                }
            }
        }
        
    }
}