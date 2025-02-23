using System;
using System.Collections;
using AppCore;
using AppCore.InputManagement;
using Tools;
using UnityEngine;

namespace Game.GameManagement {
    public class UIManager : MonoBehaviour {
        [SerializeField] private Animator pauseAnimator;
        [SerializeField] private Animator gameOverAnimator;
        [SerializeField] private PauseUIAnimationEvents pauseEvents;
        [SerializeField] private GameObject scrapbookCoverObject;
        [SerializeField] private GameObject scrapbookPageObject;
        
        private int scrapbookPage = 0;
        private bool advancingPage = false;
        private bool scrapbookOpen = false;
        
        private bool _isGameOver = false;
        private bool _canRestart = false;
        
        public static event Action OnRestartGame;

        private void LoadPage()
        {
            Debug.Log("Load page " + scrapbookPage);
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
            scrapbookPage++;
            OpenPage(scrapbookPage);
        }
        
        public void PreviousPage()
        {
            scrapbookPage--;
            OpenPage(scrapbookPage);
        }

        public void OnBookDown()
        {
            scrapbookCoverObject.SetActive(!scrapbookOpen);
            scrapbookPageObject.SetActive(scrapbookOpen);
            if (advancingPage)
            {
                if (scrapbookOpen) LoadPage();
                pauseAnimator.SetBool("BookUp", true);
            }
        }

        public void OnBookUp()
        {
            advancingPage = false;
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
            pauseAnimator.SetBool(Constants.Animator.GameUI.IsPaused, gameEvent.IsPaused);
        }

        private void HandleGameOver() {
            gameOverAnimator.SetBool(Constants.Animator.GameUI.IsGameOver, true);
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

                gameOverAnimator.SetBool(Constants.Animator.GameUI.IsGameOver, false);
            }
        }
    }
}