using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Block : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _hasLanded = false;
    private bool _isDropped = false;

    private int _floorLayer;
    private int _blockLayer;
    private int _baseLayer;

    [Header("Ajustes de Juego")]
    [SerializeField] private float snapThreshold = 0.25f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;

        _floorLayer = LayerMask.NameToLayer("Floor");
        _blockLayer = LayerMask.NameToLayer("Block");
        _baseLayer = LayerMask.NameToLayer("Base");
    }

    private void OnEnable()
    {
        
        GameEvents.OnPerfectDrop += StabilizeBlock;
    }

    private void OnDisable()
    {
       
        GameEvents.OnPerfectDrop -= StabilizeBlock;
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

        if (otherLayer == _blockLayer || otherLayer == _baseLayer)
        {
            _hasLanded = true;

            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            Vector3 targetCenter = collision.collider.bounds.center;
            float errorX = Mathf.Abs(transform.position.x - targetCenter.x);

            if (errorX < snapThreshold)
            {
              
                transform.position = new Vector3(targetCenter.x, transform.position.y, transform.position.z);
                _rb.constraints = RigidbodyConstraints.FreezeAll;

              
                GameEvents.TriggerPerfectDrop();
            }
            else
            {
              
                _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                _rb.mass = 50f;
            }

            if (otherLayer == _blockLayer)
            {
                GameEvents.TriggerBlockLanded(transform.position.y);
            }
        }
        else if (otherLayer == _floorLayer)
        {
            _hasLanded = true;
            Destroy(gameObject, 3f);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_hasLanded && _rb.constraints != RigidbodyConstraints.None)
        {
            if (collision.gameObject.layer == _blockLayer)
            {
                Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();
                if (otherRb != null && otherRb.constraints != RigidbodyConstraints.FreezeAll)
                {
                    _rb.constraints = RigidbodyConstraints.None;
                }
            }
        }
    }

  
    private void StabilizeBlock()
    {
      
        if (_hasLanded && _rb != null && _rb.constraints != RigidbodyConstraints.None)
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }
}
