using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]  private int _width, _height;

    [SerializeField]  private CellGrid _cellPrefab;

    [SerializeField]  private Transform _cam;

    public Dictionary<string, CellGrid> cellDict = new Dictionary<string, CellGrid>();

    private void Start() {
        Generator();

        MazeDFS("Cell_2_2");
    }
    
    private void Generator(){
        for(int x = 0; x < _width; x++) {
            for(int y = 0; y < _height; y++) {
                var spawCell = Instantiate(_cellPrefab, new Vector3(x,y), Quaternion.identity);
                spawCell.name = $"Cell_{x}_{y}";
                spawCell.Position(x,y);
                spawCell.State(false, false, false);

                cellDict.Add(spawCell.name, spawCell);
            }   
        }

        _cam.transform.position = new Vector3((float) _width/2 - 0.5f, (float) _height/2 - 0.5f, -100);
    }

    private void MazeChess(){
        for(int x = 0; x < _width; x++) {
            for(int y = 0; y < _height; y++) {
                var currentCell = cellDict[$"Cell_{x}_{y}"];

                bool isWall = ((x%2 == 0 && y%2 != 0) || (x%2 != 0 && y%2 == 0)); 
                currentCell.State(isWall, false, false);
            }   
        }            
    }

    private void MazeDFS(string startCell){
        List<string>    neighborhoodCells   = NeighborhoodDFS(startCell);
        List<string>    visited             = new List<string>();
        List<string>    stack               = new List<string>();

        CellGrid currentCell = cellDict[startCell];
        CellGrid newCell = cellDict[neighborhoodCells[Random.Range(0, neighborhoodCells.Count)]]; //Seleccionamos un vecino al azar

        visited.Add(startCell);
        stack.Add(startCell);

        //Generamos las celdas aisladas
        for(int x = 0; x < _width; x++) {
            for(int y = 0; y < _height; y++) {
                currentCell = cellDict[$"Cell_{x}_{y}"];

                bool isWall = !(x%2 == 0 && y%2 == 0); 
                currentCell.State(isWall, false, false);
            }   
        }

        int count = 0;

        while (true)
        {
            Debug.Log(currentCell.name);
            neighborhoodCells = NeighborhoodDFS(currentCell.name); // Actualizamos el vecindario
            
            //Eliminamos las celdas visitadas
            for(int i = 0; i < neighborhoodCells.Count; i++) {
                if(visited.Contains(neighborhoodCells[i])) {
                    neighborhoodCells.Remove(neighborhoodCells[i]);
                }                    
            }

            if(neighborhoodCells.Count >= 0) {
                newCell = cellDict[neighborhoodCells[Random.Range(0, neighborhoodCells.Count)]]; //Seleccionamos un vecino al azar
                //Si no ha sido visitada, creamos un puente entre ellas
                if((newCell.getPosition() - currentCell.getPosition()).x > 0) {
                    //La nueva celda está a la derecha
                    string name = $"Cell_{newCell.getPosition().x - 1}_{newCell.getPosition().y}";  
                    if(cellDict.ContainsKey(name)){
                        cellDict[name].State(false,false,false);
                    }
                    
                } else if((newCell.getPosition() - currentCell.getPosition()).x < 0) {
                    //La nueva celda está a la izquierda
                    string name = $"Cell_{newCell.getPosition().x + 1}_{newCell.getPosition().y}";  
                    if(cellDict.ContainsKey(name)){
                        cellDict[name].State(false,false,false);
                    }
                    
                } else if((newCell.getPosition() - currentCell.getPosition()).y < 0) {
                    //La nueva celda está abajo
                    string name = $"Cell_{newCell.getPosition().x}_{newCell.getPosition().y + 1}";  
                    if(cellDict.ContainsKey(name)){
                        cellDict[name].State(false,false,false);
                    }
                    
                } else if((newCell.getPosition() - currentCell.getPosition()).y > 0) {
                    //La nueva celda está arriba
                    string name = $"Cell_{newCell.getPosition().x}_{newCell.getPosition().y - 1}";  
                    if(cellDict.ContainsKey(name)){
                        cellDict[name].State(false,false,false);
                    }
                }

                //Añadimos la celda a las visitadas
                visited.Add(newCell.name);
                stack.Add(newCell.name);
                currentCell = newCell; //Nos movemos a la nueva celda
            } else{
                currentCell = cellDict[stack[stack.Count - 1]];
                stack.Remove(newCell.name);
            }

            //Condiciones de parada
            if(count >= _width * _height * 100){
                Debug.Log($"Se recorrieron {count} celdas");
                break;
            }
            if(currentCell.name == startCell){
                break;
            }
            if(stack.Count <= 1){
                break;
            }
            count++;
        }

    }

    private List<string> NeighborhoodDFS(string currentCell){
        List<string> result = new List<string>();
        Vector3 positionCell  = cellDict[currentCell].getPosition();

        if(cellDict.ContainsKey($"Cell_{positionCell.x}_{positionCell.y + 2}")) {
            result.Add($"Cell_{positionCell.x}_{positionCell.y + 2}");
        }
        if(cellDict.ContainsKey($"Cell_{positionCell.x + 2}_{positionCell.y}")) {
            result.Add($"Cell_{positionCell.x + 2}_{positionCell.y}");
        }
        if(cellDict.ContainsKey($"Cell_{positionCell.x}_{positionCell.y - 2}")) {
            result.Add($"Cell_{positionCell.x}_{positionCell.y - 2}");
        }
        if(cellDict.ContainsKey($"Cell_{positionCell.x - 2}_{positionCell.y}")) {
            result.Add($"Cell_{positionCell.x - 2}_{positionCell.y}");
        }
        return result;
    }

}
