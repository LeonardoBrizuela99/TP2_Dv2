using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private int _blockLayer;
    private bool _hasMoved = false;

    [SerializeField] private float targetHeightY = 0.2f;      
    [SerializeField] private float safetyDistanceX = 0.8f;    

    private void Awake()
    {
        _blockLayer = LayerMask.NameToLayer("Block");
    }

    private void OnEnable()
    {
       
        GameEvents.OnBlockLanded += HandleBlockLanded;
    }

    private void OnDisable()
    {
        GameEvents.OnBlockLanded -= HandleBlockLanded;
    }

    private void HandleBlockLanded(float landedHeight, float errorX)
    {
       
        if (!_hasMoved)
        {
            _hasMoved = true;
           
            Invoke(nameof(MoveUp), 0.2f);
        }
    }

    private void MoveUp()
    {
        transform.position = new Vector3(transform.position.x, targetHeightY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (_hasMoved && other.gameObject.layer == _blockLayer)
        {
          
            if (Mathf.Abs(other.transform.position.x) < safetyDistanceX)
            {
                return;
            }
            GameEvents.TriggerBlockFailed();
            Destroy(other.gameObject);
        }
    }
}
