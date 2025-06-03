
using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollisionHandler : MonoBehaviour
{
    [Header("Sequence Delay")]
    [SerializeField] float waitTime = 2.0f;

    [Header("Audio")]
    [SerializeField] AudioClip SFX_Crash;
    [SerializeField] AudioClip SFX_NextLevel;

    [Header("Particles")]
    [SerializeField] ParticleSystem PFX_NextLevel;
    [SerializeField] ParticleSystem PFX_Crash;

    [Header("Debug")]
    [SerializeField]  static GameObject debugImage;

    AudioSource audioSource;
    bool isControllable = true;
    bool isCollidable = true;
    [SerializeField] static bool isDebugging = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("No Audio Source found on game component");
        }

        debugImage = GameObject.Find("DebugImage");
        debugImage.SetActive(isDebugging);
        if (debugImage == null)
        {
            Debug.Log("No Image attached");
        }
    }

    void Update()
    {
        if (Keyboard.current.backquoteKey.wasPressedThisFrame)
        {
            isDebugging = !isDebugging;
            debugImage.SetActive(isDebugging);
            
        }
       
        RespondToDebugKeys(isDebugging);
        
    }

    private void RespondToDebugKeys(bool isActive)
    {
        if (isActive)
        {
            if (Keyboard.current.lKey.wasPressedThisFrame == true)
            {
                LoadNextLevel();
            }
            else if (Keyboard.current.cKey.wasPressedThisFrame == true)
            {
                isCollidable = !isCollidable;
            }
        }
        else
        {
            isCollidable = true;

            return;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!isControllable || !isCollidable)
        {
            return;
        }
        
        switch (other.gameObject.tag)
            {
                case "Friendly":
                    {
                        Debug.Log("Friendly");
                        break;
                    }
                case "Finish":
                    {
                        StartNextLevel();
                        break;
                    }
                case "Collectable":
                    {
                        Debug.Log("Collectable");
                        break;
                    }

                default:
                    {
                        StartCrashSequence();

                        break;
                    }
            }
    }

    void StartNextLevel()
    {
        isControllable = false;
        audioSource.Stop();
        //TODO add sfx and animations
        audioSource.PlayOneShot(SFX_NextLevel);
        PFX_NextLevel.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", waitTime);
    }

    void StartCrashSequence()
    {
        Vector3 currentPos = transform.position;
        isControllable = false;
        audioSource.Stop();
        //TODO add sfx and animations
        audioSource.PlayOneShot(SFX_Crash);
        PFX_Crash.Play();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        GameObject.Find("Rocket Model").SetActive(false);
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", waitTime);
        
    }

    void LoadNextLevel()
    {
    
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log("scene count = " + SceneManager.sceneCountInBuildSettings);
        Debug.Log("current index = " + nextLevel);
        if (nextLevel == SceneManager.sceneCountInBuildSettings)
        {
            nextLevel = 0;
        }
        SceneManager.LoadScene(nextLevel);
    }
    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);

    }
    
}
