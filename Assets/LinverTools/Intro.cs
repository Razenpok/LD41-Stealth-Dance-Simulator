﻿using System.Collections;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField] private Portrait portrait;
    [SerializeField] private string text;
    private DialogueCanvas dialogueCanvas;

    private void Awake()
    {
        dialogueCanvas = FindObjectOfType<DialogueCanvas>();
    }

    private IEnumerator Start()
    {
        SetPlayerAnimations(false);
        yield return new WaitForSeconds(2f);
        dialogueCanvas.Portrait = portrait;
        dialogueCanvas.Text = text;
        dialogueCanvas.Appear();
        yield return new WaitForSeconds(0.1f); // hack to wait appear animation
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        dialogueCanvas.Disappear();
        SetPlayerAnimations(true);
    }

    private void SetPlayerAnimations(bool active)
    {
        var player = FindObjectOfType<Player>();
        player.enabled = active;
        var animators = player.GetComponentsInChildren<Animator>();
        foreach (var animator in animators)
            animator.enabled = active;
        player.GetComponent<PlayerMovement>().enabled = active;
    }
}