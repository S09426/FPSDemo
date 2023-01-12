using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStepListener : MonoBehaviour
{
    //玩家移动音效列表
    public FootStepAudioData footStepAudioData;
    //声音播放组件
    public AudioSource footStepAudioSource;
    //玩家身上的CharacterController组件
    private CharacterController playerCharacterController;
    //玩家身上的transform组件
    private Transform playerTransform;
    //下一次的声音播放时间
    private float NextPlayTime;
    //空声音
    public AudioClip none;
    //玩家的当前状态
    public enum State
    {
        Idle,
        Walk,
        Springting,
        Others
    }
    public State playerState;
    private void Awake() {
        //获取玩家的CharacterController组件
        playerCharacterController = GetComponent<CharacterController>();
        //获取玩家身上的transform组件
        playerTransform = transform;
    }
    private void FixedUpdate() {
       
        //如果玩家在地面上
        if(playerCharacterController.isGrounded)
        {
            Vector3 newvelcity = playerCharacterController.velocity;
            newvelcity.y = 0;
            //玩家移动
            if(newvelcity.magnitude >= 0.1f)
            {
                //计时器（下一次播放音频的时间）增加
                NextPlayTime += Time.deltaTime;
                //切换状态
                if(newvelcity.magnitude >= 4)
                {
                    playerState = State.Springting;
                }
                if(newvelcity.magnitude < 4 && newvelcity.magnitude > 0.1f)
                {
                    playerState = State.Walk;
                }
                if(newvelcity.magnitude <= 0.1f)
                {
                    playerState = State.Idle;
                }

                //向下发射一条射线，并判断是否碰到了物体
                bool isHit = Physics.Linecast(playerTransform.position, playerTransform.position +
                     Vector3.down * (playerCharacterController.height / 2 + playerCharacterController.skinWidth - playerCharacterController.center.y),
                      out RaycastHit hitinfo);
                //如果射线射到了物体
                if(isHit)
                {
                    //遍历所有声音，判断声音的tag是否与碰到的声音匹配
                    foreach(var audioElement in footStepAudioData.footStepAudios)
                    {
                        if(hitinfo.collider.CompareTag(audioElement.tag))
                        {
                            //声音播放间隔时间
                            float audiodelay = 0;
                            switch(playerState)
                            {
                                case State.Idle:
                                    audiodelay = float.MaxValue;
                                    break;
                                case State.Others:
                                    break;
                                case State.Walk:
                                    audiodelay = audioElement.Delay;
                                    break;
                                case State.Springting:
                                    audiodelay = audioElement.SpringtingDelay;
                                    break;
                            }
                            //如果下一次播放时间大于播放等待时间，则播放
                            if(NextPlayTime >= audiodelay)
                            {
                                //定义的音频数量
                                int audioCount = audioElement.AudioClips.Count;
                                //随机播放一个音频（audioInex，目标音频的下标）
                                int audioIndex = Random.Range(0, audioCount);
                                //通过随机下标获取目标音频
                                AudioClip targetaudioClip = audioElement.AudioClips[audioIndex];
                                //播放目标音频
                                footStepAudioSource.clip = targetaudioClip;
                                footStepAudioSource.Play();
                                //计时器归0
                                NextPlayTime = 0;
                                //跳出循环
                                break;
                            }
                        }
                    }
                }
            }else
            {
                //若玩家停止移动，则将播放声音设置为空
                footStepAudioSource.clip = none;
            }
        }
    }
}
