using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float height = 1;
    //玩家的transform组件
    [SerializeField] private Transform playerTransform;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        //计算摄像机位置
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + height, playerTransform.position.z);
    }
}
