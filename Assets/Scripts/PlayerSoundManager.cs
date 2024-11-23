using UnityEngine;
using UnityEngine.InputSystem;

public class FootstepSoundManager : MonoBehaviour
{
    public AudioClip[] footstepClips; // Tableau des sons de pas
    public AudioSource audioSource; // Source audio pour jouer les sons
    public float walkingInterval = 0.5f; // Intervalle des sons en marchant
    public float runningInterval = 0.3f; // Intervalle des sons en courant

    private float stepTimer; // Timer pour gérer l'intervalle
    private int currentClipIndex = 0; // Index du son actuel
    public bool isRunning; // Indique si le joueur court

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (IsWalking()) // Vérifie si le joueur marche ou court
        {
            stepTimer += Time.deltaTime;

            // Choisit l'intervalle approprié (marche ou course)
            float currentInterval = isRunning ? runningInterval : walkingInterval;

            if (stepTimer >= currentInterval)
            {
                PlayNextFootstep();
                stepTimer = 0f; // Réinitialise le timer
            }
        }
        else
        {
            stepTimer = 0f; // Réinitialise si le joueur s'arrête
        }
    }

    bool IsWalking()
    {
        // Remplacez cette condition par votre logique pour détecter si le joueur marche
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    void PlayNextFootstep()
    {
        if (footstepClips.Length > 0)
        {
            // Joue le clip actuel
            audioSource.PlayOneShot(footstepClips[currentClipIndex]);

            // Passe au son suivant, et revient au début si on atteint la fin
            currentClipIndex = (currentClipIndex + 1) % footstepClips.Length;
        }
    }
}