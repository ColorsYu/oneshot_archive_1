using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    //重力等の変更ができるようにパブリック変数とする
    public float gravity;
    public float speed;
    public float jumpSpeed;
    public float rotateSpeed;

    //外部から値が変わらないようにPrivateで定義
    private CharacterController characterController;
    private Animator animator;
    private Vector3 moveDirection = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        InvokeRepeating("YourFunction", 0.0f, 0.5f); // 0.5秒ごとにYourFunctionを実行
    }

    public float seconds;

    float timer = 0.0f;
    float interval = 0.4f; // 0.5秒ごとに実行
    bool isExecuting = false;



    IEnumerator ExecuteForDuration(float duration)
    {
        isExecuting = true;

        // ここで実行したい処理を記述
        animator.SetFloat("Pose", 0.0f);
        Debug.Log("0.5秒ごとに0.2秒間実行");

        yield return new WaitForSeconds(duration);

        // 処理の後、Poseを元に戻すなどの追加の処理を行う場合はここで記述
        animator.SetFloat("Pose", 1.0f);

        isExecuting = false;
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetFloat("Pose", 1.0f);
        /*
        seconds += Time.deltaTime;
        if (seconds % 10 >= 1)
        {
            animator.SetFloat("Pose", 0.0f);
            Debug.Log("2秒毎1秒間実行");
            Debug.Log(seconds);
        }
        else
        {
            animator.SetFloat("Pose", 1.0f);
        }
         // 一時停止
        */

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            if (!isExecuting)
            {
                StartCoroutine(ExecuteForDuration(0.1f)); // 0.2秒間だけ実行
            }

            timer = 0.0f;
        }

        //rayを使用した接地判定
        if (CheckGrounded() == true)
        {

            //前進処理
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveDirection.z = speed;
            }
            else
            {
                moveDirection.z = 0;
            }

            //方向転換
            //方向キーのどちらも押されている時
            if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                //向きを変えない
            }
            //左方向キーが押されている時
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0, rotateSpeed * -1, 0);
            }
            //右方向キーが押されている時
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0, rotateSpeed, 0);
            }

            //jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = jumpSpeed;
            }

            //重力を発生させる
            moveDirection.y -= gravity * Time.deltaTime;

            //移動の実行
            Vector3 globalDirection = transform.TransformDirection(moveDirection);
            characterController.Move(globalDirection * Time.deltaTime);

            //速度が０以上の時、Runを実行する
            animator.SetBool("Run", moveDirection.z > 0.0f);
        }
    }

    //rayを使用した接地判定メソッド
    public bool CheckGrounded()
    {

        //初期位置と向き
        var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        //rayの探索範囲
        var tolerance = 0.3f;

        //rayのHit判定
        //第一引数：飛ばすRay
        //第二引数：Rayの最大距離
        return Physics.Raycast(ray, tolerance);
    }

}