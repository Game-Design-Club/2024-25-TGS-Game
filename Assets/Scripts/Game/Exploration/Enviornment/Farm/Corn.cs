using UnityEngine;

namespace Game.Exploration.Enviornment.Farm
{
    public class Corn : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer standing;
        [SerializeField] private SpriteRenderer squished;
        [SerializeField] private BoxCollider2D collider;

        [ContextMenu("Squish")]
        public void Squish()
        {
            standing.enabled = false;
            squished.enabled = true;
            collider.enabled = false;
        }

        public void Squish(Vector2 direction)
        {
            Squish();
            float angle = 0;
            if (direction.x > direction.y)
            {
                if (direction.x > 0) angle = 270;
                else angle = 90;
            }
            else
            {
                if (direction.y > 0) angle = 0;
                else angle = 180;
            }


            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        
        [ContextMenu("Stand")]
        public void Stand()
        {
            standing.enabled = true;
            squished.enabled = false;
            collider.enabled = true;
        }
    }
}