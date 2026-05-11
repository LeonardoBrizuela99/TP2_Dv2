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

        // 1. Lo soltamos del padre
        transform.SetParent(null);

        // 2. Activamos gravedad
        _rb.isKinematic = false;

        // 3. LA CLAVE: Cortamos toda velocidad heredada
        // Usamos linearVelocity para Unity 6
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
}
