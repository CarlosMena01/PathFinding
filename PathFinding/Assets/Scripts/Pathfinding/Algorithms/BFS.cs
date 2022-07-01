using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFSPathing: MonoBehaviour {

    IEnumerator BFSPathfinding(string start, string end){
        List<string> neighborhoodCells = Neighborhood(start);
        List<string> visited = new List<string>();
        List<string> stack = new List<string>();
        List<string> walls = new List<string>();


        CellGrid currentCell;
        CellGrid newCell = cellDict[neighborhoodCells[Random.Range(0, neighborhoodCells.Count)]]; //Seleccionamos un vecino al azar

        visited.Add(start);
        stack.Add(start);
        
        while(!mazeEND){

            yield return new WaitForSeconds(0.01f);
        }; //Esperamos a que se construya el laberinto

        //Creamos una lista con todos los muros
        for(int x = 0; x < _width; x++) {
            for(int y = 0; y < _height; y++) {
                currentCell = cellDict[$"Cell_{x}_{y}"];
                if(currentCell.getStateWall()) {
                    walls.Add(currentCell.name);
                }
            }   
        }

        currentCell = cellDict[start];
        currentCell.State(false,true,false);
        int count = 0;
        while (mazeEND)
        {
            Debug.Log(currentCell.name);
            currentCell.setColor(Color.red);

            neighborhoodCells = Neighborhood(currentCell.name); // Actualizamos el vecindario
            
            //Eliminamos las celdas que fueron visitadas
            for(int i = 0; i < visited.Count; i++) {   
                if(neighborhoodCells.Contains(visited[i])) {
                    //Debug.Log($"Removed: {neighborhoodCells[i]}");
                    neighborhoodCells.Remove(visited[i]);   
                    //Debug.Log($"Visited: {visited[i]}");
                }
            }

            //Eliminamos las celdas que son muros
            for(int i = 0; i < walls.Count; i++) {   
                if(neighborhoodCells.Contains(walls[i])) {
                    //Debug.Log($"Removed: {neighborhoodCells[i]}");
                    neighborhoodCells.Remove(walls[i]);   
                }
            }
            
            if(neighborhoodCells.Count > 0) {
                
                newCell = cellDict[neighborhoodCells[Random.Range(0, neighborhoodCells.Count)]]; //Seleccionamos un vecino al azar
                //Añadimos la celda a las visitadas
                visited.Add(newCell.name);
                stack.Add(currentCell.name);
                currentCell = newCell; //Nos movemos a la nueva celda
                currentCell.State(false,true,false);

            } else{
                visited.Add(currentCell.name);
                currentCell = cellDict[stack[stack.Count - 1]];
                stack.RemoveAt(stack.Count -1);
                currentCell.State(false,true,false);
            }

            //Condiciones de parada
            if(count >= _width * _height * 1){
                break;
            }
            if(currentCell.name == end){
                stack.Add(currentCell.name);
                break;
            }
            if(stack.Count < 1){
                break;
            }
            count++;
            yield return new WaitForSeconds(0.05f);
        }

        foreach(string cell in stack) {
            cellDict[cell].State(false,true,true);
            yield return new WaitForSeconds(0.05f);
        }
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class BFSPathing: MonoBehaviour {

//     IEnumerator BFSPathing(string start, string target){

//         while(!mazeEND){
//             yield return new WaitForSeconds(0.05f);
//         }; 
//         // CellGrid cells = cellDict[$"Cell_{i}_{j}"];
//         List<string> visited = new List<string>();

//         Queue<CellGrid> queue = new Queue<CellGrid>();
//         CellGrid c_start = cellDict[start];
//         queue.Enqueue(c_start);
        
//         c_start.State(false, true, true);
//         while (queue.Count != 0)
//         {
//             CellGrid c = queue.Dequeue();
//             c.State(false, true, false);

//             if ($"Cell_{c.getPosition().x}_{c.getPosition().y}" == target) break;

//             List<CellGrid> neighbours = new List<CellGrid>();

//             if (neighbours.Count == 0) continue;

//             for (int i = 0; i < neighbours.Count; i++)
//             {
//                 CellGrid neighbour = neighbours[i];

//                 if (neighbour.getStateWall() == true) continue;

//                 neighbour.State(false, true, false);
//                 queue.Enqueue(neighbour);

//                 // if (target != neighbour)
//                 //     PathfindingTools.SetCellColorByDistance(neighbour, neighbour.GetHelperNum());

//                 if (target != $"Cell_{neighbour.getPosition().x}_{neighbour.getPosition().y}"){
//                     c.State(false, true, false);
//                 }
//             }

//             if (true)
//                 yield return new WaitForSeconds(0.01f);

//         }

//     }
// }
