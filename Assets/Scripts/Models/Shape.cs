using UnityEngine;
using Zenject;

public class Shape : MonoBehaviour
{
    public int Level { get; private set; }

    [SerializeField] private ObjectPrefabType shapeType;
    [SerializeField] private GameObject nextPrefab;

    public int ScoreValue => Level * 10;

    public ObjectPrefabType Type => shapeType;

    private Rigidbody2D rb;
    private Collider2D col;

    [Inject] private GameManager _gameManager;

    [Inject] private DiContainer _container; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void Initialize(int level)
    {
        Level = level;
        ApplyScaleByLevel();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Shape>(out var other))
        {
            // 自分の方がID低い場合のみMerge実行
            if (this.GetInstanceID() < other.GetInstanceID())
            {
                // 同じ種類 ＋ 同じレベル ならマージ
                if (other.Level == this.Level && other.Type == this.Type)
                {
                    Merge(other);
                }
            }
        }
    }
    
    public void ApplyScaleByLevel()
    {
        float scale = 1f + (Level - 1) * 0.2f; // レベルが上がるごとに20%拡大
        Debug.Log("Level:"+Level+" " + "scale:"+ scale);
        transform.localScale = new Vector3(scale, scale, 1);
    }

    private void Merge(Shape other)
    {

        Destroy(other.gameObject);
        Destroy(this.gameObject);

        if (nextPrefab != null)
        {
            GameObject newObj = _container.InstantiatePrefab(nextPrefab, transform.position, Quaternion.identity, null);

            if (newObj.TryGetComponent<Shape>(out var shape))
            {
                shape.Initialize(Level + 1);

                _gameManager.AddScore(shape.ScoreValue);
            }
        }
    }
}
