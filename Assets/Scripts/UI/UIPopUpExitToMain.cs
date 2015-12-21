﻿using UnityEngine;
using UnityEngine.UI;

public class UIPopUpExitToMain : MonoBehaviour
{
    [SerializeField]
    private Button m_UIYesButton;

    [SerializeField]
    private Button m_UINoButton;

    private void OnEnable()
    {
        enabled &= DebugUtilities.Verify(m_UIYesButton != null, "Yes button not assigned");
        enabled &= DebugUtilities.Verify(m_UINoButton != null, "No button not assigned");
    }

    private void Start()
    {
        // Give functionality to buttons
        m_UIYesButton.onClick.AddListener(delegate { SceneManager.Instance.LoadMainMenu(); });
        m_UINoButton.onClick.AddListener(delegate { UIGame.Instance.ReturnToGame(); });
    }
}