using AppCore;
using AppCore.AudioManagement;
using UnityEngine;

namespace Tools {
    public class SoundEffectCreator : MonoBehaviour {
        [SerializeField] private SoundEffect[] soundEffects;
        
        public void PlaySoundEffect(int n) {
            if (soundEffects == null || n < 0 || n >= soundEffects.Length) {
                Debug.LogError("Sound effect is not assigned.");
                return;
            }
            soundEffects[n].Play();
        }
    }
}