using UnityEngine;

namespace AppCore.AudioManagement {
    [CreateAssetMenu(fileName = "Music", menuName = "Audio/Music", order = 0)]
    public class Music : ScriptableObject {
        [SerializeField] public AudioClip audioClip;
        [SerializeField] public float volume = 1f;
    }
}