using System;
using UnityEditor;
using UnityEngine;

namespace Game.Exploration.Enviornment.River {
    public class RiverSprites : MonoBehaviour {
        [SerializeField] private GameObject baseObject;
        [SerializeField] private Transform spriteReference;
        [SerializeField] private int length = 5;
        [SerializeField] private float offset = 0.5f;
        [SerializeField] private float moveSpeed = 1f;
        
        private GameObject[] _sprites;

        private void OnValidate() {
            if (baseObject == null) {
                Debug.LogWarning("Base object is null!");
            }
            // local scale is eventual size of all the sprites
            spriteReference.localScale = new Vector3(offset * length, 1, 1);
        }

        private void CreateObjects() {
            foreach (Transform child in transform) {
                EditorApplication.delayCall+=()=>
                 {
                      DestroyImmediate(child.gameObject);
                 };
            }
            
            _sprites = new GameObject[length];
            float currentOffset = -offset*length / 2;
            for (int i = 0; i < length; i++) {
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(baseObject);
                instance.transform.SetParent(transform, false);
                _sprites[i] = instance;
                _sprites[i].transform.localPosition = new Vector3(currentOffset, 0, 0);
                currentOffset += offset;
            }
        }

        private void Awake() {
            CreateObjects();
        }

        private void Update() {
            for (int i = 0; i < length; i++) {
                _sprites[i].transform.localPosition += Vector3.right * (moveSpeed * Time.deltaTime);
                if (_sprites[i].transform.localPosition.x > offset * length / 2) {
                    _sprites[i].transform.localPosition = new Vector3(-offset * length / 2, 0, 0);
                }
            }
        }
    }
}