using UnityEngine;

public class PlayerInteractor : Player, IPlayerModule
{
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [SerializeField] InteractableObject currentTarget;

    public void Init()
    {

    }


    private void Update()
    {
        FindClosestInteractable();

        if (currentTarget != null)
        {
            currentTarget.Interact(inputPlayerController.input_interact);
        }
    }

    private void FindClosestInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position, interactRange, interactableLayer
        );

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
