using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Sound Configuration")]
public class SoundConfig : ScriptableObject
{
    [SerializeField] private AudioClip _uiButtonClick;
    [SerializeField] private AudioClip _comboExplosion;
    [SerializeField] private AudioClip _gameOverSound;
    [SerializeField] private AudioClip _ostSound;
    [SerializeField] private AudioClip _minusScore;
    [SerializeField] private AudioClip _pendulumClick;
    
    public AudioClip UiButtonClick => _uiButtonClick;
    public AudioClip ComboExplosion => _comboExplosion;
    public AudioClip GameOverSound => _gameOverSound;
    public AudioClip MinusScore => _minusScore;
    public AudioClip OSTSound => _ostSound;
    public AudioClip PendulumClick => _pendulumClick;
}


