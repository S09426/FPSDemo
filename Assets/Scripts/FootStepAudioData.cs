using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "FPS/FootStep Audio Data")]
public class FootStepAudioData : ScriptableObject
{
    //创建footstepaudio列表
    public List<FootStepAudio> footStepAudios = new List<FootStepAudio>();

}

[System.Serializable]
public class FootStepAudio
{
    //随时更改声音
    public string tag;
    //创建音效列表
    public List<AudioClip> AudioClips = new List<AudioClip>();
    //声音播放间隔
    public float Delay;
    //跑步时声音播放间隔
    public float SpringtingDelay;
}