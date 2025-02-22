using System;
using System.Collections;
using AppCore;
using AppCore.DataManagement;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables.Scrapbook
{
    public class ScrapbookItemObject : InteractableObject
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject scrapObject;
        [SerializeField] private ScrapbookItem scrapbookItem;
        [SerializeField] private Animator animator;
        [SerializeField] private ParticleSystem spinnyParticles;

        private void Start()
        {
            spriteRenderer.sprite = scrapbookItem.sprite;
        }

        public override void InteractStarted()
        {
            //TODO change sorting layer
        }

        public override void InteractionEnded()
        {
            App.Get<DataManager>().FoundScrapbookItem(scrapbookItem);
            animator.SetTrigger("Spin and Grow");
            interactableParticleSystem.Stop();
            spriteRenderer.sortingLayerName = "Kinda UI";
            StartCoroutine(Blow());
        }

        IEnumerator Blow()
        {
            yield return new WaitForSeconds(2.1f);
            spinnyParticles.Play();
            yield return new WaitForSeconds(2.0f);
            Destroy(scrapObject);
        }
    }
}
