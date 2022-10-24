using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

[ExecuteInEditMode]
public abstract class Cube : MonoBehaviour
{
    [SerializeField] private CUBE_TYPE cubeType;
    public CUBE_TYPE CubeType => cubeType;

    [SerializeField] protected GameObject cube;

    // Vérifie si ce cube a été mangé ou pas
    public bool isManged()
    {
        return !cube.activeSelf;
    }
}