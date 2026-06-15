using UnityEngine;

public class PlayerInteractor : MonoBehaviour, IPlayerModule
{
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private InteractableObject currentTarget;

    private Player player;
    private InputPlayerController input;

    public void Init(Player player)
    {
        this.player = player;
        input = player.Input;
    }

    private void Update()
    {
        // Bloqueado durante cinematica o minijuego
        if (player.StunMovement || player.MinigameStunMovement)
        {
            if (currentTarget != null)
            {
                currentTarget.HideUI();
                currentTarget = null;
            }
            return;
        }

        FindClosestInteractable();

        if (currentTarget != null)
            currentTarget.Interact(input.Interact);
    }

    private void FindClosestInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, interactableLayer);

        InteractableObject closest = null;
        float closestDist = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out InteractableObject obj))
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = obj;
                }
            }
        }

        if (closest != currentTarget)
        {
            currentTarget?.HideUI();
            currentTarget = closest;
            currentTarget?.ShowUI();
        }
    }
}