using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float lerpSpeed = 3.0f;
    [SerializeField] private float verticalOffset = -4.0f;

    private float _initialX;
    private float _initialZ;

    private void Start()
    {
        _initialX = transform.position.x;
        _initialZ = transform.position.z;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        float targetY = target.position.y + verticalOffset;

        if (targetY > transform.position.y)
        {
            float newY = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * lerpSpeed);
            transform.position = new Vector3(_initialX, newY, _initialZ);
        }
    }
}
