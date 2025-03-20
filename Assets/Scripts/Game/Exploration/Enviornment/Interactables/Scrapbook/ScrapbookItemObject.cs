using System;
using System.Collections;
using AppCore;
using AppCore.DataManagement;
using Game.GameManagement;
using UnityEngine;

namespace Game.Exploration.Enviornment.Interactables.Scrapbook
{
    public class ScrapbookItemObject : InteractableObject
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject scrapObject;
        [SerializeField] private Animator animator;
        [SerializeField] private ParticleSystem spinnyParticles;
        [Header("Scrapbook Item")]
        [SerializeField] private ScrapbookItem scrapbookItem;

        private void Awake() {
            if (App.Get<DataManager>().HasScrapbookItem(scrapbookItem.itemName)) {
                Destroy(scrapObject);
            }
        }

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
            App.Get<DataManager>().AddScrapbookItem(scrapbookItem.itemName);
    
            UIManager uiManager = GameManager.GetUIManager();
            uiManager.SetNewItem(scrapbookItem);
    
            animator.SetTrigger("Spin and Grow");
            spriteRenderer.sortingLayerName = "Kinda UI";
            StartCoroutine(Blow());
        }

        IEnumerator Blow()
        {
            yield return new WaitForSeconds(2.1f);
            spinnyParticles.Play();
            yield return new WaitForSeconds(2.0f);
            GameManager.OnGamePaused();
            GameManager.GetUIManager().OpenScrapbook();
            GameManager.GetUIManager().OpenToItem(scrapbookItem);
            Destroy(scrapObject);
            GameManager.DialogueEnd();
        }
    }
}
