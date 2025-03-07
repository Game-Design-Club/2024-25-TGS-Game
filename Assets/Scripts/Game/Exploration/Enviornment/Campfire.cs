using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Game.Combat;
using Game.GameManagement;
using Tools;

namespace Game.Exploration.Enviornment {
    public class Campfire : MonoBehaviour {
        [Header("References")]
        [SerializeField] private CombatAreaManager CombatAreaManager;
        
        [Header("Exploring")]
        // These are grabbed from the Light2D on Awake.
        private float _exploreIntensity;
        private float _exploreRadius;
        
        [Header("Combat")]
        [SerializeField] private float baseCombatIntensity; // Combat base target intensity
        [SerializeField] private float baseCombatRadius;    // Combat base target radius
        
        [Header("Based on Sanity")]
        // Affect the light when in combat.
        [SerializeField] private AnimationCurve combatIntensityAddition; // Steady addition: darker when less sane
        [SerializeField] private AnimationCurve combatPulseFrequency;  // Pulse frequency modifier based on sanity
        [SerializeField] private AnimationCurve combatPulseIntensity;  // Sine-wave pulse intensity modifier
        
        [Header("Flash")]
        [SerializeField] private AnimationCurve intensityFlashCurve; // For flash intensity changes
        [SerializeField] private AnimationCurve radiusFlashCurve;    // For flash radius changes

        private Light2D _light;
        private Coroutine _flashCoroutine;
        private int _flashQueue;

        // Affectors:
        // (1) Base state value (set via transitions)
        // (2) Flash addition (from OnChildHit)
        // (3) Sinusoidal pulse (active in combat)
        private float _baseIntensity; // Updated via TransitionLighting (explore vs. combat)
        private float _baseRadius;
        private float _flashIntensity; // Temporary flash addition from FlashCoroutine
        private float _flashRadius;

        private float _currentAppliedIntensity;
        private float _currentAppliedRadius;
        [SerializeField] private float intensitySmoothSpeed = 2f;
        [SerializeField] private float radiusSmoothSpeed = 2f;
        
        private float _transitionMultiplier = 0f;

        enum CampfireState {
            Explore,
            Combat
        }
        [SerializeField] private CampfireState _state = CampfireState.Explore;

        private void Awake() {
            if (!TryGetComponent(out _light))
                Debug.LogError("No Light2D component found on Campfire!");

            _exploreIntensity = _light.intensity;
            _exploreRadius = _light.pointLightOuterRadius;

            _baseIntensity = _exploreIntensity;
            _baseRadius = _exploreRadius;

            _currentAppliedIntensity = _light.intensity;
            _currentAppliedRadius = _light.pointLightOuterRadius;

            _transitionMultiplier = 0f;
        }

        private void OnEnable() {
            CombatAreaManager.OnChildHit += OnChildHit;
            GameManager.OnGameEvent += OnGameEvent;
        }

        private void OnDisable() {
            CombatAreaManager.OnChildHit -= OnChildHit;
            GameManager.OnGameEvent -= OnGameEvent;
        }

        private void OnGameEvent(GameEvent gameEvent) {
            StopAllCoroutines();
            if (gameEvent.GameEventType == GameEventType.CombatEnter) {
                _state = CampfireState.Combat;
                StartCoroutine(TransitionLighting(baseCombatIntensity, baseCombatRadius, GameManager.TransitionDuration, 1f));
            }
            else if (gameEvent.GameEventType == GameEventType.ExploreEnter) {
                _state = CampfireState.Explore;
                StartCoroutine(TransitionLighting(_exploreIntensity, _exploreRadius, GameManager.TransitionDuration, 0f));
            }
        }

        private IEnumerator TransitionLighting(float toIntensity, float toRadius, float duration, float targetMultiplier) {
            float fromIntensity = _baseIntensity;
            float fromRadius = _baseRadius;
            float startMultiplier = _transitionMultiplier;
            float t = 0f;
            while (t < duration) {
                t += Time.deltaTime;
                float progress = t / duration;
                _baseIntensity = Mathf.Lerp(fromIntensity, toIntensity, progress);
                _baseRadius = Mathf.Lerp(fromRadius, toRadius, progress);
                _transitionMultiplier = Mathf.Lerp(startMultiplier, targetMultiplier, progress);
                yield return null;
            }
            _baseIntensity = toIntensity;
            _baseRadius = toRadius;
            _transitionMultiplier = targetMultiplier;
        }

        // Called when a child is hit; queues up a flash.
        private void OnChildHit() {
            _flashQueue++;
            if (_flashCoroutine == null) {
                _flashCoroutine = StartCoroutine(FlashCoroutine());
            }
        }

        // Flash coroutine applies temporary changes using the provided curves.
        private IEnumerator FlashCoroutine() {
            float t = 0f;
            float flashDuration = intensityFlashCurve.Time();
            while (t < flashDuration) {
                t += Time.deltaTime;
                _flashIntensity = intensityFlashCurve.Evaluate(t);
                _flashRadius = radiusFlashCurve.Evaluate(t);
                yield return null;
            }
            _flashIntensity = 0f;
            _flashRadius = 0f;
            _flashQueue--;
            if (_flashQueue > 0) {
                _flashCoroutine = StartCoroutine(FlashCoroutine());
            } else {
                _flashCoroutine = null;
            }
        }

        private float CurrentIntensityAddition => combatIntensityAddition.Percentage(CombatAreaManager.SanityPercentage);
        private float CurrentPulseFrequency => combatPulseFrequency.Evaluate(CombatAreaManager.SanityPercentage);
        private float CurrentPulseIntensityAddition => combatPulseIntensity.Evaluate(CombatAreaManager.SanityPercentage);

        private void Update() {
            // Update the base (plus steady combat addition) and smoothly move toward it.
            float baseIntensity = _baseIntensity + CurrentIntensityAddition;
            _currentAppliedIntensity = Mathf.MoveTowards(_currentAppliedIntensity, baseIntensity, intensitySmoothSpeed * Time.deltaTime);
            _currentAppliedRadius = Mathf.MoveTowards(_currentAppliedRadius, _baseRadius, radiusSmoothSpeed * Time.deltaTime);

            float pulseAddition = 0f;
            if (_state == CampfireState.Combat) {
                pulseAddition = Mathf.Sin(Time.time * CurrentPulseFrequency * 2 * Mathf.PI) * CurrentPulseIntensityAddition;
            }
            float effectivePulse = _transitionMultiplier * pulseAddition;
            float effectiveFlash = _transitionMultiplier * _flashIntensity;

            _light.intensity = _currentAppliedIntensity + effectivePulse + effectiveFlash;
            _light.pointLightOuterRadius = _currentAppliedRadius + _flashRadius;
        }
    }
}