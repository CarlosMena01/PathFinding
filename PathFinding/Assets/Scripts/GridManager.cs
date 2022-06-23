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
        
        //StartCoroutine(BinaryTreeMaze());
        StartCoroutine(MazeDFS("Cell_0_0"));
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

    private IEnumerator MazeDFS(string startCell){
        List<string>    neighborhoodCells   = NeighborhoodDFS(startCell);
        List<string>    visited             = new List<string>();
        List<string>    stack               = new List<string>();

        CellGrid currentCell;
        CellGrid newCell = cellDict[neighborhoodCells[Random.Range(0, neighborhoodCells.Count)]]; //Seleccionamos un vecino al azar

        visited.Add(startCell);
        stack.Add(startCell);

        //Generamos las celdas aisladas
        for(int x = 0; x < _width; x++) {
            for(int y = 0; y < _height; y++) {
                currentCell = cellDict[$"Cell_{x}_{y}"];
                currentCell.State(true, false, false);
            }   
        }
        
        currentCell = cellDict[startCell];
        currentCell.State(false,false,false);
        int count = 0;
        
        while (true)
        {
            currentCell.setColor(Color.red);
            Debug.Log($"Current: {currentCell.name}");
            neighborhoodCells = NeighborhoodDFS(currentCell.name); // Actualizamos el vecindario
            
            //Eliminamos las celdas visitadas
            for(int i = 0; i < visited.Count; i++) {
                
                if(neighborhoodCells.Contains(visited[i])) {
                    //Debug.Log($"Removed: {neighborhoodCells[i]}");
                    neighborhoodCells.Remove(visited[i]);
                    
                }
            }
            
            if(neighborhoodCells.Count > 0) {
                
                newCell = cellDict[neighborhoodCells[Random.Range(0, neighborhoodCells.Count)]]; //Seleccionamos un vecino al azar
                //Si no ha sido visitada, creamos un puente entre ellas
                Vector3 wallPosition = new Vector3();
                wallPosition = (currentCell.getPosition() + newCell.getPosition())/2;
                string name = $"Cell_{wallPosition.x}_{wallPosition.y}";  
                if(cellDict.ContainsKey(name)){
                    cellDict[name].State(false,false,false);
                }

                //Añadimos la celda a las visitadas
                visited.Add(newCell.name);
                stack.Add(newCell.name);
                currentCell = newCell; //Nos movemos a la nueva celda
                currentCell.State(false,false,false);
                
            } else{
                Debug.Log($"No vecinos");
                visited.Add(currentCell.name);
                currentCell = cellDict[stack[stack.Count - 1]];
                stack.RemoveAt(stack.Count -1);
                currentCell.State(false,false,false);
            }

            //Condiciones de parada
            if(count >= _width * _height * 1){
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
            yield return new WaitForSeconds(0.05f);
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

    IEnumerator BinaryTreeMaze() {

         for (int i = 0; i < _width; i += 2 )
            {
                for (int j = 0; j < _height; j += 1)
                {
                    
                    CellGrid c = cellDict[$"Cell_{i}_{j}"];
                    List<CellGrid> emptyNeighbours = new List<CellGrid>();

                    if (i != 0 ){
                        emptyNeighbours.Add(cellDict[$"Cell_{i - 1}_{j}"]);
                    }

                    if (j != 0 ){
                        emptyNeighbours.Add(cellDict[$"Cell_{i}_{j - 1}"]);
                    }

                    if (emptyNeighbours.Count != 0)
                    {
                        emptyNeighbours[Random.Range(0, emptyNeighbours.Count)].State(true, false, false);
                    }
                    if (true)
                        yield return new WaitForSeconds(0.05f);

                }
            }
        
    }

}
