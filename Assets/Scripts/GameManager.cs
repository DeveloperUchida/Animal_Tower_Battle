using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] characters; //キャラクターのオブジェクト配列
    bool isGene = false; //キャラクターが生成されたかどうかのフラグ判定
    GameObject geneChara; //生成されたキャラクター単体;
    bool isInterval = false; //キャラクター生成の間隔を制御するフラグ

    // Update is called once per frame
    void Update()
    {
        //キャラクターが生成されていないかつキャラが静止している場合
        if (!isGene && !isInterval && !CheckMove())
        {
            CreateCaractor(); //キャラクターを生成
            isGene = true;
        }
        //マウスの左ボタンが離されたとき、かつキャラクターが生成されている場合
        else if (Input.GetMouseButtonUp(0) && isGene)
        {
            //物理挙動を有効にする
            geneChara.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            isGene = false; //キャラクター生成フラグをリセット
            StartCoroutine(IntervalCoroutine()); //キャラクター生成の間隔を制御
        }
        //⬇️ここから追加⬇️
        else if (Input.GetMouseButton(0) && isGene)
        {
            //マウスの左ボタンが押されている間、キャラクターを移動させる(x座標のみ)
            float mousePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            geneChara.transform.position = new Vector2(mousePositionX, transform.position.y);
        }
    }


    void CreateCaractor()
    {
        //回転せずにGameManagerの座標にランダムにキャラ生成
        geneChara = Instantiate(characters[Random.Range(0, characters.Length)],
        transform.position, Quaternion.identity);
        //物理挙動をさせない状態にする
        geneChara.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    IEnumerator IntervalCoroutine()
    {
        isInterval = true; //間隔制御フラグを立てる
        yield return new WaitForSeconds(1); //1秒待機
        isInterval = false; //間隔制御フラグをリセット
    }

    bool CheckMove()
    {
        //Characterタグのオブジェクトを取得
        GameObject[] characterObjects = GameObject.FindGameObjectsWithTag("Caractor");
        foreach (GameObject character in characterObjects)
        {
            //キャラクターの速度が0.001以上なら動いていると判断
            if (character.GetComponent<Rigidbody2D>().velocity.magnitude > 0.001f)
            {
                return true; //キャラクターが動いている場合はtrue
            }
        }
        return false; //キャラクターが動いていない場合はfalse
    }

}


