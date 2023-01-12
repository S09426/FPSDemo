using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMouseLook : MonoBehaviour
{
    //玩家身上的transform组件
    [SerializeField] private Transform playerTransform;
    //摄像机的transform组件
    private Transform cameraTransform;
    //鼠标输入值
    private Vector3 cameraRotation;
    //旋转速度(灵敏度)
    public float rotateSpeed;
    //摄像机旋转x轴的限制的值
    public Vector2 mouseXRotationMinAndMaxNum;
    private void Start()
    {
        //获取自身的transform组件
        cameraTransform = transform;
        //隐藏鼠标
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        //获取鼠标X轴和Y轴的移动
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        //摄像机x和y轴的旋转角度
        cameraRotation.y += mouseX * rotateSpeed;
        cameraRotation.x -= mouseY * rotateSpeed;
        //限制cameraX轴的旋转角度
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, mouseXRotationMinAndMaxNum.x, mouseXRotationMinAndMaxNum.y);
        //摄像机的旋转
        cameraTransform.rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0);
        //玩家y轴也跟随旋转
        playerTransform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
    }
}
