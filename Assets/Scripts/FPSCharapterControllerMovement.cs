using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCharapterControllerMovement : MonoBehaviour
{
    //奔跑速度
    public float sprintingSpeed;
    //玩家身上的characterController
    private CharacterController characterController;
    //玩家身上的transform组件
    private Transform playerTransform;
    //玩家移动速度
    public float walkSpeed;
    //重力
    public float gravity = 9.8f;
    //移动坐标
    private Vector3 moveDirection;
    //跳跃高度
    public float JumpHeight;
    //下蹲后的玩家高度
    public float crouchHeight = 1f;
    //是否正在蹲下
    private bool isCrouching;
    //原本的高度
    private float originHeight;
    //下蹲时的奔跑速度
    public float sprintingSpeedWhenCroud;
    //下蹲时的行走速度
    public float walkSpeedWhenCroud;
    //玩家身上的Animator组件
    private Animator playerAnimator;
    //玩家身上的CharacterController的velocity
    private float velocity;
    private void Awake()
    {
        //从子节点中获取玩家的Animator组件
        playerAnimator = GetComponentInChildren<Animator>();
        //获取玩家的characterController
        characterController = GetComponent<CharacterController>();
        //获取玩家的transform组件
        playerTransform = transform;
        //获取原本的高度
        originHeight = characterController.height;
    }

    private void Update()
    {
        float speed = walkSpeed;
        //如果玩家落地
        if(characterController.isGrounded)
        {
            //获取水平轴输入
            float InputHorizontal = Input.GetAxis("Horizontal");
            //获取垂直轴输入
            float InputVertical = Input.GetAxis("Vertical");

            //世界坐标转化为自身坐标
            moveDirection = transform.TransformDirection(new Vector3(InputHorizontal, 0, InputVertical));
            //如果按下了跳跃键
            if(Input.GetButtonDown("Jump"))
            {
                //跳跃
                moveDirection.y = JumpHeight;
            }
            if(isCrouching)
            {
                //如果按下了奔跑键则将移动速度设为奔跑速度，否则将移动速度设为行走速度
                speed = Input.GetButton("Sprint") ? sprintingSpeedWhenCroud : walkSpeedWhenCroud;
            }else{
                //如果按下了奔跑键则将移动速度设为奔跑速度，否则将移动速度设为行走速度
                speed = Input.GetButton("Sprint") ? sprintingSpeed : walkSpeed;
            }
            //如果按下了下蹲键
            if(Input.GetButtonDown("Crouch"))
            {
                //如果按下了下蹲键，则目标高度为下蹲高度，否则为目标高度为原本高度
                float currentHeight = isCrouching? originHeight : crouchHeight;
                //开始下蹲协程
                StartCoroutine(DoCrouch(currentHeight));
                //将下蹲状态设置为对立状态
                isCrouching = !isCrouching;
            }
            //玩家身上CharacterController的velocity值
            Vector3 newVelocity = characterController.velocity;
            //将获取到的velocity值的y轴设为0
            newVelocity.y = 0;
            //将获取到的velocity值计算成一个数
            velocity = newVelocity.magnitude;
            //将获取到的velocity值赋给玩家身上的animator组件的velocity变量
            playerAnimator.SetFloat("Velocity", velocity, 0.25f, Time.deltaTime);
        }
        //玩家下落
        moveDirection.y -= gravity * Time.deltaTime;
        //玩家移动
        characterController.Move(moveDirection * Time.deltaTime * speed);        
    }
    IEnumerator DoCrouch(float target)
    {
        float currentHeight = 0;
        while(Mathf.Abs(characterController.height - target) > 0.1f)
        {
            yield return null;
            characterController.height = Mathf.SmoothDamp(characterController.height, target, ref currentHeight, Time.deltaTime * 5);
        }
    }
}
