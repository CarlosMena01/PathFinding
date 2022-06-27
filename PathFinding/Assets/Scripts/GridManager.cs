using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]  private int _width, _height;

    [SerializeField]  private CellGrid _cellPrefab;

    [SerializeField]  private Transform _cam;

    public Dictionary<string, CellGrid> cellDict = new Dictionary<string, CellGrid>();

    bool mazeEND = false;

    private void Start() {
        Generator();
        
        //StartCoroutine(BinaryTreeMaze());
        StartCoroutine(MazeDFS("Cell_0_0"));
        StopCoroutine(MazeDFS("Cell_0_0"));
        StartCoroutine(DFSPathFinding("Cell_0_0", "Cell_18_6"));
        //StopCoroutine(DFSPathFinding("Cell_0_0", "Cell_18_18"));
        
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
        mazeEND = true;
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
    private List<string> Neighborhood(string currentCell){
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
                        yield return new WaitForSeconds(0.00f);

                }
            }
        
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
    
    public IEnumerator DFSPathFinding(string startCell, string endCell){
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
                stack.Add(newCell.name);
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
}
