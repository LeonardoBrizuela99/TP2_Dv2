using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour
{
    private int _blockLayer;
    private bool _hasMoved = false;
    private Vector3 _initialPosition;

    // Guardamos la referencia exacta del primer bloque para que sea inmune
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
        // Reseteo forzado al subsuelo
        transform.position = _initialPosition;
        _hasMoved = false;
        _firstBlock = null;
    }

    private void OnEnable()
    {
        // Nos suscribimos al evento nativo de colisión del bloque
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

            // TRUCO SENIOR: Buscamos el último bloque que acaba de aterrizar en este frame
            // y lo guardamos como el primer bloque de la torre para darle inmunidad total
            Block[] allBlocks = FindObjectsOfType<Block>();
            if (allBlocks.Length > 0)
            {
                // El primer bloque colocado es el que está más abajo en el eje Y
                _firstBlock = allBlocks[0].gameObject;
            }

            StartCoroutine(AscendWithDelay());
        }
    }

    private IEnumerator AscendWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeAscent);
        transform.position = new Vector3(transform.position.x, targetHeightY, transform.position.z);
        Debug.Log("DeathZone: Posicionada arriba en la superficie de forma segura.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasMoved && other.gameObject.layer == _blockLayer)
        {
            // REGLA DE ORO DE INMUNIDAD: Si el objeto que tocó el trigger es el PRIMER bloque,
            // lo ignoramos por completo. Jamás lo va a destruir ni va a dar GameOver.
            if (_firstBlock != null && other.gameObject == _firstBlock)
            {
                return;
            }

            // Si es cualquier otro bloque (el segundo, tercero, etc.) que cayó fuera de la torre, pierde
            GameEvents.TriggerBlockFailed();
            Destroy(other.gameObject);
        }
    }
}
