using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Cursor manager for the simulation view.
/// - Clicking the scene (left mouse) locks the cursor if the pointer is NOT over UI.
/// - Pressing Escape (configurable) unlocks the cursor.
/// - Public UnlockCursor/LockCursor methods are provided for UI buttons (e.g. Back button should call UnlockCursor).
/// </summary>
public class CursorUnlocker : MonoBehaviour
{
    [Tooltip("If true, clicking anywhere that is NOT a UI element will lock the cursor (typical for immersive simulation).")]
    public bool lockOnSceneClick = true;

    [Tooltip("If true, pressing Escape will unlock the cursor.")]
    public bool unlockOnEscape = true;

    [Tooltip("Key used to unlock the cursor (default: Escape).")]
    public KeyCode unlockKey = KeyCode.Escape;

    void Start()
    {
        // Start unlocked so UI is clickable by default
        UnlockCursor();
    }

    void Update()
    {
        // Unlock via key (Escape by default)
        if (unlockOnEscape && Input.GetKeyDown(unlockKey))
        {
            UnlockCursor();
        }

        // Lock when clicking the scene, but do NOT lock if the click was on UI
        if (lockOnSceneClick && Input.GetMouseButtonDown(0))
        {
            // If EventSystem exists and pointer is over UI, don't lock
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            LockCursor();
        }
    }

    /// <summary>
    /// Unlock cursor & make it visible (callable from UI Button OnClick)
    /// </summary>
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Lock cursor to center & hide it (useful for immersive simulation)
    /// </summary>
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Convenience method for UI buttons that might want to both unlock and perform navigation.
    /// Can be wired to a Back button's OnClick: first call this, then scene change.
    /// </summary>
    public void OnBackButtonPressed()
    {
        UnlockCursor();
    }
}