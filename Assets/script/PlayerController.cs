using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Sliderのためのもの 

public class PlayerController : MonoBehaviour
{
    Slider poseSlider; //Slider型を宣言

    private Rigidbody _rigidbody;
    private Transform _transform;
    private Animator _animator;
    private float _horizontal;
    private float _vertical;
    private Vector3 _velocity;
    private float _speed = 2f;

    private Vector3 _aim; // 追記
    private Quaternion _playerRotation; // 追記

    float timer = 0.0f;
    float interval = 0.2f; // 〇秒ごとに実行
    bool isExecuting = false;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        poseSlider = GetComponent<Slider>(); //Sliderのコンポーネントを取得

        float maxPose = 1.0f;
        float nowPose = 0.5f;


        //スライダーの最大値の設定
        poseSlider.maxValue = maxPose;

        //スライダーの現在値の設定
        poseSlider.value = nowPose;

        _playerRotation = _transform.rotation; // 追記
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            if (!isExecuting)
            {
                StartCoroutine(ExecuteForDuration(0.2f)); // 〇秒間だけ実行
            }

            timer = 0.0f;
        }
    }

    IEnumerator ExecuteForDuration(float duration)
    {
        isExecuting = true;

        // ここで実行したい処理を記述
        _animator.SetFloat("Pose", 0.5f);
        //_speed = 0.5f;
        _speed = poseSlider.value;
        Debug.Log(poseSlider.value);
        Debug.Log("0.5秒ごとに0.2秒間実行");

        yield return new WaitForSeconds(duration);

        // 処理の後、Poseを元に戻すなどの追加の処理を行う場合はここで記述
        _animator.SetFloat("Pose", 1.0f);
        _speed = 2f;

        isExecuting = false;
    }

    float a;

    void FixedUpdate()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        var _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up); // 追記

        _velocity = _horizontalRotation * new Vector3(_horizontal, _rigidbody.velocity.y, _vertical).normalized; // 修正

        _aim = _horizontalRotation * new Vector3(_horizontal, 0, _vertical).normalized; // 追記

        if (_aim.magnitude > 0.5f)
        { // 以下追記
            _playerRotation = Quaternion.LookRotation(_aim, Vector3.up);
        }

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _playerRotation, 600 * Time.deltaTime); // 追記

        if (_velocity.magnitude > 0.1f)
        {
            _animator.SetBool("walking", true);
        }
        else
        {
            _animator.SetBool("walking", false);
        }

        _rigidbody.velocity = _velocity * _speed;
    }
}
