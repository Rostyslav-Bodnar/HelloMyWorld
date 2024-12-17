using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CursorController : MonoBehaviour
{
    [SerializeField] public GameObject cursor;
    [SerializeField] private Grid grid;
    [SerializeField] private LayerMask selectableLayer;

    private Vector3Int startDrag;
    private Vector3Int endDrag;
    private Vector3 dragSize;
    Vector3 dragCenter;
    private bool isDragging;

    public List<GameObject> selectedObjects = new List<GameObject>();

    public void HandleCursorInput()
    {
        HandleMouseInput(GetCursorPosition());

        if (!isDragging)
        {
            cursor.transform.position = grid.CellToWorld(GetCursorPosition());
        }

    }

    public Vector3Int GetCursorPosition()
    {
        Vector3 mousePos = InputManager.Current.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);
        return gridPosition;
    }

    private void Update()
    {
        /*if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Якщо Raycast потрапляє в об'єкт, зберігаємо координати точки
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = hit.point;

                // Встановлюємо ціль для NavMeshAgent
                test.SetDestination(targetPosition);
            }
        }*/
    }

    private void HandleMouseInput(Vector3Int gridPosition)
    {
        if (Input.GetMouseButtonDown(0))
        {
            startDrag = gridPosition;
            isDragging = true;

            cursor.transform.position = grid.CellToWorld(startDrag);
            cursor.transform.localScale = Vector3.one; 
        }
        else if (Input.GetMouseButton(0))
        {
            endDrag = gridPosition;

            float xSize = Mathf.Abs(endDrag.x - startDrag.x) + 1f;
            float zSize = Mathf.Abs(endDrag.z - startDrag.z) + 1f;

            Vector3Int newCursorPosition = startDrag;

            if (endDrag.x < startDrag.x)
            {
                newCursorPosition.x = endDrag.x; 
            }
            if (endDrag.z < startDrag.z)
            {
                newCursorPosition.z = endDrag.z; 
            }

            dragSize = new Vector3(xSize, 1f, zSize);
            cursor.transform.localScale = dragSize;
            cursor.transform.position = grid.CellToWorld(newCursorPosition); 
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isDragging = false;
            SelectObjectsInDragArea();

            cursor.transform.localScale = Vector3.one;
        }
    }

    private void SelectObjectsInDragArea()
    {
        selectedObjects.Clear();

        Vector3 minBounds = Vector3.Min(startDrag, endDrag);
        Vector3 maxBounds = Vector3.Max(startDrag, endDrag);

        Bounds selectionBounds = new Bounds();
        selectionBounds.SetMinMax(minBounds, maxBounds);
        dragCenter = (startDrag + endDrag) / 2;
        //Debug.Log(dragCenter);
        Collider[] collidersInRange = Physics.OverlapBox(dragCenter + new Vector3(0.5f, 0, 0.5f), dragSize / 2, Quaternion.identity, selectableLayer);
        foreach (Collider collider in collidersInRange)
        {
            GameObject obj = collider.gameObject;
            Debug.Log(obj.transform.position);
            if (!selectedObjects.Contains(obj))
            {
                selectedObjects.Add(obj);
                //Debug.Log("Selected: " + obj.name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(dragCenter + new Vector3(0.5f, 0, 0.5f), dragSize);
        
    }


    public void SetCursorScale(float width, float height)
    {
        cursor.transform.localScale = new Vector3(width, 1f, height);
    }

}
