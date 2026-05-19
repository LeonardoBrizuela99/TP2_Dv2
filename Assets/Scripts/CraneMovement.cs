using UnityEngine;

public class CraneMovement : MonoBehaviour
{
   
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float range = 5.0f;

   
    [SerializeField] private float verticalOffset = 8f;
    [SerializeField] private float lerpSpeed = 2.5f;
    [SerializeField] private float safeMargin = 3.0f;

   
    [SerializeField] private GameObject[] blockPrefabs;
    [SerializeField] private Transform spawnPoint;

    private Block _currentBlock;
    private float _targetHeight;

    private void Start()
    {
        _targetHeight = transform.position.y;
        SpawnNewBlock();
    }

    private void OnEnable() => GameEvents.OnBlockLanded += RegisterBlockLanding;
    private void OnDisable() => GameEvents.OnBlockLanded -= RegisterBlockLanding;

    private void Update()
    {
        
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null && gm.IsGameOver)
        {
            return;
        }

        float x = Mathf.PingPong(Time.time * speed, range * 2) - range;

      
        float newY = Mathf.Lerp(transform.position.y, _targetHeight, Time.deltaTime * lerpSpeed);

        transform.position = new Vector3(x, newY, transform.position.z);

        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            DropBlock();
        }
    }

    private void RegisterBlockLanding(float landedHeight, float errorX)
    {
        if (landedHeight < safeMargin) return;
        float potentialNewHeight = landedHeight + verticalOffset;
        if (potentialNewHeight > _targetHeight)
        {
            _targetHeight = potentialNewHeight;
        }
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

    private void SpawnNewBlock()
    {
        if (blockPrefabs == null || blockPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, blockPrefabs.Length);
        GameObject selectedPrefab = blockPrefabs[randomIndex];

        GameObject go = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity, transform);
        _currentBlock = go.GetComponent<Block>();
        go.layer = LayerMask.NameToLayer("Block");
    }
}
