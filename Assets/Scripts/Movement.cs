using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    Rigidbody rb;
    [SerializeField] float Thrust_Power;
    [SerializeField] float RotateSpeed;
    [Header("Audio")]
    AudioSource audioSource;
    [SerializeField] AudioClip SFXthrust;

    [Header("Particles")]
    [SerializeField] ParticleSystem PFX_LeftBooster;
    [SerializeField] ParticleSystem PFX_RightBooster;
    [SerializeField] ParticleSystem PFX_MainBooster;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.Log("Component has no Rigidbody");
        }
        audioSource = GetComponent<AudioSource>();
       if (audioSource == null)
        {
            Debug.Log("Component has no AudioSource");
        }
      
    }
    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    void FixedUpdate()
    {
        Thrust();
        Rotation();
    }

    private void Thrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }

    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * Thrust_Power * Time.fixedDeltaTime);
        if (!PFX_MainBooster.isPlaying)
        {
            PFX_MainBooster.Play();
        }
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(SFXthrust);
        }
    }
    private void StopThrusting()
    {
        PFX_MainBooster.Stop();
        audioSource.Stop();
    }


    private void Rotation()
    {
        rb.freezeRotation = true;
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput > 0)
        {
            RotateRight();
        }
        else if (rotationInput < 0)
        {
            RotateLeft();

        }
        else
        {
            StopRotating();
        }
        rb.freezeRotation = false;
        
    }

    private void RotateRight()
    {
        transform.Rotate(Vector3.back * RotateSpeed * Time.fixedDeltaTime);
        if (!PFX_LeftBooster.isPlaying)
        {
            PFX_RightBooster.Stop();
            PFX_LeftBooster.Play();
        }
    }
    private void RotateLeft()
    {
        transform.Rotate(Vector3.forward * RotateSpeed * Time.fixedDeltaTime);
        if (!PFX_RightBooster.isPlaying)
        {
            PFX_LeftBooster.Stop();
            PFX_RightBooster.Play();
        }
    }
    private void StopRotating()
    {
        PFX_LeftBooster.Stop();
        PFX_RightBooster.Stop();
    }

}
