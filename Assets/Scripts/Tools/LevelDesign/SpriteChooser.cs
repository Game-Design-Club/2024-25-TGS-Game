using System;
using System.Collections.Generic;
using Game.Exploration.Enviornment.LayerChanging;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Tools.LevelDesign {
    public class SpriteChooser : MonoBehaviour {
        [SerializeField] private bool hideSprite = false;
        [SerializeField] private bool randomize = false;
        [SerializeField] private bool randomizeFlip = false;
        [SerializeField] private int objectNumber;
        [FormerlySerializedAs("_spriteChoserData")] [SerializeField] private SpriteChooserData spriteChooserData;
        [SerializeField] private bool useRandomizeFlags = false;
        [SerializeField] private bool[] randomizeFlags;
        
        private List<int> _possibleObjects = new List<int>();
        
        private void OnValidate() {
            if (randomize) {
                randomize = false;
                RandomizeSpriteInternal(useRandomizeFlags, randomizeFlags);
            }

            if (randomizeFlip) {
                RandomizeFlip();
            }

            if (objectNumber > spriteChooserData.objects.Length) {
                objectNumber = spriteChooserData.objects.Length - 1;
            }
            if (objectNumber < 0) {
                objectNumber = 0;
            }
            ChangeSprite();
            if (hideSprite) {
                if (TryGetComponent(out SpriteRenderer spriteRenderer)) {
                    spriteRenderer.enabled = false;
                } else {
                    Debug.LogError($"{nameof(SpriteChooser)} requires a {nameof(SpriteRenderer)} component");
                }
            } else {
                if (TryGetComponent(out SpriteRenderer spriteRenderer)) {
                    spriteRenderer.enabled = true;
                } else {
                    Debug.LogError($"{nameof(SpriteChooser)} requires a {nameof(SpriteRenderer)} component");
                }
            }
        }

        private void Start() {
            GetComponent<SpriteRenderer>().enabled = true;
        }

        private void RandomizeSpriteInternal(bool shouldUseFlags = false, bool[] creatorRandomizeFlags = null) {
            if (!shouldUseFlags) {
                objectNumber = Random.Range(0, spriteChooserData.objects.Length);
            } else {
                CalculatePossibleObjects(creatorRandomizeFlags);
            }
            ChangeSprite();
        }

        private void CalculatePossibleObjects(bool[] creatorRandomizeFlags) {
            _possibleObjects.Clear();
            for (int i = 0; i < creatorRandomizeFlags.Length; i++) {
                if (creatorRandomizeFlags[i]) {
                    _possibleObjects.Add(i);
                }
            }
            if (_possibleObjects.Count == 0) {
                objectNumber = 0;
                Debug.LogWarning("No flags set for randomization");
            } else {
                objectNumber = _possibleObjects[Random.Range(0, _possibleObjects.Count)];
            }
        }

        private void ChangeSprite() {
            if (objectNumber >= spriteChooserData.objects.Length) {
                objectNumber = spriteChooserData.objects.Length - 1;
            }
            if (objectNumber < 0) {
                objectNumber = 0;
            }
            if (TryGetComponent(out SpriteRenderer spriteRenderer)) {
                spriteRenderer.sprite = spriteChooserData.objects[objectNumber].sprite;
                if (TryGetComponent(out BoxCollider2D boxCollider2D)) {
                    boxCollider2D.size = spriteChooserData.objects[objectNumber].size;
                    boxCollider2D.offset = spriteChooserData.objects[objectNumber].offset;
                }
                if (TryGetComponent(out LayerChanger layerChanger)) {
                    layerChanger.yOffset = spriteChooserData.objects[objectNumber].YOffset;
                }
            } else {
                Debug.LogError($"{nameof(SpriteChooser)} requires a {nameof(SpriteRenderer)} component");
            }
        }

        public void RandomizeSprite(bool[] RandomizedFlags) {
            RandomizeSpriteInternal(true, RandomizedFlags);
        }

        public void RandomizeSprite() {
            RandomizeSpriteInternal();
        }

        public void RandomizeFlip()
        {
            if (TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.flipX = Random.Range(0, 2) == 0;
            }

            randomizeFlip = false;
        }

    }
}