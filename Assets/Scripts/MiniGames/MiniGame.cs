using UnityEngine;

public abstract class MiniGame : InteractableObject
{
    protected bool isActive;

    public static System.Action OnEscPressed;

    public void Open()
    {
        if (isActive) return;

        isActive = true;
        gameObject.SetActive(true);

        Player.OnCinematic?.Invoke(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        OnOpen();
    }

    public void Close()
    {
        if (!isActive) return;

        isActive = false;

        OnClose();

        Player.OnCinematic?.Invoke(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameObject.SetActive(false);
    }

    protected override void OnInteract()
    {
        Open();
    }

    protected virtual void OnOpen() { }

    protected virtual void OnClose() { }

    protected virtual void OnEnable()
    {
        OnEscPressed += HandleEsc;
    }

    protected virtual void OnDisable()
    {
        OnEscPressed -= HandleEsc;
    }

    void HandleEsc()
    {
        if (isActive)
            Close();
    }

    protected virtual void Update()
    {
    }
}