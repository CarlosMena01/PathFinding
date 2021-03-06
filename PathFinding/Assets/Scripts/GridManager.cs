using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]  private int _width, _height;

    [SerializeField]  private CellGrid _cellPrefab;

    [SerializeField]  private Outskirts _outskirtPrefab;

    [SerializeField]  private Transform _cam;

    [SerializeField] private GameObject _cellStart;

    [SerializeField] private GameObject _cellEnd;

    public Dictionary<string, CellGrid> cellDict = new Dictionary<string, CellGrid>();

    bool mazeEND = false;

    private string startCell = "Cell_1_1";
    private string endCell = "Cell_31_15";

    private Vector3 startPosition, endPosition; 

    private void Start() {
        Generator();
    }

    private void Update() {
        startPosition = cellDict[startCell].getPosition();
        endPosition = cellDict[endCell].getPosition();

        if (cellDict[endCell].getStateWall())
        {
            List<string> keyList = new List<string>(cellDict.Keys);
            this.endCell = keyList[Random.Range(0, keyList.Count)];
        }
        if (cellDict[startCell].getStateWall())
        {
            List<string> keyList = new List<string>(cellDict.Keys);
            this.startCell = keyList[Random.Range(0, keyList.Count)];
        }

        _cellStart.transform.position = startPosition;
        _cellEnd.transform.position = endPosition;
        
        
    }

    private void Generator(){
        //Creamos toda la cuadricula
        for(int x = 0; x < _width; x++) {
            for(int y = 0; y < _height; y++) {
                var spawCell = Instantiate(_cellPrefab, new Vector3(x,y), Quaternion.identity);
                spawCell.name = $"Cell_{x}_{y}";
                spawCell.Position(x,y);
                spawCell.State(false, false, false);
                cellDict.Add(spawCell.name, spawCell);
            }   
        }

        //Creamos los contornos
        for(int x = -1; x < _width +1; x++) {
            var spawOutskirt = Instantiate(_outskirtPrefab, new Vector3(x,-1), Quaternion.identity);
            spawOutskirt = Instantiate(_outskirtPrefab, new Vector3(x, _height), Quaternion.identity);

        }
        for(int y = -1; y < _height +1; y++) {
            var spawOutskirt = Instantiate(_outskirtPrefab, new Vector3(-1,y), Quaternion.identity);
            spawOutskirt = Instantiate(_outskirtPrefab, new Vector3(_width,y), Quaternion.identity);

        }   

        _cam.transform.position = new Vector3((float) _width/2 - 0.5f, (float) _height/2 - 0.5f, -100);
    }

    public Vector3 getDimensions(){
        return new Vector3(this._height, this._width);
    }

    public Dictionary<string, CellGrid> getCellsDict(){
        return this.cellDict;
    }

    public bool isMazeEnd(){
        return this.mazeEND;
    } 

    public void setMazeState(bool mazeState){
        this.mazeEND = mazeState;
    } 

    public void setStart(string startName){
        this.startCell = startName;
        Debug.Log($"Punto inicio es {startName}");
    }

    public void setEnd(string endName){
        this.endCell = endName;
        Debug.Log($"Punto final es {endName}");
    }

    public string getStart(){
        return this.startCell;
        Debug.Log($"Punto inicial es {this.startCell}");
    }

    public string getEnd(){
        return this.endCell;
        Debug.Log($"Punto finalss es {this.endCell}");
    }
}