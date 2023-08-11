using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private bool _isGamePaused = false;

    [SerializeField] private ExcavatorController _excavatorController;
    [SerializeField] private Material[] skybox;
    private int skyboxIndex = 0;

    [SerializeField] private AudioSource playerSounds;

    [SerializeField] private AudioClip winSound;

    [SerializeField] private AudioSource themeMusic;

    public event EventHandler OnWinning;

    private void Awake()
    {
        Instance = this;
        _excavatorController = GameObject.Find("Player").GetComponent<ExcavatorController>();

    }
    private void Start()
    {
        themeMusic = GameObject.Find("Music").GetComponent<AudioSource>();
        themeMusic.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }
    }

    public void TogglePauseGame()
    {
        _isGamePaused = !_isGamePaused;
        if (_isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            _excavatorController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        } else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
            _excavatorController.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void skyboxChange()
    {
        skyboxIndex++;
        
        
        if (skyboxIndex >= skybox.Length)
        {
            skyboxIndex = 0;
        }
        RenderSettings.skybox = skybox[skyboxIndex];
    }

    public string getMaterialName()
    {
        return "THEME: " + skybox[skyboxIndex].name.ToUpper();
    }

    public void playSound(AudioClip soundToPlay)
    {
        playerSounds.PlayOneShot(soundToPlay);
    }

    public void LevelComplete()
    {
        themeMusic.Stop();
        playSound(winSound);
        OnWinning?.Invoke(this, EventArgs.Empty);

        float newScore = ExcavatorController.Instance.getScore();
        if (!PlayerPrefs.HasKey("hiScore"))
        {
            PlayerPrefs.SetFloat("hiScore", 0);
        }
        float highScore = PlayerPrefs.GetFloat("hiScore");

        if (PlayerPrefs.HasKey("hiScore"))
        {
            if (newScore > PlayerPrefs.GetFloat("hiScore"))
            {
                highScore = newScore;
                PlayerPrefs.SetFloat("hiScore", highScore);
                PlayerPrefs.Save();
            }
        }
        else
        {
            if (newScore > highScore)
            {
                highScore = newScore;
                PlayerPrefs.SetFloat("hiScore", highScore);
                PlayerPrefs.Save();
            }
        }
    }
}
