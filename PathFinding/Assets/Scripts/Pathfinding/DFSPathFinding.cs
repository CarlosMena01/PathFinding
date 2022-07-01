using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFSPathFinding : MonoBehaviour
{   
    [SerializeField] GridManager gridManager;

    public Dictionary<string, CellGrid> cellDict = new Dictionary<string, CellGrid>();

    bool mazeEND;

    private float _height;
    private float _width;

    public  void Start() {
        mazeEND = gridManager.isMazeEnd();
        _height = gridManager.getDimensions().x;
        _width = gridManager.getDimensions().y; 
        cellDict = gridManager.getCellsDict();
    }
    IEnumerator DFSPathfinding(string startCell, string endCell){
        List<string>    neighborhoodCells   = Neighborhood(startCell);
        List<string>    visited             = new List<string>();
        List<string>    stack               = new List<string>();
        List<string>    walls               = new List<string>();


        CellGrid currentCell;
        CellGrid newCell = cellDict[neighborhoodCells[Random.Range(0, neighborhoodCells.Count)]]; //Seleccionamos un vecino al azar

        visited.Add(startCell);
        stack.Add(startCell);

        
        
        while(!mazeEND){

            yield return new WaitForSeconds(0.05f);
        }; //Esperamos a que se construya el laberinto

        //Creamos una lista con todos los muros
        for(int x = 0; x < _width; x++) {
            for(int y = 0; y < _height; y++) {
                currentCell = cellDict[$"Cell_{x}_{y}"];
                if(currentCell.getStateWall()) {
                    walls.Add(currentCell.name);
                    Debug.Log($"WALLS: {currentCell.name}");
                }
            }   
        }

        currentCell = cellDict[startCell];
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
                Debug.Log($"NV: {currentCell.name}");
                visited.Add(currentCell.name);
                currentCell = cellDict[stack[stack.Count - 1]];
                stack.RemoveAt(stack.Count -1);
                currentCell.State(false,true,false);
            }

            //Condiciones de parada
            if(count >= _width * _height * 1){
                Debug.Log($"Se recorrieron {count} celdas");
                break;
            }
            if(currentCell.name == endCell){
                stack.Add(currentCell.name);
                break;
            }
            if(stack.Count < 1){
                Debug.Log("Stack está vacío");
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
