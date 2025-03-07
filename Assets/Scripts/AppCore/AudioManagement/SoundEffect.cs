using System;
using UnityEditor;
using UnityEngine;

namespace AppCore.AudioManagement {
    [CreateAssetMenu(fileName = "SoundEffect", menuName = "Audio/SoundEffect", order = 0)]
    public class SoundEffect : ScriptableObject {
        [SerializeField] public AudioClip[] clips = new AudioClip[1];
        [SerializeField] public float volume = 1;
        [SerializeField] public float pitch = 1;
        [SerializeField] public float pitchRandomness = 0;
        [SerializeField] public bool loop = false;
        [SerializeField] public Transform parent = null;
        [SerializeField] public float spatialBlend = 0;
        [SerializeField] public float minDistance = 1;
        [SerializeField] public float maxDistance = 500;
        [SerializeField] public float fadeInTime = .1f;
        [SerializeField] public float fadeOutTime = .1f;
        public Func<bool> continueCondition = null;
        public Func<bool> paused = () => false;
        public Vector2? position = null;

        public void Play() {
            App.Get<AudioManager>().PlaySoundEffect(this);
        }

        public void Stop() {
            App.Get<AudioManager>().StopSoundEffect(this);
        }
    }
}