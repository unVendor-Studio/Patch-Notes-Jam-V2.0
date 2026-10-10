using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgMusic;
    [SerializeField] private AudioSource soundEffectforUI;

    [Header("Dedicated Looping Sources (Player/Global)")]
    [SerializeField] private AudioSource sprintLoopSource;
    [SerializeField] private AudioSource slideLoopSource;
    [SerializeField] private AudioSource environmentLoopSource; // For ambient states like charging or momentum wind

    [Header("UI Volume Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private float globalMasterVolume = 1f;
    private float masterMusicVolume = 1f;
    private float masterSFXVolume = 1f;

    public bool AudioExisted = false;
    public GameObject ExistedAudio;

    [Header("BGM")]
    [SerializeField] public AudioClip music;
    [SerializeField, Range(0f, 2f)] public float musicVolume = 0.5f;

    [Header("Global UI & Hub Room")]
    [SerializeField] private AudioClip menuButton;
    [SerializeField, Range(0f, 2f)] private float menuButtonVolume = 1f;
    [SerializeField] private AudioClip paperRustle;
    [SerializeField, Range(0f, 2f)] private float paperRustleVolume = 1f;
    [SerializeField] private AudioClip levelEnterWarp;
    [SerializeField, Range(0f, 2f)] private float levelEnterWarpVolume = 1f;
    [SerializeField] private AudioClip aiNarratorBeep;
    [SerializeField, Range(0f, 2f)] private float aiNarratorBeepVolume = 1f;

    [Header("Core Mechanics (The Swap)")]
    [SerializeField] private AudioClip deviceClick;
    [SerializeField, Range(0f, 2f)] private float deviceClickVolume = 1f;
    [SerializeField] private AudioClip swapToPast;
    [SerializeField, Range(0f, 2f)] private float swapToPastVolume = 1f;
    [SerializeField] private AudioClip swapToFuture;
    [SerializeField, Range(0f, 2f)] private float swapToFutureVolume = 1f;

    [Header("Player Movement")]
    [SerializeField] private AudioClip futureFootsteps;
    [SerializeField, Range(0f, 2f)] private float futureFootstepsVolume = 1f;
    [SerializeField] private AudioClip pastFootsteps;
    [SerializeField, Range(0f, 2f)] private float pastFootstepsVolume = 1f;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField, Range(0f, 2f)] private float jumpSoundVolume = 1f;
    [SerializeField] private AudioClip lightLanding;
    [SerializeField, Range(0f, 2f)] private float lightLandingVolume = 1f;
    [SerializeField] private AudioClip heavyLanding;
    [SerializeField, Range(0f, 2f)] private float heavyLandingVolume = 1f;
    [SerializeField] private AudioClip sprintLoop;
    [SerializeField, Range(0f, 2f)] private float sprintLoopVolume = 1f;

    [Header("Environment & Interactables")]
    [SerializeField] private AudioClip blastDoor;
    [SerializeField, Range(0f, 2f)] private float blastDoorVolume = 1f;
    [SerializeField] private AudioClip vaultDoor;
    [SerializeField, Range(0f, 2f)] private float vaultDoorVolume = 1f;
    [SerializeField] private AudioClip cubeScrape;
    [SerializeField, Range(0f, 2f)] private float cubeScrapeVolume = 1f;
    [SerializeField] private AudioClip cubeThud;
    [SerializeField, Range(0f, 2f)] private float cubeThudVolume = 1f;

    [Header("Level 1: ClockWatch (Floating)")]
    [SerializeField] private AudioClip antiGravityHum; // Used as environmental loop
    [SerializeField, Range(0f, 2f)] private float antiGravityHumVolume = 1f;
    [SerializeField] private AudioClip laserBuzz;
    [SerializeField, Range(0f, 2f)] private float laserBuzzVolume = 1f;

    [Header("Level 2: Apple (Gravity)")]
    [SerializeField] private AudioClip gravityInversion;
    [SerializeField, Range(0f, 2f)] private float gravityInversionVolume = 1f;
    [SerializeField] private AudioClip vacuumRoar; // Can be looped locally on the object
    [SerializeField, Range(0f, 2f)] private float vacuumRoarVolume = 1f;
    [SerializeField] private AudioClip lavaBubbles;
    [SerializeField, Range(0f, 2f)] private float lavaBubblesVolume = 1f;

    [Header("Level 3: Kite (Electricity)")]
    [SerializeField] private AudioClip thunderstrike;
    [SerializeField, Range(0f, 2f)] private float thunderstrikeVolume = 1f;
    [SerializeField] private AudioClip electricChargeLoop; // Loop
    [SerializeField, Range(0f, 2f)] private float electricChargeLoopVolume = 1f;
    [SerializeField] private AudioClip nodePowerUp;
    [SerializeField, Range(0f, 2f)] private float nodePowerUpVolume = 1f;
    [SerializeField] private AudioClip nodeAccessGranted;
    [SerializeField, Range(0f, 2f)] private float nodeAccessGrantedVolume = 1f;
    [SerializeField] private AudioClip nodePowerDown;
    [SerializeField, Range(0f, 2f)] private float nodePowerDownVolume = 1f;

    [Header("Level 4: The Prism (Light)")]
    [SerializeField] private AudioClip lightTrapSizzle;
    [SerializeField, Range(0f, 2f)] private float lightTrapSizzleVolume = 1f;
    [SerializeField] private AudioClip monsterRush;
    [SerializeField, Range(0f, 2f)] private float monsterRushVolume = 1f;
    [SerializeField] private AudioClip monsterRetreat;
    [SerializeField, Range(0f, 2f)] private float monsterRetreatVolume = 1f;
    [SerializeField] private AudioClip eerieWindLoop; // Environmental Loop
    [SerializeField, Range(0f, 2f)] private float eerieWindLoopVolume = 1f;

    [Header("Level 5: Ice (Friction)")]
    [SerializeField] private AudioClip momentumWindLoop; // Loop for compounding speed
    [SerializeField, Range(0f, 2f)] private float momentumWindLoopVolume = 1f;
    [SerializeField] private AudioClip slideScrapeLoop; // Loop
    [SerializeField, Range(0f, 2f)] private float slideScrapeLoopVolume = 1f;
    [SerializeField] private AudioClip ricochetClang;
    [SerializeField, Range(0f, 2f)] private float ricochetClangVolume = 1f;
    [SerializeField] private AudioClip gripBrake;
    [SerializeField, Range(0f, 2f)] private float gripBrakeVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeLoopSources();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void InitializeLoopSources()
    {
        if (sprintLoopSource == null) sprintLoopSource = gameObject.AddComponent<AudioSource>();
        if (slideLoopSource == null) slideLoopSource = gameObject.AddComponent<AudioSource>();
        if (environmentLoopSource == null) environmentLoopSource = gameObject.AddComponent<AudioSource>();

        ConfigureLoopSource(sprintLoopSource, sprintLoop, sprintLoopVolume);
        ConfigureLoopSource(slideLoopSource, slideScrapeLoop, slideScrapeLoopVolume);
        // Environment loop is configured dynamically based on level needs
    }

    private void ConfigureLoopSource(AudioSource src, AudioClip clip, float baseVol)
    {
        if (clip != null) src.clip = clip;
        src.loop = true;
        src.playOnAwake = false;
        src.volume = baseVol * masterSFXVolume * globalMasterVolume;
    }

    private void Start()
    {
        globalMasterVolume = PlayerPrefs.GetFloat("GlobalMasterVolume", 1f);
        masterMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        masterSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (masterSlider != null)
        {
            masterSlider.value = globalMasterVolume;
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (musicSlider != null)
        {
            musicSlider.value = masterMusicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = masterSFXVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            PlayMusic(music, musicVolume);
        }
    }

    // --- Volume Setters ---
    public void SetMasterVolume(float volume)
    {
        globalMasterVolume = volume;
        if (bgMusic != null) bgMusic.volume = musicVolume * masterMusicVolume * globalMasterVolume;
        UpdateLoopVolumes();
        PlayerPrefs.SetFloat("GlobalMasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        masterMusicVolume = volume;
        if (bgMusic != null) bgMusic.volume = musicVolume * masterMusicVolume * globalMasterVolume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        masterSFXVolume = volume;
        UpdateLoopVolumes();
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void UpdateLoopVolumes()
    {
        float sfxVol = masterSFXVolume * globalMasterVolume;
        if (sprintLoopSource != null) sprintLoopSource.volume = sprintLoopVolume * sfxVol;
        if (slideLoopSource != null) slideLoopSource.volume = slideScrapeLoopVolume * sfxVol;
        if (environmentLoopSource != null) environmentLoopSource.volume = sfxVol; // Adjusts based on active clip
    }

    // --- Dynamic Loop Controls ---
    public void SetSprintSound(bool isPlaying) => ToggleLoopSource(sprintLoopSource, sprintLoop, sprintLoopVolume, isPlaying);
    public void SetSlideScrapeSound(bool isPlaying) => ToggleLoopSource(slideLoopSource, slideScrapeLoop, slideScrapeLoopVolume, isPlaying);

    public void SetEnvironmentLoop(AudioClip clip, float baseVol, bool isPlaying)
    {
        ToggleLoopSource(environmentLoopSource, clip, baseVol, isPlaying);
    }

    private void ToggleLoopSource(AudioSource src, AudioClip clip, float baseVol, bool play)
    {
        if (src == null || clip == null) return;

        if (src.clip != clip) src.clip = clip;
        src.volume = baseVol * masterSFXVolume * globalMasterVolume;

        if (play)
        {
            if (!src.isPlaying) src.Play();
        }
        else
        {
            if (src.isPlaying) src.Stop();
        }
    }

    // --- Base Audio Methods ---
    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (clip == null || bgMusic == null) return;
        bgMusic.clip = clip;
        bgMusic.volume = volume * masterMusicVolume * globalMasterVolume;
        bgMusic.loop = true;
        bgMusic.Play();
    }

    public void PlayUISound(AudioClip clip, float volume = 1f)
    {
        if (clip != null && soundEffectforUI != null)
        {
            soundEffectforUI.PlayOneShot(clip, volume * masterSFXVolume * globalMasterVolume);
        }
    }

    public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position, volume * masterSFXVolume * globalMasterVolume);
        }
    }

    // --- Global UI & Hub Callbacks ---
    public void PlayMenuButton() => PlayUISound(menuButton, menuButtonVolume);
    public void PlayPaperRustle() => PlayUISound(paperRustle, paperRustleVolume);
    public void PlayLevelEnterWarp() => PlayUISound(levelEnterWarp, levelEnterWarpVolume);
    public void PlayAINarratorBeep() => PlayUISound(aiNarratorBeep, aiNarratorBeepVolume);

    // --- Core Mechanics (Swap) Callbacks ---
    public void PlayDeviceClick(Vector3 pos) => PlaySFX(deviceClick, pos, deviceClickVolume);
    public void PlaySwapToPast(Vector3 pos) => PlaySFX(swapToPast, pos, swapToPastVolume);
    public void PlaySwapToFuture(Vector3 pos) => PlaySFX(swapToFuture, pos, swapToFutureVolume);

    // --- Player Movement Callbacks ---
    public void PlayFutureFootstep(Vector3 pos) => PlaySFX(futureFootsteps, pos, futureFootstepsVolume);
    public void PlayPastFootstep(Vector3 pos) => PlaySFX(pastFootsteps, pos, pastFootstepsVolume);
    public void PlayJumpSound(Vector3 pos) => PlaySFX(jumpSound, pos, jumpSoundVolume);
    public void PlayLightLanding(Vector3 pos) => PlaySFX(lightLanding, pos, lightLandingVolume);
    public void PlayHeavyLanding(Vector3 pos) => PlaySFX(heavyLanding, pos, heavyLandingVolume);

    // --- Environment & Interactables Callbacks ---
    public void PlayBlastDoor(Vector3 pos) => PlaySFX(blastDoor, pos, blastDoorVolume);
    public void PlayVaultDoor(Vector3 pos) => PlaySFX(vaultDoor, pos, vaultDoorVolume);
    public void PlayCubeScrape(Vector3 pos) => PlaySFX(cubeScrape, pos, cubeScrapeVolume);
    public void PlayCubeThud(Vector3 pos) => PlaySFX(cubeThud, pos, cubeThudVolume);

    // --- Level Specific Callbacks ---
    public void PlayLaserBuzz(Vector3 pos) => PlaySFX(laserBuzz, pos, laserBuzzVolume);
    public void PlayGravityInversion(Vector3 pos) => PlaySFX(gravityInversion, pos, gravityInversionVolume);
    public void PlayVacuumRoar(Vector3 pos) => PlaySFX(vacuumRoar, pos, vacuumRoarVolume);
    public void PlayLavaBubbles(Vector3 pos) => PlaySFX(lavaBubbles, pos, lavaBubblesVolume);

    public void PlayThunderstrike(Vector3 pos) => PlaySFX(thunderstrike, pos, thunderstrikeVolume);
    public void PlayNodePowerUp(Vector3 pos) => PlaySFX(nodePowerUp, pos, nodePowerUpVolume);
    public void PlayNodeAccessGranted(Vector3 pos) => PlaySFX(nodeAccessGranted, pos, nodeAccessGrantedVolume);
    public void PlayNodePowerDown(Vector3 pos) => PlaySFX(nodePowerDown, pos, nodePowerDownVolume);

    public void PlayLightTrapSizzle(Vector3 pos) => PlaySFX(lightTrapSizzle, pos, lightTrapSizzleVolume);
    public void PlayMonsterRush(Vector3 pos) => PlaySFX(monsterRush, pos, monsterRushVolume);
    public void PlayMonsterRetreat(Vector3 pos) => PlaySFX(monsterRetreat, pos, monsterRetreatVolume);

    public void PlayRicochetClang(Vector3 pos) => PlaySFX(ricochetClang, pos, ricochetClangVolume);
    public void PlayGripBrake(Vector3 pos) => PlaySFX(gripBrake, pos, gripBrakeVolume);
}