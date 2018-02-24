using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    new Rigidbody rigidbody;
    AudioSource audioSource;

    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 120f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip winSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem winParticles;

    enum State { Alive, Dying, Winning };
    State state = State.Alive;

    // Use this for initialization
    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                WinTransition();
                break;
            default:
                DeathTransition();
                break;
        }
    }

    private void DeathTransition()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("LoadNextScene", 1.5f);
    }

    private void WinTransition()
    {
        state = State.Winning;
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
        winParticles.Play();
        Invoke("LoadNextScene", 1.5f);
    }

    private void LoadNextScene()
    {
        Scene currentLevel = SceneManager.GetActiveScene();
        Scene levelTwo = SceneManager.GetSceneByName("Level 2");
        int currentLevelIndex = currentLevel.buildIndex;

        if (currentLevel.Equals(levelTwo))
        {
            SceneManager.LoadScene(1);
        }
        else if(state == State.Winning)
        {
            SceneManager.LoadScene(currentLevelIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(currentLevelIndex);
        }
    }

    private void RespondToThrustInput()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust(thrustThisFrame);
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust(float thrustThisFrame)
    {
        rigidbody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        rigidbody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }
        rigidbody.freezeRotation = false;
    }
}
