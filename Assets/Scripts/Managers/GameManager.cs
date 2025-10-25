using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] ObjectGameObjectSet objectGameObjectSet;

    [Inject] private DiContainer _container;

    public int TotalScore { get; private set; }

    private GameObject previewObj;

    private GameObject spawnObj;

    [SerializeField] private TMP_Text scoreText;

    private bool gameOverFlg = false;

    public bool btnDisFlg = false;

    [Inject] private RestartButton _restartButton;

    private bool canSpawn = true;   // 1秒ごとにリセットされるフラグ
    public float spawnInterval = 1f;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        previewObj = GetRandomAndShowTopLeft();

        _restartButton.ShowJudge(btnDisFlg);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameOverFlg == false && canSpawn) // 左クリック
        {
            SpawnAtClick();
            StartCoroutine(SpawnCooldown());
        }
    }

    // クールダウン用コルーチン
    private IEnumerator SpawnCooldown()
    {
        canSpawn = false;              // しばらく落とせない
        yield return new WaitForSeconds(spawnInterval); // 1秒待つ
        canSpawn = true;               // また落とせる
    }

    void SpawnAtClick()
    {
        Vector3 spawnPos = GetWorldPositionFromMouse2D();
        spawnObj = _container.InstantiatePrefab(previewObj, spawnPos, Quaternion.identity, null);

        // 落とすときに Rigidbody を有効化！
        EnablePhysics(spawnObj);


        // Shape を取得してスケール調整
        if (spawnObj.TryGetComponent<Shape>(out var shape))
        {
            shape.Initialize(1);
            shape.ApplyScaleByLevel();
        }

        Destroy(previewObj);
        previewObj = GetRandomAndShowTopLeft();
        Debug.Log($"previewObj: {previewObj.name}");
    }

    Vector3 GetWorldPositionFromMouse2D()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mousePos.y = 3;
        return mousePos;
    }

    // ランダムで1つ返すメソッド
    public GameObject GetRandom()
    {
        int rand = Random.Range(0, 3);

        switch (rand)
        {
            case 0: return objectGameObjectSet.GetGameObject(ObjectPrefabType.Capsule);
            case 1: return objectGameObjectSet.GetGameObject(ObjectPrefabType.Circle);
            case 2: return objectGameObjectSet.GetGameObject(ObjectPrefabType.Square);
            default: return null;
        }
    }

    public GameObject GetRandomAndShowTopLeft()
    {
        GameObject prefab = GetRandom();

        // カメラの左上のワールド座標を求める
        Camera cam = Camera.main;
        Vector3 topLeft = new Vector3(-8, 3, 0);

        // Z位置を調整（2Dなら0でOK）
        topLeft.z = 0;

        // 生成
        GameObject obj = _container.InstantiatePrefab(prefab, topLeft, Quaternion.identity, null);

        DisablePhysics(obj);

        return obj;
    }

    // Rigidbody2D / Rigidbody / Collider を無効化する共通メソッド
    private void DisablePhysics(GameObject obj)
    {
        if (obj.TryGetComponent<Rigidbody2D>(out var rb2d))
        {
            rb2d.simulated = false;    // これで2D物理を止められる
        }
    }

    //Rigidbody2D / Rigidbody / Collider を有効化する共通メソッド
    private void EnablePhysics(GameObject obj)
    {
        if (obj.TryGetComponent<Rigidbody2D>(out var rb2d))
        {
            rb2d.simulated = true;
        }
    }

    public void AddScore(int score)
    {
        TotalScore += score;
        UpdateScore();
    }

    private void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + TotalScore;
    }

    public void GameOver()
    {
        gameOverFlg = true;
        btnDisFlg = true;

        _restartButton.ShowJudge(btnDisFlg);

        scoreText.text = "GameOver!! TotalScore: " + TotalScore;
    }
}
