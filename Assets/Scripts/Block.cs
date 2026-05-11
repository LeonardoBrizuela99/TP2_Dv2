using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Block : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _isDropped = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
      
        _rb.isKinematic = true;
    }

    public void Drop()
    {
        if (_isDropped) return;

        _isDropped = true; 
        transform.SetParent(null);
        _rb.isKinematic = false;
    }
}
