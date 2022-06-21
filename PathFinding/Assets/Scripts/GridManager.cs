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
        Debug.Log("Empezamos");
        MazeDFS("Cell_8_8");
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
        

        CellGrid currentCell =  cellDict[neighborhoodCells[Random.Range(0, neighborhoodCells.Count)]];
        CellGrid newCell;
        visited.Add(currentCell.name);

        int count = 0;

        while (currentCell.name != startCell)
        {
            neighborhoodCells   = NeighborhoodDFS(currentCell.name); //Actualizamos los nuevos vecinos
            newCell = cellDict[neighborhoodCells[Random.Range(0, neighborhoodCells.Count)]]; //Seleccionamos un vecino al azar
            
            if(visited.Contains(newCell.name)){
                //Si la celta ya había sido vistada, se cera un muro
                if((newCell.getPosition() - currentCell.getPosition()).x > 0) {
                    string name = $"Cell_{newCell.getPosition().x - 1}_{newCell.getPosition().y}";  
                    if(cellDict.ContainsKey(name)){
                        cellDict[name].State(true,false,false);
                    }
                    
                } else if((newCell.getPosition() - currentCell.getPosition()).x < 0) {
                    string name = $"Cell_{newCell.getPosition().x + 1}_{newCell.getPosition().y}";  
                    if(cellDict.ContainsKey(name)){
                        cellDict[name].State(true,false,false);
                    }
                    
                } else if((newCell.getPosition() - currentCell.getPosition()).y < 0) {
                    string name = $"Cell_{newCell.getPosition().x}_{newCell.getPosition().y + 1}";  
                    if(cellDict.ContainsKey(name)){
                        cellDict[name].State(true,false,false);
                    }
                    
                } else if((newCell.getPosition() - currentCell.getPosition()).y > 0) {
                    string name = $"Cell_{newCell.getPosition().x}_{newCell.getPosition().y - 1}";  
                    if(cellDict.ContainsKey(name)){
                        cellDict[name].State(true,false,false);
                    }
                    
                }
            } else {
                currentCell = newCell;
                visited.Add(currentCell.name);
            }

            if(count >= _width * _height){
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
