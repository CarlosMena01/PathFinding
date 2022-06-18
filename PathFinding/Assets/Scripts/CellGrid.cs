using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour
{
    [SerializeField] private Color _colorBase, _colorWall, _colorInPath, _colorThePath;
    
    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private bool isWall, isInPath, isThePath;

    public void Update() {
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

    public void OnMouseDown() {
        if(isWall) {
            _renderer.color = _colorBase;
            isWall = false;
        } else {
            _renderer.color = _colorWall;
            isWall = true;
        }
        
    }
    public void State(bool isWall, bool isInPath, bool isThePath){
        this.isWall     = isWall;
        this.isInPath   = isInPath;
        this.isThePath  = isThePath;
    }
}
