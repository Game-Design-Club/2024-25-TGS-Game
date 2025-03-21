using Game.GameManagement;
using Tools.LevelDesign;
using UnityEditor;
using UnityEngine;

namespace Tools.Editor {
    [CustomEditor(typeof(LevelCreator))]
    public class LevelCreatorEditor : UnityEditor.Editor {
        private LevelCreator _creator;
        private void OnEnable() {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable() {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnSceneGUI(SceneView sceneView) {
            _creator = target as LevelCreator;
            if (_creator == null) return;
            if (!_creator.isPlacing) return;
        
            Event e = Event.current;

            if (e.type == EventType.MouseDown && e.button == 0) {
                Vector2 mousePosition = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
            
                PlaceObject(mousePosition);

                e.Use();
            }
        }

        private void PlaceObject(Vector2 position) {
            _creator = (LevelCreator)target;
            if (_creator.ObjectPlacingPrefab == null) {
                Debug.LogWarning("No prefab assigned to LevelCreator");
                return;
            }

            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(_creator.ObjectPlacingPrefab);
            
            if (newObj != null) {
                if (_creator.snapToGrid) {
                    position = new Vector2(Mathf.Round(position.x / _creator.gridSize) * _creator.gridSize, Mathf.Round(position.y / _creator.gridSize) * _creator.gridSize);
                }
                newObj.transform.position = position;

                if (_creator.ParentTransform != null) {
                    newObj.transform.SetParent(_creator.ParentTransform, true);
                }
                
                if (_creator.TryRandomize) {
                    RandomizeObject(newObj);
                }

                Undo.RegisterCreatedObjectUndo(newObj, "Place Prefab");
            }
        }

        private void RandomizeObject(GameObject newObj) {
            SpriteChooser spriteChooser = newObj.GetComponent<SpriteChooser>();
            if (spriteChooser != null) {
                if (_creator.UseFlags) {
                    spriteChooser.RandomizeSprite(_creator.CustomRandomizeFlags);
                } else {
                    spriteChooser.RandomizeSprite();
                }
            }
        }
    }
}