using System;
using Tools;
using UnityEngine;

namespace AppCore.AudioManagement {
    public class MusicTrigger : MonoBehaviour {
        [SerializeField] private Music music;
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag(Tags.Child)) {
                App.Get<MusicManager>().PlayMusic(music);
            }
        }
    }
}