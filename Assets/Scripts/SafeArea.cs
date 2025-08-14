using UnityEngine;

public class SafeArea : MonoBehaviour
{
    [SerializeField] GameObject resultPanel;

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Caractor"))
        {
            if (!GameManager.isGameOver)
            {
                GameOver();
            }
            Destroy(collision.gameObject); // キャラクターを削除
        }
    }

    void GameOver()
    {
        // ゲームオーバー処理
        resultPanel.SetActive(true); // 結果パネルを表示
        GameManager.isGameOver = true; // ゲームオーバー状態を更新
    }

}
