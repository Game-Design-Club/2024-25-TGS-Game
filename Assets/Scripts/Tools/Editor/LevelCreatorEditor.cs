using Game.GameManagement;
using Tools.LevelDesign;
using UnityEditor;
using UnityEngine;

namespace Tools.Editor {
    [CustomEditor(typeof(LevelCreator))]
    public class LevelCreatorEditor : UnityEditor.Editor
    {
        private LevelCreator _creator;
        private Vector2 lastMousePosition;
        private void OnEnable() {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable() {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            LevelCreator levelCreator = (LevelCreator)target;

            // Draw a custom dropdown for selecting the active module
            if (levelCreator.modules != null && levelCreator.modules.Length > 0)
            {
                string[] moduleNames = new string[levelCreator.modules.Length];
                for (int i = 0; i < levelCreator.modules.Length; i++)
                {
                    moduleNames[i] = (levelCreator.modules[i].objectPlacingPrefab != null)
                        ? levelCreator.modules[i].objectPlacingPrefab.name
                        : $"Module {i}";
                }
                SerializedProperty activeModuleIndexProp = serializedObject.FindProperty("activeModuleIndex");
                activeModuleIndexProp.intValue = EditorGUILayout.Popup("Active Module", activeModuleIndexProp.intValue, moduleNames);
            }
            else
            {
                EditorGUILayout.LabelField("No modules defined.");
            }

            // Iterate through properties and selectively draw them
            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true); // Skip "m_Script"

            while (property.NextVisible(false))
            {
                if (property.name == "activeModuleIndex") continue;

                // Conditional drawing for fillArea
                if (property.name == "areaSize" && !levelCreator.fillArea) continue;
                if (property.name == "density" && !levelCreator.fillArea) continue;

                // Conditional drawing for gridSize
                if (property.name == "gridSize" && !levelCreator.snapToGrid) continue;

                EditorGUILayout.PropertyField(property, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        private void OnSceneGUI(SceneView sceneView) {
            _creator = target as LevelCreator;
            if (_creator == null) return;
            if (!_creator.isPlacing) return;
        
            Event e = Event.current;
            Vector2 mousePosition = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
            if (_creator.snapToGrid) {
                mousePosition = new Vector2(Mathf.Round(mousePosition.x / _creator.gridSize) * _creator.gridSize, Mathf.Round(mousePosition.y / _creator.gridSize) * _creator.gridSize);
            }
            
            if (e.type == EventType.MouseDown && e.button == 0) {
                if (_creator.fillArea)
                {
                    FillArea(mousePosition);
                }
                else
                {
                    PlaceObject(mousePosition);
                }

                e.Use();
            } 
            if (e.type == EventType.Repaint && _creator.fillArea)
            {
                Color rectColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
                
                Handles.color = rectColor;

                Vector3 halfSize = new Vector3(_creator.areaSize / 2, _creator.areaSize / 2, 0);
                Vector3 rectCenter = new Vector3(mousePosition.x, mousePosition.y, 0);
                Vector3[] verts = new Vector3[]
                {
                    rectCenter + new Vector3(-halfSize.x, -halfSize.y, 0),
                    rectCenter + new Vector3(-halfSize.x, halfSize.y, 0),
                    rectCenter + new Vector3(halfSize.x, halfSize.y, 0),
                    rectCenter + new Vector3(halfSize.x, -halfSize.y, 0)
                };
                
                Handles.DrawSolidRectangleWithOutline(verts, rectColor, Color.black);
                
                
                SceneView.RepaintAll();
            }

            lastMousePosition = mousePosition;
        }

        private void FillArea(Vector2 center)
        {
            for (int i = 0; i < _creator.density * _creator.areaSize * _creator.areaSize; i++)
            {
                PlaceObject(new Vector2(center.x + Random.Range(-_creator.areaSize, _creator.areaSize) / 2, center.y + Random.Range(-_creator.areaSize, _creator.areaSize) / 2));
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