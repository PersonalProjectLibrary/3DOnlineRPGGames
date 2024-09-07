
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Service_AudioService.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/30 22:08
    功能：声音播放服务
***************************************/
#endregion

using UnityEngine;

/// <summary>
/// 声音播放服务
/// </summary>
public class AudioService : MonoBehaviour
{
    public static AudioService Instance =null;

    public AudioSource bgAudio;
    public AudioSource uiAudio;

    /// <summary>
    /// 声音播放服务初始化
    /// </summary>
    public void InitService()
    {
        Instance = this;
        PECommon.Log("Init AudioService...");
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="musicName"></param>
    /// <param name="isLoop"></param>
    public void PlayBgMusic(string musicName, bool isLoop = true)
    {
        //背景音乐经常反复切换，所以对其进行缓存
        AudioClip audio = ResService.Instance.LoadAudio("ResAudio/" + musicName, true);

        //之前不存在背景音乐，或当前音乐不是目标音乐，需要切换背景音乐
        if (bgAudio.clip == null || bgAudio.clip.name != audio.name)
        {
            bgAudio.clip = audio;
            bgAudio.loop = isLoop;
            bgAudio.Play();
        }
    }

    public void PlayUIAudio(string audioName)
    {
        //UI音乐也经常触发，对其进行缓存
        AudioClip audio = ResService.Instance.LoadAudio("ResAudio/" + audioName, true);
        //直接触发UI音效
        uiAudio.clip = audio;
        uiAudio.Play();
    }
}