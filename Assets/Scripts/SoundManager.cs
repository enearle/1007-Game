using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("SoundData")]
public class SoundData
{
    public float mast;
    public float bgm;
    public float sfx;
    public float pan;

    public SoundData()
    {
        mast = 1;
        bgm = 0.3f;
        sfx = 0.6f;
        pan = 0;
    }
}

public class SoundManager : MonoBehaviour
{
    public enum SoundType
    {
        SOUND_SFX,
        SOUND_MUSIC
    }

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip[] music;
    [SerializeField] private AudioClip[] oneShot;
    private AudioSource sfxSource;
    private AudioSource musicSource;
    
    [SerializeField] private GameObject Menu;
    
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider panSlider;
    [SerializeField] private int sceneSongIndex = 1;
    [Range(0, 1)] private float volMaster = 1f, volSFX = 0.75f, volBGM = 0.25f;
    [Range(-1, 1)] private float pan = 0;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Attempted to instantiate SoundManager(Singleton Pattern)");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        DeserializeFromXml();
        
        Initialize();
    }

    private void Initialize()
    {
        Debug.Log("Initializing SoundManager.");
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.volume = volSFX;
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.volume = volBGM;
        musicSource.loop = true;
        
        PlayMusic(sceneSongIndex);
        Debug.Log("SoundManager initialized.");
        InitSliders();
    }

    public void PlaySoundFX(int soundIndex)
    {
        sfxSource.PlayOneShot(oneShot[soundIndex]);
    }
    
    public void PlayMusic(int soundIndex)
    {
        musicSource.Stop();
        musicSource.clip = music[soundIndex];
        musicSource.Play();
    }
    
    private void UpdateVolume()
    {
        sfxSource.volume = volMaster * volSFX;
        musicSource.volume = volMaster * volBGM;
    }
    
    public void SetMasterVolume()
    {
        volMaster = masterSlider.value;
        UpdateVolume();
    }

    public void SetBGMVolume()
    {
        volBGM = bgmSlider.value;
        UpdateVolume();
    }

    public void SetSFXVolume()
    {
        volSFX = sfxSlider.value;
        UpdateVolume();
    }
    
    public void SetPan()
    {
        pan = panSlider.value * 2 - 1;
        musicSource.panStereo = pan;
        sfxSource.panStereo = pan;
    }
    
    private void InitSliders()
    {
        masterSlider.value = volMaster;
        bgmSlider.value = volBGM;
        sfxSlider.value = volSFX;
        panSlider.value = (pan + 1) / 2;
    }
    public void SerializeToXml()
    {
        SoundData soundData = new SoundData();
        soundData.mast = volMaster;
        soundData.bgm = volBGM;
        soundData.sfx = volSFX;
        soundData.pan = pan;
        
        XmlSerializer serializer = new XmlSerializer(typeof(SoundData));
        using (var streamWriter = new FileStream("Assets/sounddata.xml", FileMode.Create))
        {
            serializer.Serialize(streamWriter, soundData);
        }
    }

    private void DeserializeFromXml()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SoundData));
        
        using (var streamReader = new FileStream("Assets/sounddata.xml", FileMode.Open))
        {
            SoundData soundData = serializer.Deserialize(streamReader) as SoundData;
            
            volMaster = soundData.mast;
            volBGM = soundData.bgm;
            volSFX = soundData.sfx;
            pan = soundData.pan;
        }
    }
}