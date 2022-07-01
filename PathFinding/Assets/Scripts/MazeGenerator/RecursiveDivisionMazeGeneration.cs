using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveDivisionMazeGeneration : MonoBehaviour
{
    public Dictionary<string, CellGrid> cellDict = new Dictionary<string, CellGrid>();
    
    [SerializeField] GridManager gridManager;

    private int _height;
    private int _width;

    public  void Start() {
        _height = (int) gridManager.getDimensions().x;
        _width = (int) gridManager.getDimensions().y; 
        cellDict = gridManager.getCellsDict();

        StartCoroutine(RecursiveDivision());
    }

    bool yieldBool;

    IEnumerator RecursiveDivision()
    {
        int lowerX=0;
        int lowerY=0;
        int upperX=_width;
        int upperY=_height;
        yield return RecursivePathing(lowerX, upperX, lowerY, upperY);
    }

    IEnumerator RecursivePathing(int lowerX, int upperX, int lowerY, int upperY)
    {
        if (lowerX >= upperX - 2 || lowerY >= upperY - 2) yield break;

        if (Random.Range(0, 2) == 0)
        {
            yield return StartCoroutine(Vertical(lowerX, upperX, lowerY, upperY));

            if (!yieldBool)
            {
                Horizontal(lowerX, upperX, lowerY, upperY);
            }
        }
        else
        {
            yield return StartCoroutine(Horizontal(lowerX, upperX, lowerY, upperY));

            if (!yieldBool)
            {
                Vertical(lowerX, upperX, lowerY, upperY);
            }

        }
    }

    IEnumerator Vertical(int lowerX, int upperX, int lowerY, int upperY)
    {
        CellGrid currentCell;
        
        if (upperX - lowerX - 3 <= 0)
        {
            yieldBool = false;
            yield break;
        }
        else
        {
            int idx = Random.Range(0, upperX - lowerX - 3) + lowerX + 2;
            int wallSpaceidx = Random.Range(0, upperY - lowerY - 1) + lowerY + 1;

            for (int i = lowerY + 1; i < upperY; i++)
            {
                currentCell = cellDict[$"Cell_{idx}_{i}"];
                currentCell.setColor(Color.red);
                currentCell.State(true, false, false);
            }

            currentCell = cellDict[$"Cell_{idx}_{wallSpaceidx}"];
            currentCell.State(false, false, false);

            yield return StartCoroutine(RecursivePathing(lowerX, idx, lowerY, upperY));
            yield return StartCoroutine(RecursivePathing(idx, upperX, lowerY, upperY));
        }

        yieldBool = true;
    }

    IEnumerator Horizontal(int lowerX, int upperX, int lowerY, int upperY)
    {
        CellGrid currentCell;


        if (upperY - lowerY - 3 <= 0)
        {
            yieldBool = false;
            yield break;
        }

        int idx = Random.Range(0, upperY - lowerY - 3) + lowerY + 2;
        int wallSpaceidx = Random.Range(0, upperX - lowerX - 1) + lowerX + 1;

        for (int i = lowerX + 1; i < upperX; i++)
        {
            currentCell = cellDict[$"Cell_{i}_{idx}"];
            currentCell.State(true, false, false);
        }

        currentCell = cellDict[$"Cell_{wallSpaceidx}_{idx}"];
        currentCell.State(false, false, false);

        yield return StartCoroutine(RecursivePathing(lowerX, upperX, lowerY, idx));
        yield return StartCoroutine(RecursivePathing(lowerX, upperX, idx, upperY));

        yieldBool = true;
    }
}
