﻿using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BustingScene : MonoBehaviour
{
    private static readonly string[] DialogueTexts = {
        "Your moves are outdated.",
        "This style has expired last month.",
        "Your moves are too cool for this disco.",
        "Are you alone? May I buy you a drink?",
        "Awesome moves, pal! Let me buy you a drink.",
        "May I see your license for those awesome moves?"
    };
    private static readonly System.Random Random = new System.Random();

    [SerializeField] private DJ dj;
    [SerializeField] private AudioSource bustedMusicBox;
    [SerializeField] private AudioSource failureMusicBox;
    [SerializeField] private AudioSource victoryMusicBox;
    private DialogueCanvas dialogueCanvas;
    private Animator animator;

    private void Awake()
    {
        dialogueCanvas = FindObjectOfType<DialogueCanvas>();
        animator = GetComponent<Animator>();
    }

    public void StartBusting()
    {
        StartCommonBusting();
        bustedMusicBox.Play();
        animator.SetTrigger("start");
    }

    [UsedImplicitly]
    public void StartDialogue()
    {
        dialogueCanvas.Portrait = Portrait.Police;
        dialogueCanvas.Text = DialogueTexts[Random.Next(6)];
        dialogueCanvas.Appear();
        StartCoroutine(Dialogue());
    }

    private IEnumerator Dialogue()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        dialogueCanvas.ShowFailure();
        failureMusicBox.Play();
        yield return new WaitForSeconds(2f);
        Fade.instance.ResetLevel();
    }

    public void StartBustingPrank()
    {
        StartCommonBusting();
        dialogueCanvas.StopTimeRecording();
        FindObjectOfType<CameraMovment>().enabled = true;
        StartCoroutine(BustingPrank());
    }

    private void StartCommonBusting()
    {
        FindObjectOfType<CameraMovment>().resetZoom = true;
        dj.VolumeMultiplier = 0.25f;
        foreach (var otherMovement in FindObjectsOfType<Movement>())
            otherMovement.enabled = false;
        foreach (var dancer in FindObjectsOfType<Dancer>())
        {
            var animators = dancer.GetComponentsInChildren<Animator>();
            foreach (var dancerAnimator in animators)
                dancerAnimator.enabled = false;
        }
    }

    private IEnumerator BustingPrank()
    {
        dialogueCanvas.Portrait = Portrait.Boss;
        dialogueCanvas.Text = "Oh my, I like you, boy!";
        dialogueCanvas.Appear();
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("prank");
    }

    [UsedImplicitly]
    private void StartVictoryJingle()
    {
        dj.VolumeMultiplier = 0;
        victoryMusicBox.Play();
    }

    [UsedImplicitly]
    private IEnumerator StartVicotoryDialogue()
    {
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(dialogueCanvas.StartVictorySequence());
    }
}