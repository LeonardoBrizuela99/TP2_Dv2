using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour
{
    private int _blockLayer;
    private bool _hasMoved = false;
    private Vector3 _initialPosition;
    private GameObject _firstBlock;

    [Header("Ajustes de Movimiento")]
    [SerializeField] private float targetHeightY = 0.2f;
    [SerializeField] private float delayBeforeAscent = 0.3f;

    private void Awake()
    {
        _blockLayer = LayerMask.NameToLayer("Block");
        _initialPosition = transform.position;
    }

    private void Start()
    {
        transform.position = _initialPosition;
        _hasMoved = false;
        _firstBlock = null;
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

            Block[] allBlocks = FindObjectsOfType<Block>();
            if (allBlocks.Length > 0)
            {
                _firstBlock = allBlocks[0].gameObject;
            }

            StartCoroutine(AscendWithDelay());
        }
    }

    private IEnumerator AscendWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeAscent);
        transform.position = new Vector3(transform.position.x, targetHeightY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasMoved && other.gameObject.layer == _blockLayer)
        {
            if (_firstBlock != null && other.gameObject == _firstBlock)
            {
                return;
            }

            GameEvents.TriggerBlockFailed();
            Destroy(other.gameObject);
        }
    }
}
