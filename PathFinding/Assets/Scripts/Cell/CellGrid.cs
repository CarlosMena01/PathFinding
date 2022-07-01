using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour
{
    [SerializeField] private Color _colorBase, _colorWall, _colorInPath, _colorThePath;
    
    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private GridManager gridManager;

    [SerializeField] private bool isWall, isInPath, isThePath;

    [SerializeField] private int x,y;

    public void OnMouseDown() {
        if (Input.GetKey(KeyCode.LeftControl)){
            Debug.Log("Pressed ctrl+ left click.");
            gridManager.setEnd($"Cell_{x}_{y}");
            
        } else if (Input.GetKey(KeyCode.LeftAlt)){
            Debug.Log("Pressed alt+ left click.");
            gridManager.setStart($"Cell_{x}_{y}");
        } else {
            if(isWall) {
                _renderer.color = _colorBase;
                isWall = false;
            } else {
                _renderer.color = _colorWall;
                isWall = true;
            }
            Debug.Log($"Se cambio la celda: Cell_{x}_{y}");
        }
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
    }

    public void State(bool isWall, bool isInPath, bool isThePath){
        this.isWall     = isWall;
        this.isInPath   = isInPath;
        this.isThePath  = isThePath;

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
    }

    public bool getStateWall() {
        return this.isWall;
    }

    public int getX() {
        return this.x;
    }

    public int getY() {
        return this.y;
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
