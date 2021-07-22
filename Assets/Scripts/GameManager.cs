using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private PlayerController playerControllerScript;
    public float score;
    public Transform startingPoint;
    public float lerpSpeed;
    private ParticleSystem dirtParticle;


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        dirtParticle = GameObject.Find("DirtSplatter").GetComponent<ParticleSystem>();
        playerControllerScript.gameOver = true;
        dirtParticle.Stop();
        StartCoroutine(PlayIntro());
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControllerScript.gameOver)
        {
            score += playerControllerScript.gameSpeed;
            Debug.Log("Score: " + score);
        }
    }

    IEnumerator PlayIntro()
    {
        Vector3 startPos = playerControllerScript.transform.position;
        Vector3 endPos = startingPoint.position;
        float journeyLength = Vector3.Distance(startPos, endPos);
        float startTime = Time.time;
        float distanceCovered = (Time.time - startTime) * lerpSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;
        while (fractionOfJourney < 1)
        {
            distanceCovered = (Time.time - startTime) * lerpSpeed;
            fractionOfJourney = distanceCovered / journeyLength;
            playerControllerScript.transform.position = Vector3.Lerp(startPos, endPos,
            fractionOfJourney);
            yield return null;
        }
        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_f",
        1.0f);
        playerControllerScript.gameOver = false;
        dirtParticle.Play();
    }

}
