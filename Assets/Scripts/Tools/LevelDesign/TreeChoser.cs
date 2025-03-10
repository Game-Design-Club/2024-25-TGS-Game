using System;
using UnityEngine;

namespace Tools.LevelDesign {
    public class TreeChoser : MonoBehaviour {
        [SerializeField] private TreeType treeType;

        private void OnValidate() {
            TreeChoserData treeChoserData = Resources.Load<TreeChoserData>("TreeChooserData");
            
            if (TryGetComponent(out SpriteRenderer spriteRenderer)) {
                spriteRenderer.sprite = treeType switch {
                    TreeType.Tree1 => treeChoserData.tree1,
                    TreeType.Tree2 => treeChoserData.tree2,
                    TreeType.Tree3 => treeChoserData.tree3,
                    TreeType.Tree4 => treeChoserData.tree4,
                    _ => spriteRenderer.sprite
                };
            } else {
                Debug.LogError($"{nameof(TreeChoser)} has no sprite renderer");
            }
        }
    }
    
    public enum TreeType {
        Tree1,
        Tree2,
        Tree3,
        Tree4
    }
}