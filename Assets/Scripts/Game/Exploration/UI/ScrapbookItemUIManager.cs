using System;
using System.Collections;
using System.Collections.Generic;
using AppCore;
using AppCore.DialogueManagement;
using Game.Exploration.Enviornment.Interactables.Scrapbook;
using Game.GameManagement;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Exploration.UI
{
    public class ScrapbookItemUIManager : MonoBehaviour
    {
        public ScrapbookPage.ScrapbookItemUIInfo itemInfo;
        [SerializeField] private RectTransform rt;
        public  RectTransform rtCanvas;
        [SerializeField] private RectTransform rtItemHolder;
        [SerializeField] private Animator animator;
        [SerializeField] private AnimationCurve moveToPosCurve;
        [SerializeField] private float movementDuration;
        [SerializeField] private Image itemImage;
        [SerializeField] private Image holeImage;
        public UIManager uIManager;
        
        public void Start()
        {
            itemImage.sprite = itemInfo.item.sprite;
            holeImage.sprite = itemInfo.item.sprite;
            rt.anchoredPosition = itemInfo.pos;
            rt.sizeDelta = itemInfo.size * 100;
        }

        public void Unlock()
        {
            
        }

        private Vector2 GetMid()
        {
            Vector2 canvas = new Vector2(0, 0);
            Vector2 world = rtCanvas.TransformPoint(canvas);
            return rt.InverseTransformPoint(world);
        }
        
        public void Hover(bool hover)
        {
            animator.SetBool("Hovering", hover);
            
            uIManager.FocusOnItem(this, hover);
        }

        public void ButtonPress()
        {
            
            Hover(!animator.GetBool("Hovering"));
        }
        
        [ContextMenu("Fly In")]
        public void FlyIn()
        {
            animator.SetBool("Resting", false);
            rtItemHolder.anchoredPosition = GetMid();
        }

        public void OnFinishedFlyIn()
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
            rtItemHolder.anchoredPosition = endPosition;
            animator.SetBool("Hovering", false);
            Hover(true);
        }
    }
}
