using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMovement : MonoBehaviour
{
    //是否落地
    private bool isGround;
    //玩家身上的transform组件
    private Transform playerTransform;
    //玩家身上的rigibody组件
    private Rigidbody playerRb;
    //玩家移动速度
    public float playerMoveSpeed;
    //重力
    public float gravity;
    //跳跃高度
    public float jumpHeight;
    private void Awake()
    {
        //获取玩家身上的transform组件
        playerTransform = transform;
        //获取自身的rigibody组件
        playerRb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if(isGround)
        {
            //获取垂直轴和水平轴的输入
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            //将用户的输入存储为Vector3
            Vector3 inputDirection = new Vector3(horizontalInput, 0, verticalInput);
            //将世界坐标转化成玩家的自身坐标
            Vector3 localTransform = playerTransform.TransformDirection(inputDirection);
            //增加速度
            localTransform *= playerMoveSpeed;
            //玩家身上的rigibody组件目前的velocity
            Vector3 playerVelocity = playerRb.velocity;
            //获取速度向量
            Vector3 velocityChange = localTransform - playerVelocity;
            //先不计算y值
            velocityChange.y = 0;
            //向玩家身上的刚体组件施加计算出来的移动向量的力
            playerRb.AddForce(velocityChange, ForceMode.VelocityChange);
            //如果按下了跳跃键
            if(Input.GetButtonDown("Jump"))
            {
                playerRb.velocity = new Vector3(playerRb.velocity.x, JumpSpeed(), playerRb.velocity.z);
            }
        }
        //玩家下落
        playerRb.AddForce(new Vector3(0, -gravity * playerRb.mass, 0));
    }
    private void OnCollisionStay(Collision collision)
    {
        isGround = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        isGround = false;
    }
    private float JumpSpeed()
    {
        return Mathf.Sqrt(2 * gravity * jumpHeight);
    }
}
