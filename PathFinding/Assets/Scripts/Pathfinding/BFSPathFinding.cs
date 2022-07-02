using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSPathFinding : MonoBehaviour
{
    public Dictionary<string, CellGrid> cellDict = new Dictionary<string, CellGrid>();
    
    [SerializeField] GridManager gridManager;

    bool mazeEND;

    private float _height;
    private float _width;

    string endCell;
    string startCell;

    public  void StartSolve() {
        mazeEND = gridManager.isMazeEnd();
        _height = gridManager.getDimensions().x;
        _width = gridManager.getDimensions().y; 
        cellDict = gridManager.getCellsDict();

        startCell = gridManager.getStart();
        endCell   = gridManager.getEnd();

        StartCoroutine(BFSPathfinding(startCell, endCell));
    }

    IEnumerator BFSPathfinding(string start, string end){
        List<string> neighborhoodCells = Neighborhood(start);
        List<string> visited = new List<string>();
        LinkedList<string> queue = new LinkedList<string>();
        List<string> walls = new List<string>();


        CellGrid currentCell;
        CellGrid newCell = cellDict[neighborhoodCells[Random.Range(0, neighborhoodCells.Count)]]; //Seleccionamos un vecino al azar

        visited.Add(start);
        queue.AddLast(start);
        
        while(!mazeEND){
            mazeEND = gridManager.isMazeEnd();
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
                queue.AddFirst(currentCell.name);
                currentCell = newCell; //Nos movemos a la nueva celda
                currentCell.State(false,true,false);

            } else{
                visited.Add(currentCell.name);
                currentCell = cellDict[queue.First.Value];
                queue.RemoveFirst();
                currentCell.State(false,true,false);
            }

            //Condiciones de parada
            if(count >= _width * _height * 1){
                break;
            }
            if(currentCell.name == end){
                queue.AddLast(currentCell.name);
                break;
            }
            if(queue.Count < 1){
                break;
            }
            count++;
            yield return new WaitForSeconds(0.05f);
        }

        foreach(string cell in queue) {
            cellDict[cell].State(false,true,true);
            yield return new WaitForSeconds(0.05f);
        }
    }
    List<string> Neighborhood(string currentCell){
        List<string> result = new List<string>();
        Vector3 positionCell  = cellDict[currentCell].getPosition();

        if(cellDict.ContainsKey($"Cell_{positionCell.x}_{positionCell.y + 1}")) {
            result.Add($"Cell_{positionCell.x}_{positionCell.y + 1}");
        }
        if(cellDict.ContainsKey($"Cell_{positionCell.x + 1}_{positionCell.y}")) {
            result.Add($"Cell_{positionCell.x + 1}_{positionCell.y}");
        }
        if(cellDict.ContainsKey($"Cell_{positionCell.x}_{positionCell.y - 1}")) {
            result.Add($"Cell_{positionCell.x}_{positionCell.y - 1}");
        }
        if(cellDict.ContainsKey($"Cell_{positionCell.x - 1}_{positionCell.y}")) {
            result.Add($"Cell_{positionCell.x - 1}_{positionCell.y}");
        }
        return result;
    }
}
