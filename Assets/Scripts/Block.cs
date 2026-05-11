using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Block : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _hasLanded = false;
    private bool _isDropped = false;

    private int _floorLayer;
    private int _blockLayer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;

        _floorLayer = LayerMask.NameToLayer("Floor");
        _blockLayer = LayerMask.NameToLayer("Block");
    }

    public void Drop()
    {
        if (_isDropped) return;
        _isDropped = true;

        transform.SetParent(null);
        _rb.isKinematic = false;

        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasLanded) return;
        int otherLayer = collision.gameObject.layer;
        if (otherLayer == _floorLayer || otherLayer == _blockLayer)
        {
            _hasLanded = true;
          
            _rb.constraints = RigidbodyConstraints.FreezeRotationY;
            _rb.mass = 20f;

            if (otherLayer == _blockLayer)
                GameEvents.TriggerBlockLanded(transform.position.y);
        }
    }
}
