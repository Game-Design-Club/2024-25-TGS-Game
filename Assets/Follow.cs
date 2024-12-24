using UnityEngine;

public class Follow : MonoBehaviour {
    [SerializeField] private Transform target;

    void Update() {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
