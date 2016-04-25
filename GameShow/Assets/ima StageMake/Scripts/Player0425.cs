using UnityEngine;
using System.Collections;

public class Player0425 : MonoBehaviour {

    public float speed = 4f; //歩くスピード
    public float cameraThresholdX = 4f;
    public float cameraOffsetX = 1f;
    public float cameraThresholdY = 2f;
    public float cameraOffsetY = 1f;
    public float jumpPower = 700; //ジャンプ力
    public LayerMask groundLayer; //Linecastで判定するLayer
    public GameObject mainCamera;

    private Rigidbody2D rigidbody2D;
    private Animator anim;
    private bool isGrounded; //着地判定

    void Start()
    {
        //各コンポーネントをキャッシュしておく
        anim = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Linecastでユニティちゃんの足元に地面があるか判定
        isGrounded = Physics2D.Linecast(
        transform.position + transform.up * 1,
        transform.position - transform.up * 0.05f,
        groundLayer);
        //スペースキーを押し、
        if (Input.GetKeyDown("space"))
        {
            //着地していた時、
            if (isGrounded)
            {
                //Dashアニメーションを止めて、Jumpアニメーションを実行
                anim.SetBool("Dash", false);
                anim.SetTrigger("Jump");
                //着地判定をfalse
                isGrounded = false;
                //AddForceにて上方向へ力を加える
                rigidbody2D.AddForce(Vector2.up * jumpPower);
            }
        }
        //上下への移動速度を取得
        float velY = rigidbody2D.velocity.y;
        //移動速度が0.1より大きければ上昇
        bool isJumping = velY > 0.1f ? true : false;
        //移動速度が-0.1より小さければ下降
        bool isFalling = velY < -0.1f ? true : false;
        //結果をアニメータービューの変数へ反映する
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isFalling", isFalling);
    }

    void FixedUpdate()
    {
        //左キー: -1、右キー: 1
        float x = Input.GetAxisRaw("Horizontal");
        //左か右を入力したら
        if (x != 0)
        {
            //入力方向へ移動
            rigidbody2D.velocity = new Vector2(x * speed, rigidbody2D.velocity.y);
            //localScale.xを-1にすると画像が反転する
            Vector2 temp = transform.localScale;
            temp.x = x;
            transform.localScale = temp;
            //Wait→Dash
            anim.SetBool("Walk", true);

            /*
            //カメラ表示領域の左下をワールド座標に変換
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            //カメラ表示領域の右上をワールド座標に変換
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
            //ユニティちゃんのポジションを取得
            Vector2 pos = transform.position;
            //ユニティちゃんのx座標の移動範囲をClampメソッドで制限
            pos.x = Mathf.Clamp(pos.x, min.x + 0.5f, max.x);
            transform.position = pos;
            */
        }
        //左も右も入力していなかったら
        else
        {
            //横移動の速度を0にしてピタッと止まるようにする
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            //Dash→Wait
            anim.SetBool("Walk", false);
        }


        //画面中央から左にcameraThreshold移動した位置をユニティちゃんが超えたら
        if (transform.position.x > mainCamera.transform.position.x + cameraThresholdX - cameraOffsetX)
        {
            //カメラの位置を取得
            Vector3 cameraPos = mainCamera.transform.position;
            //ユニティちゃんの位置から右にcameraThreshold移動した位置を画面中央にする
            cameraPos.x = transform.position.x - cameraThresholdX + cameraOffsetX;
            mainCamera.transform.position = cameraPos;
        }
        //画面中央から右にcameraThreshold移動した位置をユニティちゃんが超えたら
        if (transform.position.x < mainCamera.transform.position.x - cameraThresholdX - cameraOffsetX)
        {
            //カメラの位置を取得
            Vector3 cameraPos = mainCamera.transform.position;
            //ユニティちゃんの位置から左にcameraThreshold移動した位置を画面中央にする
            cameraPos.x = transform.position.x + cameraThresholdX + cameraOffsetX;
            mainCamera.transform.position = cameraPos;
        }
        //画面中央から上にcameraThreshold移動した位置をユニティちゃんが超えたら
        if (transform.position.y > mainCamera.transform.position.y + cameraThresholdY - cameraOffsetY)
        {
            //カメラの位置を取得
            Vector3 cameraPos = mainCamera.transform.position;
            //ユニティちゃんの位置から下にcameraThreshold移動した位置を画面中央にする
            cameraPos.y = transform.position.y - cameraThresholdY + cameraOffsetY;
            mainCamera.transform.position = cameraPos;
        }
        //画面中央から下にcameraThreshold移動した位置をユニティちゃんが超えたら
        if (transform.position.y < mainCamera.transform.position.y - cameraThresholdY - cameraOffsetY)
        {
            //カメラの位置を取得
            Vector3 cameraPos = mainCamera.transform.position;
            //ユニティちゃんの位置から上にcameraThreshold移動した位置を画面中央にする
            cameraPos.y = transform.position.y + cameraThresholdY + cameraOffsetY;
            mainCamera.transform.position = cameraPos;
        }
    }
}
