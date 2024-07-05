using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private float _speed = 10.0f;
    float idle_run_ratio = 0;
    bool bRunning = false;
    void Start()
    {
        GameManager.Input.KeyAction -= OnKeyInput;
        GameManager.Input.KeyAction += OnKeyInput;
    }

    void OnKeyInput()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            bRunning = true;

        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            bRunning = false;
        }

        if (Input.GetKey(KeyCode.S))
        {

        }

        if (Input.GetKey(KeyCode.A))
        {

        }

        if (Input.GetKey(KeyCode.D))
        {

        }
    }

    void Update()
    {
        if(bRunning)
        {
            transform.position += new Vector3(0.0f, 0.0f, 1.0f) * Time.deltaTime * _speed;
            idle_run_ratio = Mathf.Lerp(idle_run_ratio, 1, _speed * Time.deltaTime);
            Animator animator = GetComponent<Animator>();
            animator.SetFloat("idle_run_ratio", idle_run_ratio);
            animator.Play("OnGround");
        }

        else
        {
            idle_run_ratio = Mathf.Lerp(idle_run_ratio, 0, _speed * Time.deltaTime);
            Animator animator = GetComponent<Animator>();
            animator.SetFloat("idle_run_ratio", idle_run_ratio);
            animator.Play("OnGround");
        }
    }
}
