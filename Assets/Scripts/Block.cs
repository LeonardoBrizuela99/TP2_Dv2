using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Block : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _col;
    private bool _hasLanded = false;
    private bool _isDropped = false;
    private bool _hasFailed = false; 

    private int _floorLayer;
    private int _blockLayer;
    private int _baseLayer;

    
    [SerializeField] private float perfectOverlap = 85.0f;
    [SerializeField] private float goodOverlap = 40.0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _rb.isKinematic = true;

        _floorLayer = LayerMask.NameToLayer("Floor");
        _blockLayer = LayerMask.NameToLayer("Block");
        _baseLayer = LayerMask.NameToLayer("Base");
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
        int otherLayer = collision.gameObject.layer;

        
        if (_hasFailed)
        {
            if (otherLayer == _blockLayer || otherLayer == _baseLayer || otherLayer == _floorLayer)
            {
                GameEvents.TriggerBlockFailed();
                Destroy(gameObject);
                return;
            }
        }

        if (_hasLanded) return;

        if (otherLayer == _blockLayer || otherLayer == _baseLayer)
        {
            _hasLanded = true;

            if (otherLayer == _baseLayer)
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll;
                GameEvents.TriggerBlockLanded(transform.position.y, 0f);
                return;
            }

            EvaluatePlacement(collision);
        }
        else if (otherLayer == _floorLayer)
        {
            _hasLanded = true;
            GameEvents.TriggerBlockFailed();
            Destroy(gameObject);
        }
    }

    private void EvaluatePlacement(Collision collision)
    {
        float overlapPercentage = GetOverlapPercentage(collision);

        if (overlapPercentage > perfectOverlap)
        {
            
            Vector3 targetCenter = collision.collider.bounds.center;
            transform.position = new Vector3(targetCenter.x, transform.position.y, transform.position.z);
            _rb.constraints = RigidbodyConstraints.FreezeAll;

            GameEvents.TriggerPerfectDrop();
            GameEvents.TriggerBlockLanded(transform.position.y, 0f);
        }
        else if (overlapPercentage > goodOverlap)
        {
           
            _rb.constraints = RigidbodyConstraints.FreezeAll;

            float errorX = Mathf.Abs(transform.position.x - collision.collider.bounds.center.x);
            GameEvents.TriggerBlockLanded(transform.position.y, errorX);
        }
        else
        {
         
            _rb.isKinematic = false;
            _rb.constraints = RigidbodyConstraints.None;
            _rb.mass = 20f;

           
            _hasFailed = true;

           
            GameEvents.TriggerBlockFailed();

          
            float pushDirection = Mathf.Sign(transform.position.x - collision.collider.bounds.center.x);
            _rb.linearVelocity = new Vector3(pushDirection * 2f, _rb.linearVelocity.y, _rb.linearVelocity.z);

           
            Destroy(gameObject, 2f);
        }
    }

    private float GetOverlapPercentage(Collision collision)
    {
        float thisLeftEdge = _col.bounds.min.x;
        float thisRightEdge = _col.bounds.max.x;
        float otherLeftEdge = collision.collider.bounds.min.x;
        float otherRightEdge = collision.collider.bounds.max.x;

        float overlapMin = Mathf.Max(thisLeftEdge, otherLeftEdge);
        float overlapMax = Mathf.Min(thisRightEdge, otherRightEdge);
        float overlap = Mathf.Max(0.0f, overlapMax - overlapMin);

        float towerWidth = _col.bounds.size.x;
        return Mathf.Clamp((overlap / towerWidth) * 100.0f, 0.0f, 100.0f);
    }
}
