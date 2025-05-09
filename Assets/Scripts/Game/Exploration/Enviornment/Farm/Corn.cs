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
        
        [ContextMenu("Stand")]
        public void Stand()
        {
            standing.enabled = true;
            squished.enabled = false;
            collider.enabled = true;
        }
    }
}