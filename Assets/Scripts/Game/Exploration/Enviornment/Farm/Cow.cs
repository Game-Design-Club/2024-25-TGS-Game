using AppCore;
using AppCore.AudioManagement;
using Game.Exploration.Child;
using UnityEngine;

namespace Game.Exploration.Enviornment.Farm
{
    public class Cow : MonoBehaviour, IChildHittable
    {
        [SerializeField] private SpriteRenderer standing;
        [SerializeField] private SpriteRenderer tipped;
        [SerializeField] private BoxCollider2D collider;
        [SerializeField] private SoundEffect moo;
        private bool isTipped = false;

        [ContextMenu("Tip")]
        public void Tip()
        {
            if (isTipped) return;
            
            Moo();
            standing.enabled = false;
            tipped.enabled = true;
            isTipped = true;
        }

        public void Moo()
        {
            App.Get<AudioManager>().PlaySoundEffect(moo);
        }

        [ContextMenu("Stand")]
        public void Stand()
        {
            standing.enabled = true;
            tipped.enabled = false;
            isTipped = false;
        }

        public void Hit(Vector2 hitDirection)
        {
            Moo();
        }
    }
}