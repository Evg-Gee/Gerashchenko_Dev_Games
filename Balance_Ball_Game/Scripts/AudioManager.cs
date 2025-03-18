using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class AudioManager : MonoBehaviour, IAudioService
{
    [Header("Dependencies")]
    [SerializeField] private SoundConfig _soundConfig;
    
    [Header("Settings")]
    [SerializeField] [Range(0f, 1f)] private float _masterVolume = 1f;
    
    private AudioSource _uiAudioSource;
    private AudioSource _gameplayAudioSource;
    private AudioSource _comboExplosion;
    private AudioSource _gameOverSound;
    private AudioSource _minusScore;
    private AudioSource _pendulumClick;
    private readonly Dictionary<SoundType, AudioSource> _audioSources = new();

    private enum SoundType
    {
        UI,
        Gameplay,
        ComboExplosion,
        GameOverSound,
        MinusScore,
        Pendulum_Click
    }

    private void Awake()
    {
        InitializeAudioSources();
        RegisterAudioSources();
    }

    private void InitializeAudioSources()
    {
        _uiAudioSource = gameObject.AddComponent<AudioSource>();
        _gameplayAudioSource = gameObject.AddComponent<AudioSource>();
        _comboExplosion = gameObject.AddComponent<AudioSource>();
        _gameOverSound = gameObject.AddComponent<AudioSource>();
        _minusScore = gameObject.AddComponent<AudioSource>();
        _pendulumClick = gameObject.AddComponent<AudioSource>();
        
        ConfigureAudioSource(_uiAudioSource);
        ConfigureAudioSource(_comboExplosion);
        ConfigureAudioSource(_gameOverSound);
        ConfigureAudioSource(_minusScore);
        ConfigureAudioSource(_pendulumClick);
    }

    private void RegisterAudioSources()
    {
        _audioSources.Add(SoundType.UI, _uiAudioSource);
        _audioSources.Add(SoundType.Gameplay, _gameplayAudioSource);
        _audioSources.Add(SoundType.ComboExplosion, _comboExplosion);
        _audioSources.Add(SoundType.GameOverSound, _gameOverSound);
        _audioSources.Add(SoundType.MinusScore, _minusScore);
        _audioSources.Add(SoundType.Pendulum_Click, _pendulumClick);
    }

    private void ConfigureAudioSource(AudioSource source)
    {
        source.playOnAwake = false;
        source.volume = _masterVolume;
    }

    public void PlayUiButtonClick()
    {
        PlaySound(_soundConfig.UiButtonClick, SoundType.UI);
    }
    public void PlayPendulumClick()
    {
        PlaySound(_soundConfig.PendulumClick, SoundType.Pendulum_Click);
    }

    public void PlayComboExplosion()
    {
        PlaySound(_soundConfig.ComboExplosion, SoundType.ComboExplosion);
    }

    public void PlayGameOver()
    {
        PlaySound(_soundConfig.GameOverSound, SoundType.GameOverSound);
    }
    public void PlayMinusScore()
    {
        PlaySound(_soundConfig.MinusScore, SoundType.MinusScore);
    }
    public void PlayOSTSound()
    {
        PlaySound(_soundConfig.OSTSound, SoundType.Gameplay);
    }
    
    public void SetMasterVolume(float volume)
    {
        _masterVolume = Mathf.Clamp01(volume);
        UpdateAllSourcesVolume();
    }

    private void UpdateAllSourcesVolume()
    {
        foreach (var source in _audioSources.Values)
        {
            source.volume = _masterVolume;
        }
    }

    private void PlaySound(AudioClip clip, SoundType type)
    {
        if (clip == null || !_audioSources.ContainsKey(type)) return;
        
        var source = _audioSources[type];
        source.PlayOneShot(clip);
    }
}