using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour
{
    [SerializeField] private Color _colorBase, _colorWall, _colorInPath, _colorThePath, _colorStart, _colorEnd;
    
    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private bool isWall, isInPath, isThePath, isEnd, isStart;

    [SerializeField] private int x,y;


    public void OnMouseDown() {
        if(isWall) {
            _renderer.color = _colorBase;
            isWall = false;
        } else {
            _renderer.color = _colorWall;
            isWall = true;
        }
        Debug.Log($"Se cambio la celda: Cell_{x}_{y}");
        
    }

    private void Update() {
        if(isWall) {
            _renderer.color = _colorWall;

        } else if(isInPath) {
            if(isThePath) {
                _renderer.color = _colorThePath;
            } else {
                _renderer.color = _colorInPath;
            }
        } else {
            _renderer.color = _colorBase;
        }

        if (Input.GetKey(KeyCode.LeftControl)){
            if (Input.GetMouseButtonDown(1)){
                Debug.Log("Pressed ctrl+ right click.");
                if(isEnd) {
                    _renderer.color = _colorBase;
                    isEnd = false;
                    Debug.Log("Set to base");
                } else {
                    _renderer.color = _colorEnd;
                    isEnd = true;
                    Debug.Log("Set to end");
                }
            } else if (Input.GetMouseButtonDown(0)){
                Debug.Log("Pressed ctrl+ left click.");
                if(isStart) {
                    _renderer.color = _colorBase;
                    isStart = false;
                    Debug.Log("Set to base");
                } else {
                    _renderer.color = _colorStart;
                    isStart = true;
                    Debug.Log("Set to start");
                }
            }
        }
    }


    public void State(bool isWall, bool isInPath, bool isThePath, bool isStart, bool isEnd){
        this.isWall     = isWall;
        this.isInPath   = isInPath;
        this.isThePath  = isThePath;
        this.isStart = isStart;
        this.isEnd = isEnd;

        if(isWall) {
            _renderer.color = _colorWall;
        } else if(isInPath) {
            if(isThePath) {
                _renderer.color = _colorThePath;
            } else {
                _renderer.color = _colorInPath;
            }
        } else if(isStart) {
            _renderer.color = _colorStart;
        } else if(isStart) {
            _renderer.color = _colorEnd;
        } else {
            _renderer.color = _colorBase;
        }
    }

    public bool getStateWall() {
        return this.isWall;
    }

    public void Position(int x, int y){
        this.x = x;
        this.y = y;
    }

    public Vector3 getPosition(){
        return new Vector3(this.x, this.y);
    }

    public void setColor(Color newColor){
        _renderer.color = newColor;
    }
}
