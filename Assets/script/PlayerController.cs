using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Slider�̂��߂̂��� 

public class PlayerController : MonoBehaviour
{
    Slider poseSlider; //Slider�^��錾

    private Rigidbody _rigidbody;
    private Transform _transform;
    private Animator _animator;
    private float _horizontal;
    private float _vertical;
    private Vector3 _velocity;
    private float _speed = 2f;

    private Vector3 _aim; // �ǋL
    private Quaternion _playerRotation; // �ǋL

    float timer = 0.0f;
    float interval = 0.2f; // �Z�b���ƂɎ��s
    bool isExecuting = false;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        poseSlider = GetComponent<Slider>(); //Slider�̃R���|�[�l���g���擾

        float maxPose = 1.0f;
        float nowPose = 0.5f;


        //�X���C�_�[�̍ő�l�̐ݒ�
        poseSlider.maxValue = maxPose;

        //�X���C�_�[�̌��ݒl�̐ݒ�
        poseSlider.value = nowPose;

        _playerRotation = _transform.rotation; // �ǋL
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            if (!isExecuting)
            {
                StartCoroutine(ExecuteForDuration(0.2f)); // �Z�b�Ԃ������s
            }

            timer = 0.0f;
        }
    }

    IEnumerator ExecuteForDuration(float duration)
    {
        isExecuting = true;

        // �����Ŏ��s�������������L�q
        _animator.SetFloat("Pose", 0.5f);
        //_speed = 0.5f;
        _speed = poseSlider.value;
        Debug.Log(poseSlider.value);
        Debug.Log("0.5�b���Ƃ�0.2�b�Ԏ��s");

        yield return new WaitForSeconds(duration);

        // �����̌�APose�����ɖ߂��Ȃǂ̒ǉ��̏������s���ꍇ�͂����ŋL�q
        _animator.SetFloat("Pose", 1.0f);
        _speed = 2f;

        isExecuting = false;
    }

    float a;

    void FixedUpdate()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        var _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up); // �ǋL

        _velocity = _horizontalRotation * new Vector3(_horizontal, _rigidbody.velocity.y, _vertical).normalized; // �C��

        _aim = _horizontalRotation * new Vector3(_horizontal, 0, _vertical).normalized; // �ǋL

        if (_aim.magnitude > 0.5f)
        { // �ȉ��ǋL
            _playerRotation = Quaternion.LookRotation(_aim, Vector3.up);
        }

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _playerRotation, 600 * Time.deltaTime); // �ǋL

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
