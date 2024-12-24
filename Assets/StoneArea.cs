using System;
using DefaultNamespace;
using UnityEngine;

public class StoneArea : MonoBehaviour {
    [SerializeField] private float dist = 0.5f;
    private void OnTriggerStay2D(Collider2D other) {
        if (other.TryGetComponent(out Stone stone)) {
            float distance = Vector2.Distance(other.transform.position, transform.position);
            Debug.Log(distance);
            if (stone.inWater) return;
            Debug.Log("In water");
            if (distance < dist) {
                stone.InWater(transform.position);
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
