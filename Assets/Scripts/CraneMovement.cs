using UnityEngine;

public class CraneMovement : MonoBehaviour
{   
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float range = 5.0f;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Transform spawnPoint;

    private Block _currentBlock;

    private void Start()
    {
        SpawnNewBlock();
    }

    private void Update()
    {
        UpdateMovement();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            HandleDrop();
        }
    }

    private void UpdateMovement()
    {
        float x = Mathf.PingPong(Time.time * speed, range * 2) - range;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    private void HandleDrop()
    {
        if (_currentBlock != null)
        {
            _currentBlock.Drop();
            _currentBlock = null;

            Invoke(nameof(SpawnNewBlock), 1.2f);
        }
    }

    private void SpawnNewBlock()
    {
        GameObject newBlockGO = Instantiate(blockPrefab, spawnPoint.position, Quaternion.identity, transform);
        _currentBlock = newBlockGO.GetComponent<Block>();
    }
    private void DropBlock()
    {
        if (_currentBlock != null)
        {
            _currentBlock.transform.position = spawnPoint.position;
            _currentBlock.Drop();
            _currentBlock = null;
            Invoke(nameof(SpawnNewBlock), 1.2f);
        }
    }
}