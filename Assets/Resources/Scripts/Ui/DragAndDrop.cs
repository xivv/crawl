
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IDragHandler, IDropHandler, IBeginDragHandler
{

    protected bool selected = false;
    protected string oldParentName;

    List<RaycastResult> hitObjects = new List<RaycastResult>();

    public virtual void WhileDrag()
    {
        Vector3 worldPosition = Input.mousePosition;
        gameObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, 2);
    }

    protected GameObject GetOldParent()
    {
        return GameObject.Find(oldParentName).gameObject;
    }

    protected void GoToOldParent()
    {
        gameObject.transform.SetParent(GetOldParent().transform);
    }

    protected virtual void OnDrag()
    {
        selected = true;
        oldParentName = gameObject.transform.parent.gameObject.name;
    }

    protected virtual void OnDrop()
    {
        selected = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (selected)
        {
            WhileDrag();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDrop();
    }

    protected virtual GameObject GetObjectUnderMouse()
    {
        var pointer = new PointerEventData(EventSystem.current);

        pointer.position = Input.mousePosition;

        EventSystem.current.RaycastAll(pointer, hitObjects);

        if (hitObjects.Count <= 0) return null;


        for (var i = hitObjects.Count - 1; i >= 0; i--)
        {
            RaycastResult element = hitObjects[i];

            if (element.gameObject == gameObject)
            {
                hitObjects.Remove(element);
            }
        }

        if (hitObjects.Count <= 0) return null;

        return hitObjects[0].gameObject;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnDrag();
    }
}
