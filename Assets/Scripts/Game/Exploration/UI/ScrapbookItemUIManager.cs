using System;
using System.Collections;
using System.Collections.Generic;
using AppCore;
using AppCore.DialogueManagement;
using Game.Exploration.Enviornment.Interactables.Scrapbook;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Exploration.UI
{
    public class ScrapbookItemUIManager : MonoBehaviour
    {
        public ScrapbookPage.ScrapbookItemUIInfo itemInfo;
        [SerializeField] private RectTransform rt;
        [SerializeField] private RectTransform rtCanvas;
        [SerializeField] private RectTransform rtItemHolder;
        [SerializeField] private GameObject itemHolder;
        [SerializeField] private Animator animator;
        [SerializeField] private AnimationCurve moveToPosCurve;
        [SerializeField] private float movementDuration;
        [SerializeField] private Image itemImage;
        [SerializeField] private Image holeImage;
        
        public void Start()
        {
            itemImage.sprite = itemInfo.item.sprite;
            holeImage.sprite = itemInfo.item.sprite;
            rt.anchoredPosition = itemInfo.pos;
            rt.sizeDelta = itemInfo.size * 100;
        }

        private Vector2 GetMid()
        {
            Vector2 canvas = new Vector2(0, 0);
            Vector2 world = rtCanvas.TransformPoint(canvas);
            return rt.InverseTransformPoint(world);
        }


        public void Hover()
        {
            animator.SetBool("Hovering", !animator.GetBool("Hovering"));
        }
        
        [ContextMenu("Fly In")]
        public void FlyIn()
        {
            animator.SetBool("Resting", false);
            rtItemHolder.anchoredPosition = GetMid();
        }

        public void OnFinishedFlyIn()
        {
            App.Get<DialogueManager>().StartDialogue(itemInfo.item.dialogue, FinishedDialogue, AnimatorUpdateMode.UnscaledTime);
        }

        private void FinishedDialogue()
        {
            StartCoroutine(MoveToPosition());
            animator.SetBool("Resting", true);
        }
        
        IEnumerator MoveToPosition()
        {
            Vector2 startPosition = GetMid();
            Vector2 endPosition = new Vector2(0, 0);
            // Debug.Log("Start: " + startPosition + " End: " + endPosition);
            float elapsedTime = 0f;

            while (elapsedTime < movementDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float t = elapsedTime / movementDuration;
                Vector2 cPos = Vector3.Lerp(startPosition, endPosition, moveToPosCurve.Evaluate(t));
                // Debug.Log(cPos + " " + elapsedTime + " / " + movementDuration + " : " + t);
                rtItemHolder.anchoredPosition = cPos;
                yield return null; // Wait for the next frame
            }

            // Ensure the object reaches the exact destination
            rt.anchoredPosition = endPosition;
        }
    }
}
