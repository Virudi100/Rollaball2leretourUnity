using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Score System")]

    [SerializeField] private int maxScore = 6;
    [SerializeField] private UnityEngine.UI.Text scoreText;
    private int _scoreValue;
    [SerializeField] private GameObject gameOverGO;

    [Header("Speed & boosts Systems")]

    private bool isSlow = false;
    private bool canBoost = true;
    private float speed = 2;
    private float speedboost = 3;
    private float slowboost = 0.5f;
    
    private void Start()
    {
        Play();
        gameOverGO.SetActive(false);
    }

    private void Update()
    {
        IsInput();
        VerifyScore();
        
    }

    private void IsInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.Space) && canBoost == true)
        {
            canBoost = false;
            StartCoroutine(Sprint());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            Goal();
            SynchScore();
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("TargetMalus") && isSlow == false)
        {
            isSlow = true;
            StartCoroutine(Slow());
            Destroy(other.gameObject);
        }
    }


    IEnumerator Slow()
    {
        speed *= slowboost;
        yield return new WaitForSeconds(1);
        speed /= slowboost;
        isSlow = false;
    }

    IEnumerator Sprint()
    {
        speed *= speedboost;
        yield return new WaitForSeconds(1);
        speed /= speedboost;
        canBoost = true;
    }

    private void Goal()
    {
        _scoreValue++;
    }

    private void SynchScore()
    {
        scoreText.text = "Score : " + _scoreValue;
    }

    private void Pause()
    {
        Time.timeScale = 0;
    }

    private void Play()
    {
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Retry()
    {
        SceneManager.LoadScene("StartScene");
    }

    private void VerifyScore()
    {
        if (_scoreValue >= maxScore)
        {
            gameOverGO.SetActive(true);
            Pause();
        }
    }
}
