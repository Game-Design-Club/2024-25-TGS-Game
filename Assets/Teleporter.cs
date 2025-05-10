using System;
using Game.GameManagement;
using Tools;
using UnityEngine;

public class Teleporter : MonoBehaviour {
    [SerializeField] private Transform location;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(Tags.Child)) {
            LevelManager.GetCurrentLevel().child.Rigidbody.position = location.position;
        }
    }
}
