using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class SongSelector : MonoBehaviour
{
    public static SongSelector instance;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool isPlayable
    {
        get
        {
            return (clip != null) && (songData != null) ? true : false;
        }
    }
    public VideoClip clip;
    public SongData songData;

    public void LoadSongData(string clipName)
    {
        Debug.Log($"Trying to load {clipName}");
        // ���� Ŭ�� �ε�
        clip = Resources.Load<VideoClip>($"VideoClips/{clipName}");
        
        // �뷡 json ������ �ε�
        TextAsset songDataText = Resources.Load<TextAsset>($"SongDatas/{clipName}");

        // json ������ Deserialize
        songData = JsonUtility.FromJson<SongData>(songDataText.ToString());

    }
}
