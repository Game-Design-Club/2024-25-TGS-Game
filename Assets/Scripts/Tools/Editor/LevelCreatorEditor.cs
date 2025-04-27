using System;
using System.Linq;
using AppCore.DataManagement;
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

            _creator = (LevelCreator)target;

            // Draw a custom dropdown for selecting the active module
            if (_creator.modules != null && _creator.modules.Length > 0)
            {
                string[] moduleNames = new string[_creator.modules.Length];
                for (int i = 0; i < _creator.modules.Length; i++)
                {
                    moduleNames[i] = (_creator.modules[i].objectPlacingPrefab != null)
                        ? _creator.modules[i].objectPlacingPrefab.name
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
            string[] indentProperty = new[] { "areaSize", "density", "thickness", "areaShape", "gridSize", "useCurve", "curve", "strength", "gridOffset"};

            while (property.NextVisible(false))
            {
                if (property.name == "activeModuleIndex") continue;

                //Conditional drawing for isErasing 
                if (property.name == "useCurve" && !_creator.isErasing) continue;
                if (property.name == "strengthCurve" && (!_creator.isErasing || !_creator.useCurve)) continue;
                if (property.name == "strength" && (!_creator.isErasing || _creator.useCurve)) continue;
                
                // Conditional drawing for fillArea
                if (!_creator.useArea)
                {
                    if (property.name == "areaSize") continue;
                    if (property.name == "areaShape") continue;
                }
                
                //Misc conditional
                if (property.name == "density" && (_creator.isErasing || !_creator.useArea)) continue;
                if (property.name == "thickness" && (!_creator.useArea || _creator.areaShape != LevelCreator.Shape.Ring)) continue;
                


                // Conditional drawing for gridSize
                if (!_creator.snapToGrid)
                {
                    if (property.name == "gridSize") continue;
                    if (property.name == "gridOffset") continue;
                }

                //indentation adjustment
                if (indentProperty.Contains(property.name)) EditorGUI.indentLevel++;

                //deactivating useArea when isErasing
                if (_creator.isErasing && property.name == "useArea") EditorGUI.BeginDisabledGroup(true);
                
                EditorGUILayout.PropertyField(property, true);
                
                //Ending deactivation
                if (_creator.isErasing && property.name == "useArea") EditorGUI.EndDisabledGroup();

                //indentation reset
                EditorGUI.indentLevel = 0;
            }

            serializedObject.ApplyModifiedProperties();

        }
        
        private void OnSceneGUI(SceneView sceneView) {
            _creator = target as LevelCreator;
            if (_creator == null) return;
            if (!_creator.isActive) return;
        
            Event e = Event.current;
            Vector2 mousePosition = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
            
            //Changing position if snapping to grid
            if (_creator.snapToGrid) {
                mousePosition = new Vector2(Mathf.Round((mousePosition.x - _creator.gridOffset.x) / _creator.gridSize) * _creator.gridSize + _creator.gridOffset.x, Mathf.Round((mousePosition.y - _creator.gridOffset.y) / _creator.gridSize) * _creator.gridSize + _creator.gridOffset.y);
            }
            
            //adding/removing from scene
            if (e.type == EventType.MouseDown && e.button == 0) {
                if (_creator.useArea)
                {
                    if (!_creator.isErasing)
                    {
                        FillArea(mousePosition);
                    }
                    else
                    {
                        EraseArea(mousePosition);
                    }
                }
                else
                {
                    PlaceObject(mousePosition);
                }

                e.Use();
            } 
            
            //Drawing preview of area
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
            //Finding the area (unit^2) of the area
            float area = _creator.areaSize * _creator.areaSize;
            if (_creator.areaShape == LevelCreator.Shape.Circle || _creator.areaShape == LevelCreator.Shape.Ring)
            {
                area = Mathf.PI * (_creator.areaSize / 2) * (_creator.areaSize / 2);
                
                if (_creator.areaShape == LevelCreator.Shape.Ring) area -= Mathf.PI * (_creator.areaSize / 2 - _creator.thickness) * (_creator.areaSize / 2 - _creator.thickness);
            }
            
            //randomly generating new objects
            for (int i = 0; i < _creator.density * area; i++)
            {
                Vector2 position = new Vector2();

                //generation dependant on shape
                if (_creator.areaShape == LevelCreator.Shape.Square)
                {
                    position = new Vector2(center.x + Random.Range(-_creator.areaSize, _creator.areaSize) / 2,
                        center.y + Random.Range(-_creator.areaSize, _creator.areaSize) / 2);
                }else if (_creator.areaShape == LevelCreator.Shape.Circle ||
                          _creator.areaShape == LevelCreator.Shape.Ring)
                {
                    //Generation of uniformly random points in a circle had help from El Señor GPT
                    
                    // Random angle between 0 and 2 * PI
                    float angle = Random.Range(0f, 2f * Mathf.PI);
                    
                    // Random radius with a uniform distribution
                    float radiusMultiplier = Mathf.Sqrt(Random.Range(0f, 1f));

                    //Adjustment for ring if needed
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
                
                DataID dataID = newObj.GetComponent<DataID>();
                if (dataID != null)
                {
                    dataID.GenerateID();
                }
                
                
                Undo.RegisterCreatedObjectUndo(newObj, "Place Prefab");
            }
        }

        private void RandomizeObject(GameObject newObj) {
            newObj.TryGetComponent(out SpriteChooser spriteChooser);
            if (spriteChooser != null) {
                if (_creator.UseFlags) {
                    spriteChooser.RandomizeSprite(_creator.CustomRandomizeFlags);
                } else {
                    spriteChooser.RandomizeSprite();
                }
            }
        }

        private void EraseArea(Vector2 center)
        {
            Debug.Log("Erase Area");
            //Finding objects
            Collider2D[] colliders;
            if (_creator.areaShape == LevelCreator.Shape.Circle || _creator.areaShape == LevelCreator.Shape.Ring)
            {
                colliders = Physics2D.OverlapCircleAll(center, _creator.areaSize / 2);
            }
            else if (_creator.areaShape == LevelCreator.Shape.Square)
            {
                colliders = Physics2D.OverlapBoxAll(center, new Vector2(_creator.areaSize, _creator.areaSize), 0);
            }
            else
            {
                return;
            }

            Debug.Log(colliders.Length);
            //
            float maxDist = 0;
            if (_creator.useCurve) switch (_creator.areaShape)
            {
                case LevelCreator.Shape.Square:
                    maxDist = _creator.areaSize / 2;
                    break;
                case LevelCreator.Shape.Circle:
                    maxDist = _creator.areaSize / 2;
                    break;
                case LevelCreator.Shape.Ring:
                    maxDist = _creator.thickness;
                    break;
            }
            
            foreach (Collider2D col in colliders)
            {
                if (!col) continue;
                GameObject gameObject = col.gameObject;
                Vector2 diff = (Vector2) gameObject.transform.position - center;
    
                if (_creator.areaShape == LevelCreator.Shape.Ring &&
                    diff.magnitude < _creator.areaSize / 2 - _creator.thickness)
                    continue;
    
                // Determine the threshold using your existing logic
                float dist = 0;
                if (_creator.useCurve)
                {
                    switch (_creator.areaShape)
                    {
                        case LevelCreator.Shape.Square:
                            dist = Mathf.Max(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
                            break;
                        case LevelCreator.Shape.Circle:
                            dist = diff.magnitude;
                            break;
                        case LevelCreator.Shape.Ring:
                            dist = diff.magnitude - (_creator.areaSize / 2 - _creator.thickness);
                            break;
                    }
                }
                float threshold = !_creator.useCurve
                    ? _creator.strength
                    : Mathf.Clamp(_creator.strengthCurve.Evaluate(dist / maxDist), 0f, 1f);
    
                
                string instancePrefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
                string treePrefabPath = AssetDatabase.GetAssetPath(_creator.ObjectPlacingPrefab);
    
                if (!string.IsNullOrEmpty(instancePrefabPath) && instancePrefabPath == treePrefabPath && Random.Range(0f, 1f) < threshold)
                {
                    Undo.RegisterFullObjectHierarchyUndo(gameObject, "Erase Object");
                    Undo.DestroyObjectImmediate(gameObject);
                }
            }
        }

    }
}