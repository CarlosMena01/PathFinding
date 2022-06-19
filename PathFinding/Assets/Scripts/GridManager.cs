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
        List<CellGrid> neighborhoodCells = NeighborhoodDFS(startCell);

        for(int i = 0; i < neighborhoodCells.Count; i++) {
            neighborhoodCells[i].State(true,false,false);
        }

    }

    private List<CellGrid> NeighborhoodDFS(string currentCell){
        List<CellGrid> result = new List<CellGrid>();
        Vector3 positionCell  = cellDict[currentCell].getPosition();

        if(cellDict.ContainsKey($"Cell_{positionCell.x}_{positionCell.y + 2}")) {
            result.Add(cellDict[$"Cell_{positionCell.x}_{positionCell.y + 2}"]);
        }
        if(cellDict.ContainsKey($"Cell_{positionCell.x + 2}_{positionCell.y}")) {
            result.Add(cellDict[$"Cell_{positionCell.x + 2}_{positionCell.y}"]);
        }
        if(cellDict.ContainsKey($"Cell_{positionCell.x}_{positionCell.y - 2}")) {
            result.Add(cellDict[$"Cell_{positionCell.x}_{positionCell.y - 2}"]);
        }
        if(cellDict.ContainsKey($"Cell_{positionCell.x - 2}_{positionCell.y}")) {
            result.Add(cellDict[$"Cell_{positionCell.x - 2}_{positionCell.y}"]);
        }
        return result;
    }

}
