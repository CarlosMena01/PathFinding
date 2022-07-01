using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTree : MonoBehaviour
{

    IEnumerator BinaryTreeMaze() {

         for (int i = 0; i < _width; i += 2 )
            {
                for (int j = 0; j < _height; j += 2)
                {
                    
                    CellGrid c = cellDict[$"Cell_{i}_{j}"];
                    List<CellGrid> emptyNeighbours = new List<CellGrid>();

                    c.State(true, false, false);

                    if (i != 0 && cellDict[$"Cell_{i-1}_{j}"].getStateWall() == false){
                        emptyNeighbours.Add(cellDict[$"Cell_{i - 1}_{j}"]);
                    }

                    if (j != 0 && cellDict[$"Cell_{i}_{j-1}"].getStateWall() == false ){
                        emptyNeighbours.Add(cellDict[$"Cell_{i}_{j - 1}"]);
                    }

                    if (emptyNeighbours.Count != 0)
                    {
                        emptyNeighbours[Random.Range(0, emptyNeighbours.Count)].State(true, false, false);
                    }
                    if (true)
                        yield return new WaitForSeconds(0.01f);

                }
            }
            mazeEND = true;
    }
}
