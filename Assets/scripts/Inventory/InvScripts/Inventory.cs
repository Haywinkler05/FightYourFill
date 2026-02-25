using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public ItemSO SwordItem;

    public GameObject hotbarObj;
    public GameObject inventorySlotParent;
    public GameObject container;

    public Image dragIcon;

    public float pickupRange = 3f;
    private Item lookedAtItem = null;
    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer lookedAtRenderer = null;

    private int equippedHotbarIndex = 0;//0-5
    public float equippedOpacity = 0.9f;
    public float normalOpacity = 0.58f;

    // IK: Reference to the IKControl script on the character
    public IKControl ikControl;

    // The live scene instance of the currently equipped item's handItemPrefab.
    // Parented to the player so it moves with them, and provides the IKGrabHandle.
    private GameObject currentHandItemInstance;

    // Cached reference to the IKGrabHandle transform on the current hand instance.
    // Set when the instance is spawned so GetGrabHandle() doesn't call Find() every frame.
    private Transform currentGrabHandle;

    private List<Slot> inventorySlots = new List<Slot>();
    private List<Slot> hotbarSlots = new List<Slot>();
    private List<Slot> allSlots = new List<Slot>();

    private Slot draggedSlot = null;
    private bool isDragging = false;

    private void Awake()
    {
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<Slot>());
        hotbarSlots.AddRange(hotbarObj.GetComponentsInChildren<Slot>());

        allSlots.AddRange(inventorySlots);
        allSlots.AddRange(hotbarSlots);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))//Tab to turn inventory menu on/off
        {
            container.SetActive(!container.activeInHierarchy);
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }

        //Pickup Functions
        DetectLookedAtItem();
        Pickup();

        //Drag Functions
        StartDrag();
        UpdateDragItemPosition();
        EndDrag();

        //Equip Functions
        HandleHotBarSelection();
        HandleDropEquippedItem();
        UpdateHotBarOpacity();
    }


    public Transform GetGrabHandle()
    {
        return currentGrabHandle;
    }

    public GameObject GetHandItemInstance()
    {
        return currentHandItemInstance;
    }

    public void AddItem(ItemSO itemToAdd, int amount)
    {
        int remaining = amount;

        foreach (Slot slot in allSlots)
        {
            if (slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetAmount();
                int maxStack = itemToAdd.maxStackSize;

                if (currentAmount < maxStack)
                {
                    int spaceLeft = maxStack - currentAmount;
                    int amountToAdd = Mathf.Min(spaceLeft, remaining);

                    slot.SetItem(itemToAdd, currentAmount + amountToAdd);
                    remaining -= amountToAdd;

                    if (remaining <= 0)
                        return;
                }
            }
        }

        foreach (Slot slot in allSlots)
        {
            if (!slot.HasItem())
            {
                int amountToPlace = Mathf.Min(itemToAdd.maxStackSize, remaining);
                slot.SetItem(itemToAdd, amountToPlace);
                remaining -= amountToPlace;

                if (remaining <= 0)
                    return;
            }
        }

        if (remaining > 0)
        {
            Debug.Log("Inventory is full, could not add " + remaining + " of " + itemToAdd.itemName);//LOG
        }
    }

    private void StartDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Slot hovered = GetHoveredSlot();

            if (hovered != null && hovered.HasItem())
            {
                draggedSlot = hovered;
                isDragging = true;

                //Show drag item
                dragIcon.sprite = hovered.GetItem().icon;
                dragIcon.color = new Color(1, 1, 1, 0.5f);
                dragIcon.enabled = true;
            }
        }
    }

    private void EndDrag()
    {
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Slot hovered = GetHoveredSlot();

            if (hovered != null)
            {
                HandleDrop(draggedSlot, hovered);

                dragIcon.enabled = false;

                draggedSlot = null;
                isDragging = false;
            }
        }
    }

    private Slot GetHoveredSlot()
    {
        foreach (Slot s in allSlots)
        {
            if (s.hovering)
                return s;
        }

        return null;
    }

    private void HandleDrop(Slot from, Slot to)
    {
        if (from == to) return;
        if (from == null || !from.HasItem()) return;

        //Stacking same item
        if (to.HasItem() && to.GetItem() == from.GetItem())
        {
            int max = to.GetItem().maxStackSize;
            int space = max - to.GetAmount();

            if (space > 0)
            {
                int move = Mathf.Min(space, from.GetAmount());

                to.SetItem(to.GetItem(), to.GetAmount() + move);
                int remaining = from.GetAmount() - move;

                if (remaining > 0)
                    from.SetItem(from.GetItem(), remaining);
                else
                    from.ClearSlot();

                return;
            }
        }

        //Swap or move to empty slot
        if (to.HasItem())
        {
            ItemSO tempItem = to.GetItem();
            int tempAmount = to.GetAmount();

            to.SetItem(from.GetItem(), from.GetAmount());
            from.SetItem(tempItem, tempAmount);
        }
        else
        {
            //Move item to empty slot
            to.SetItem(from.GetItem(), from.GetAmount());
            from.ClearSlot();
        }
    }

    private void UpdateDragItemPosition()
    {
        if (isDragging)
        {
            dragIcon.transform.position = Input.mousePosition;
        }
    }

    private void Pickup()
    {
        if (lookedAtRenderer != null && Input.GetKeyDown(KeyCode.E))
        {
            Item item = lookedAtRenderer.GetComponent<Item>();
            if (item != null)
            {
                AddItem(item.item, item.amount);
                Destroy(item.gameObject);

                UpdateEquippedIK();
            }
        }
    }

    private void DetectLookedAtItem()
    {
        if (lookedAtRenderer != null)
        {
            lookedAtRenderer.material = originalMaterial;
            lookedAtRenderer = null;
            originalMaterial = null;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
            {
                Renderer rend = item.GetComponent<Renderer>();
                if (rend != null)
                {
                    originalMaterial = rend.material;
                    rend.material = highlightMaterial;
                    lookedAtRenderer = rend;
                }
            }
        }
    }

    private void UpdateHotBarOpacity()
    {
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            Image icon = hotbarSlots[i].GetComponent<Image>();
            if (icon != null)
            {
                icon.color = (i == equippedHotbarIndex) ? new Color(1, 1, 1, equippedOpacity) : new Color(1, 1, 1, normalOpacity);
            }
        }
    }

    private void HandleHotBarSelection()
    {
        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                equippedHotbarIndex = i;
                UpdateHotBarOpacity();
                UpdateEquippedIK();
            }
        }
    }

    private void HandleDropEquippedItem()
    {
        if (!Input.GetKeyDown(KeyCode.Q)) return;

        Slot equippedSlot = hotbarSlots[equippedHotbarIndex];

        if (!equippedSlot.HasItem()) return;

        ItemSO itemSO = equippedSlot.GetItem();
        GameObject prefab = itemSO.itemPrefab;

        if (prefab == null) return;

        GameObject dropped = Instantiate(prefab, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);

        // Re-enable colliders now that the item is back in the world as a pickup
        foreach (Collider col in dropped.GetComponentsInChildren<Collider>())
            col.enabled = true;

        Item item = dropped.GetComponent<Item>();
        item.item = itemSO;
        item.amount = equippedSlot.GetAmount();

        equippedSlot.ClearSlot();

        UpdateEquippedIK();
    }

    // Spawns the handItemPrefab as a live scene instance parented to the player.
    // Searches the full hierarchy recursively for IKGrabHandle so nesting depth
    // in the prefab does not matter.
    private void UpdateEquippedIK()
    {
        // Clear cached handle and destroy previous hand item
        currentGrabHandle = null;

        if (currentHandItemInstance != null)
        {
            Destroy(currentHandItemInstance);
            currentHandItemInstance = null;
        }

        if (ikControl == null) return;

        Slot equippedSlot = hotbarSlots[equippedHotbarIndex];

        if (!equippedSlot.HasItem())
        {
            ikControl.ikActive = false;
            return;
        }

        ItemSO item = equippedSlot.GetItem();

        if (item.handItemPrefab == null)
        {
            ikControl.ikActive = false;
            return;
        }

        //NOTE: Everything below this line was generated with AI so if something is wrong with this script its probably past this line -phil
        // --- DEBUG: log exactly which prefab asset is being instantiated ---
        Debug.Log($"[Inventory] handItemPrefab name: '{item.handItemPrefab.name}'");
        Debug.Log($"[Inventory] handItemPrefab instance ID: {item.handItemPrefab.GetInstanceID()}");
        Debug.Log($"[Inventory] handItemPrefab child count (prefab asset): {item.handItemPrefab.transform.childCount}");
        for (int i = 0; i < item.handItemPrefab.transform.childCount; i++)
            Debug.Log($"[Inventory]   Prefab child[{i}]: '{item.handItemPrefab.transform.GetChild(i).name}'");
        // ---------------------------------------------------------------------

        // Instantiate into the scene. Position is managed each frame by
        // IKControl.LateUpdate which parents it to the hand bone after IK resolves.
        currentHandItemInstance = Instantiate(item.handItemPrefab);

        // Disable all colliders on the equipped instance so it doesn't
        // physically interact with the player's hand or body while held.
        foreach (Collider col in currentHandItemInstance.GetComponentsInChildren<Collider>())
            col.enabled = false;

        // --- DEBUG: log the instantiated object's full hierarchy ---
        Debug.Log($"[Inventory] Instantiated child count: {currentHandItemInstance.transform.childCount}");
        foreach (Transform t in currentHandItemInstance.GetComponentsInChildren<Transform>(true))
            Debug.Log($"[Inventory]   Instance child: '{t.name}'");
        // ------------------------------------------------------------

        // Search the full hierarchy — handles any nesting depth in the prefab
        currentGrabHandle = FindDeepChild(currentHandItemInstance.transform, "IKGrabHandle");

        if (currentGrabHandle == null)
        {
            Debug.LogWarning($"[Inventory] 'IKGrabHandle' not found. Check the prefab asset directly.");
            ikControl.ikActive = false;
            return;
        }

        Debug.Log($"[Inventory] IKGrabHandle found successfully.");
        ikControl.ikActive = true;
    }

    // Recursively searches all descendants for a Transform with the given name
    private Transform FindDeepChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            Transform result = FindDeepChild(child, childName);
            if (result != null)
                return result;
        }
        return null;
    }
}