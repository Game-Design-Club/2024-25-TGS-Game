using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float speed = 5;
    private Rigidbody2D _rb;
    void Awake() {
        TryGetComponent(out _rb);}
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow)) {
            _rb.position += new Vector2(0, 1) * (Time.fixedDeltaTime * speed);
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            _rb.position += new Vector2(0, -1) * (Time.fixedDeltaTime * speed);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            _rb.position += new Vector2(-1, 0) * (Time.fixedDeltaTime * speed);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            _rb.position += new Vector2(1, 0) * (Time.fixedDeltaTime * speed);
        }

    }
}
