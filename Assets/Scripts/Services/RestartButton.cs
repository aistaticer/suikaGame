using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class RestartButton : MonoBehaviour
{


    void Start()
    {
        // Buttonコンポーネントを取得してクリック時の処理を登録
        GetComponent<Button>().onClick.AddListener(RestartGame);
    }

    private void RestartGame()
    {
        // 現在のシーンを再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void ShowJudge(bool showFlg)
    {
        if (showFlg)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
