using System;
using System.Diagnostics;
using System.Linq;
using AppCore.DataManagement;
using Game.GameManagement;
using Tools.LevelDesign;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;


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
            string[] indentProperty = new[] { "areaSize", "density", "thickness", "areaShape", "gridSize" };

            while (property.NextVisible(false))
            {
                if (property.name == "activeModuleIndex") continue;

                // Conditional drawing for fillArea
                if (!levelCreator.useArea)
                {
                    if (property.name == "areaSize") continue;
                    if (property.name == "density") continue;
                    if (property.name == "areaShape") continue;
                }
                
                if (property.name == "thickness" && (!levelCreator.useArea || levelCreator.areaShape != LevelCreator.Shape.Ring)) continue;
                


                // Conditional drawing for gridSize
                if (property.name == "gridSize" && !levelCreator.snapToGrid) continue;

                if (indentProperty.Contains(property.name)) EditorGUI.indentLevel++;
                
                EditorGUILayout.PropertyField(property, true);
                
                EditorGUI.indentLevel = 0;
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
                if (_creator.useArea)
                {
                    FillArea(mousePosition);
                }
                else
                {
                    PlaceObject(mousePosition);
                }

                e.Use();
            } 
            if (e.type == EventType.Repaint && _creator.useArea)
            {
                Color color = new Color(1f, 1f, 1f, 0.3f);
                
                Handles.color = color;
                
                if (_creator.areaShape == LevelCreator.Shape.Square)
                {
                    Vector3 halfSize = new Vector3(_creator.areaSize / 2, _creator.areaSize / 2, 0);
                    Vector3 rectCenter = new Vector3(mousePosition.x, mousePosition.y, 0);
                    Vector3[] verts = new Vector3[]
                    {
                        rectCenter + new Vector3(-halfSize.x, -halfSize.y, 0),
                        rectCenter + new Vector3(-halfSize.x, halfSize.y, 0),
                        rectCenter + new Vector3(halfSize.x, halfSize.y, 0),
                        rectCenter + new Vector3(halfSize.x, -halfSize.y, 0)
                    };

                    Handles.DrawSolidRectangleWithOutline(verts, color, Color.black);
                }else if (_creator.areaShape == LevelCreator.Shape.Circle)
                {
                    float radius = _creator.areaSize / 2;
                    
                    Handles.DrawSolidDisc(mousePosition, Vector3.forward, radius);
                }else if (_creator.areaShape == LevelCreator.Shape.Ring)
                {
                    float radius = (_creator.areaSize - _creator.thickness) / 2;
                    
                    float thickness =
                        Screen.height / (2f * SceneView.lastActiveSceneView.camera.orthographicSize)  *
                        (_creator.thickness / 2);
                    
                    Handles.DrawWireDisc(mousePosition, Vector3.forward, radius, thickness);
                    
                }

                SceneView.RepaintAll();
            }

            lastMousePosition = mousePosition;
        }

        private void FillArea(Vector2 center)
        {
            float area = _creator.areaSize * _creator.areaSize;
            if (_creator.areaShape == LevelCreator.Shape.Circle || _creator.areaShape == LevelCreator.Shape.Ring)
            {
                area = Mathf.PI * (_creator.areaSize / 2) * (_creator.areaSize / 2);
                
                if (_creator.areaShape == LevelCreator.Shape.Ring) area -= Mathf.PI * (_creator.areaSize / 2 - _creator.thickness) * (_creator.areaSize / 2 - _creator.thickness);
            }
            for (int i = 0; i < _creator.density * area; i++)
            {
                Vector2 position = new Vector2();

                if (_creator.areaShape == LevelCreator.Shape.Square)
                {
                    position = new Vector2(center.x + Random.Range(-_creator.areaSize, _creator.areaSize) / 2,
                        center.y + Random.Range(-_creator.areaSize, _creator.areaSize) / 2);
                }else if (_creator.areaShape == LevelCreator.Shape.Circle ||
                          _creator.areaShape == LevelCreator.Shape.Ring)
                {
                    // Random angle between 0 and 2 * PI
                    float angle = Random.Range(0f, 2f * Mathf.PI);
                    
                    // Random radius with a uniform distribution
                    float radiusMultiplier = Mathf.Sqrt(Random.Range(0f, 1f));

                    float radius = _creator.areaShape == LevelCreator.Shape.Ring
                        ? _creator.areaSize / 2 - (1 - radiusMultiplier) * _creator.thickness
                        : radiusMultiplier * (_creator.areaSize / 2);
                    
                    // Convert polar coordinates (angle, r) to Cartesian coordinates (x, y)
                    float x = radius * Mathf.Cos(angle);
                    float y = radius * Mathf.Sin(angle);

                    position = center + new Vector2(x, y);
                }
                else
                {
                    throw new ArgumentException($"Area Shape: {_creator.areaShape} does not exist");
                }
                
                PlaceObject(position);
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

                if (_creator.RandomizeFlip)
                {
                    if (newObj.TryGetComponent(out SpriteChooser spriteChooser))
                    {
                        spriteChooser.RandomizeFlip();
                    }else if (newObj.TryGetComponent(out SpriteRenderer spriteRenderer))
                    {
                        spriteRenderer.flipX = spriteRenderer.flipX = Random.Range(0, 2) == 0;
                    }
                }
                
                if (newObj.TryGetComponent(out DestroyIfSavedFlag destroyIfSavedFlag))
                {
                    destroyIfSavedFlag.GenerateID();
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
        
        // private void RandomizeFlip(GameObject newObj)
    }
}