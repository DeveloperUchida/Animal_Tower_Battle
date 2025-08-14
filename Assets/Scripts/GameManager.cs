using UnityEngine;
using System.Collections;
using TMPro;
using System.Data.Common;

[RequireComponent(typeof(AudioSource))] //AudioSourceコンポーネントが必要
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] characters; //キャラクターのオブジェクト配列
    bool isGene = false; //キャラクターが生成されたかどうかのフラグ判定
    GameObject geneChara; //生成されたキャラクター単体;
    bool isInterval = false; //キャラクター生成の間隔を制御するフラグ
    bool isButtonHover = false; //ボタンがホバーされているかどうかを見る
    bool isButtonReleasedOnButton = false; //ボタンから指を離した直後
    public static bool isGameOver = false; //ゲームオーバー状態を管理
    int score; //スコアを管理
    [SerializeField] TextMeshProUGUI scoreText; //スコア表示用のTextMeshProUGUI
    bool isGameStarted = false; //ゲーム開始状態を管理

    [SerializeField] AudioClip fall_se;
    [SerializeField] AudioClip rotate_se;
    AudioSource audioSource;

    void Start()
    {
        //ゲーム開始時の初期化
        isGameOver = false; //ゲーム開始時はゲームオーバーではない
        isGene = false; //キャラクター生成フラグをリセット
        isInterval = false; //間隔制御フラグをリセット
        isButtonHover = false; //ボタンのホバー状態をリセット

        audioSource = GetComponent<AudioSource>();

        score = 0; //スコア情報初期値

        isGameStarted = false; //ゲームが始まったかのフラグ
        scoreText.text = score.ToString();//スコアテキストの初期化
    }
    // Update is called once per frame
    void Update()
    {
        //ゲームオーバーならこの先処理をさせない
        if (isGameOver)
        {
            return;
        }

        //キャラクターが生成されていないかつキャラが静止している場合
        if (!isGene && !isInterval && !CheckMove())
        {
            CreateCaractor(); //キャラクターを生成
            isGene = true;
            if (isGameStarted)

                UpdateScore();
            else
                isGameStarted = true;
        }
        //マウスの左ボタンが離されたとき、かつキャラクターが生成されている場合
        else if (Input.GetMouseButtonUp(0) && isGene && !isButtonHover)
        {

            if (isButtonReleasedOnButton)
            {
                isButtonReleasedOnButton = false; //ボタンから指を離した直後のフラグをリセット
                return; //ボタンから離した直後は何もしない
            }

            //物理挙動を有効にする
            geneChara.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            isGene = false; //キャラクター生成フラグをリセット
            audioSource.PlayOneShot(fall_se); //落下音を再生
            StartCoroutine(IntervalCoroutine()); //キャラクター生成の間隔を制御
        }

        else if (Input.GetMouseButton(0) && isGene && !isButtonHover)
        {
            //マウスの左ボタンが押されている間、キャラクターを移動させる(x座標のみ)
            float mousePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            geneChara.transform.position = new Vector2(mousePositionX, transform.position.y);
        }

    }

    public void RotateCharacter()
    {
        if (isGene)
        {
            geneChara.transform.Rotate(0, 0, -30);//30度ずつ回転
            audioSource.PlayOneShot(rotate_se); //回転音を再生
            isButtonReleasedOnButton = true; //ボタンから指を離した直後のフラグを立てる
        }

    }   //マウスカーソルがボタンの上にあるかの状態を更新する
    public void IsButtonChange(bool isX)
    {
        isButtonHover = isX; //ボタンの状態を変更
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


    void UpdateScore()
    {
        score++; //スコアを加算
        scoreText.text = score.ToString(); //スコア情報を更新
        Debug.Log("現在のスコア" + score + "です");
    }
}