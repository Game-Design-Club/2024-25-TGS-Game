using System;
using System.Collections;
using AppCore;
using AppCore.InputManagement;
using Game.Exploration.Enviornment.Interactables.Scrapbook;
using Game.Exploration.UI;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.GameManagement {
    public class UIManager : MonoBehaviour {
        [SerializeField] private Animator pauseAnimator;
        [SerializeField] private Animator gameOverAnimator;
        [SerializeField] private PauseUIAnimationEvents pauseEvents;
        [SerializeField] private RectTransform rtCanvas;
        [SerializeField] private GameObject coverObject;
        [SerializeField] private GameObject sbSpreadObject;
        [SerializeField] private TextMeshProUGUI sbInfoTitle;
        [SerializeField] private TextMeshProUGUI sbInfoDescription;
        [SerializeField] private TextMeshProUGUI sbAreaTitle;
        [SerializeField] private GameObject sbItemHolderObject;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private ScrapbookPage[] scrapbookPages;
        [SerializeField] private GameObject scrapbookUIItemPrefab;
        
        private int scrapbookPage = 0;
        private bool advancingPage = false;
        private bool scrapbookOpen = false;
        private ScrapbookItemUIManager currentFocused;
        
        private bool _isGameOver = false;
        private bool _canRestart = false;
        private ScrapbookItem newScrapbookItem;

        public static event Action OnRestartGame;
        public static event Action OnPageUp;

        private void Start() {
            rtCanvas.gameObject.SetActive(true);
            EventSystem.current.firstSelectedGameObject = resumeButton.gameObject;
            // sbSpreadObject.SetActive(false);
            // coverObject.SetActive(false);
        }

        public void FocusOnItem(ScrapbookItemUIManager itemUI, bool focus)
        {
            if (focus) currentFocused?.Hover(false);
            
            sbInfoTitle.text = focus ? itemUI.itemInfo.item.itemName : scrapbookPages[scrapbookPage].title;
            sbInfoDescription.text = focus ? itemUI.itemInfo.item.description : scrapbookPages[scrapbookPage].description;
            currentFocused = focus ? itemUI : null;
        }

        [ContextMenu("Reload Page")]
        private void LoadPage()
        {
            foreach (Transform t in sbItemHolderObject.transform)
            {
                Destroy(t.gameObject);
            }

            nextButton.interactable = scrapbookPage != scrapbookPages.Length - 1;
            previousButton.interactable = scrapbookPage != 0;

            ScrapbookPage page = scrapbookPages[scrapbookPage];
            sbAreaTitle.text = page.title;
    
            foreach (ScrapbookPage.ScrapbookItemUIInfo info in page.items)
            {
                GameObject o = Instantiate(scrapbookUIItemPrefab, sbItemHolderObject.transform);
                ScrapbookItemUIManager m = o.GetComponent<ScrapbookItemUIManager>();
                m.uIManager = this;
                m.itemInfo = info;
                m.rtCanvas = rtCanvas;
        
                // Check if this item is the new one
                if (newScrapbookItem != null && newScrapbookItem.Equals(info.item))
                {
                    m.isNewItem = true;
                    newScrapbookItem = null; // Reset after marking it
                }
            }
    
            FocusOnItem(null, false);
        }

        public void OpenToItem(ScrapbookItem scrapbookItem)
        {
            scrapbookOpen = true;
            
            for (int i = 0; i < scrapbookPages.Length; i++)
            {
                ScrapbookPage page = scrapbookPages[i];
                foreach (ScrapbookPage.ScrapbookItemUIInfo itemInfo in page.items)
                {
                    if (scrapbookItem.Equals(itemInfo.item))
                    {
                        OpenPage(i);
                        OnBookDown();
                    }
                }
            }
        }
        
        public void SetNewItem(ScrapbookItem item)
        {
            newScrapbookItem = item;
        }
        
        private void OpenPage(int pageNum)
        {
            scrapbookPage = pageNum;
            advancingPage = true;
            pauseAnimator.SetBool("BookUp", false);
        }

        public void OpenScrapbook()
        {
            scrapbookOpen = true;
            OpenPage(0);
        }

        public void CloseScrapbook()
        {
            scrapbookOpen = false;
            advancingPage = true;
            pauseAnimator.SetBool("BookUp", false);
        }

        public void NextPage()
        {
            if (scrapbookPage == scrapbookPages.Length - 1) return;
            
            scrapbookPage++;
            OpenPage(scrapbookPage);
        }
        
        public void PreviousPage()
        {
            if (scrapbookPage == 0) return;
            scrapbookPage--;
            OpenPage(scrapbookPage);
        }

        public void OnBookDown()
        {
            EventSystem.current.SetSelectedGameObject(null);
            coverObject.SetActive(!scrapbookOpen);
            sbSpreadObject.SetActive(scrapbookOpen);
            if (advancingPage)
            {
                if (scrapbookOpen) LoadPage();
                pauseAnimator.SetBool("BookUp", true);
            }
        }

        public void OnBookUp()
        {
            advancingPage = false;
            OnPageUp?.Invoke();
        }
        
        private void OnEnable() {
            GameManager.OnGameEvent += OnGameEvent;
            App.Get<InputManager>().OnUIRestart += OnUIContinue;
            pauseEvents.OnBookUp += OnBookUp;
            pauseEvents.OnBookDown += OnBookDown;
        }
        
        private void OnDisable() {
            GameManager.OnGameEvent -= OnGameEvent;
            App.Get<InputManager>().OnUIRestart -= OnUIContinue;
            pauseEvents.OnBookUp -= OnBookUp;
            pauseEvents.OnBookDown -= OnBookDown;
        }

        private void OnGameEvent(GameEvent gameEvent) {
            if (gameEvent.GameEventType == GameEventType.BearDeath) {
                HandleGameOver();
            }
            else {
                HandlePause(gameEvent);
            }
        }
        private void HandlePause(GameEvent gameEvent) {
            Time.timeScale = gameEvent.IsPaused ? 0 : 1;
            pauseAnimator.SetBool(AnimationConstants.GameUI.IsPaused, gameEvent.IsPaused);
            pauseAnimator.SetBool("BookUp", true);
            if (!gameEvent.IsPaused) CloseScrapbook();
        }

        private void HandleGameOver() {
            gameOverAnimator.SetBool(AnimationConstants.GameUI.IsGameOver, true);
            _isGameOver = true;
            StartCoroutine(WaitToRestartGame());
        }

        private IEnumerator WaitToRestartGame() {
            yield return new WaitForSeconds(GameManager.TransitionDuration);
            _canRestart = true;
        }

        private void OnUIContinue() {
            if (_isGameOver && _canRestart) {
                OnRestartGame?.Invoke();
                
                _isGameOver = false;
                _canRestart = false;

                gameOverAnimator.SetBool(AnimationConstants.GameUI.IsGameOver, false);
            }
        }

        private void OnApplicationPause(bool pauseStatus) {
            
        }
    }
}