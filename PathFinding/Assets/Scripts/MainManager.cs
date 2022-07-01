using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    //Creación de laberintos 
    [SerializeField] private GameObject     MG_binaryTree;
    [SerializeField] private GameObject     MG_DFS;
    [SerializeField] private GameObject     MG_RecursiveDivision;

    //Pathfinding
    [SerializeField] private GameObject     PF_DFS;
    [SerializeField] private GameObject     PF_BFS;

    private int mazeGeneration  = 0;
    private int pathfinding     = 0;

    public void StartGame(){
        if(mazeGeneration == 1) {
            MG_binaryTree.SetActive(true);
        } else if(mazeGeneration == 2) {
            MG_DFS.SetActive(true);   
        } else if (mazeGeneration == 3){
            MG_RecursiveDivision.SetActive(true);
        }

        if(pathfinding == 1) {
            PF_DFS.SetActive(true);
        } else if(pathfinding == 2) {
            PF_BFS.SetActive(true);   
        } 
    }

    public void setMG_BinaryTree(){
        this.mazeGeneration = 1;
    }
    public void setMG_DFS(){
        this.mazeGeneration = 2;
    }
    public void setMG_RecursiveDivision(){
        this.mazeGeneration = 3;
    }

    public void setPF_DFS(){
        this.pathfinding = 1;
    }
    public void setPF_BFS(){
        this.pathfinding = 2;
    }
}
