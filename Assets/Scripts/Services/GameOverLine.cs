using UnityEngine;
using System.Collections;
using Zenject;

public class GameOverLine : MonoBehaviour
{
    public float requiredTime = 3f; // 3秒
    private Coroutine checkCoroutine;

    [Inject] private GameManager _gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Shape>(out var shape))
        {
            // 既にコルーチンが回っている場合はスキップ
            if (checkCoroutine == null)
                Debug.Log("カウントちゅう");
                checkCoroutine = StartCoroutine(CheckStay(shape));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Shape>(out var shape))
        {
            // 3秒経たないうちに離れたらコルーチンを止める
            if (checkCoroutine != null)
            {
                StopCoroutine(checkCoroutine);
                checkCoroutine = null;
            }
        }
    }

    private IEnumerator CheckStay(Shape shape)
    {
        float timer = 0f;

        while (timer < requiredTime)
        {
            timer += Time.deltaTime;
            yield return null;

            // オブジェクトが離れたら中断
            if (shape == null) yield break;
        }

        // 3秒間触れ続けた → ゲームオーバー
        _gameManager.GameOver();
        checkCoroutine = null;
    }
}
