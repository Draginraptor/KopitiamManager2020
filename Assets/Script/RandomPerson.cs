using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Largely isolated class soley to randomise the appearance of the Person entities
public class RandomPerson : MonoBehaviour
{
    // Fields should be private to the class, but needs to be accessed from Inspector
    [SerializeField] private MeshRenderer _head = null;
    [SerializeField] private MeshRenderer _body = null;
    [SerializeField] private List<Material> _skinMaterials = new List<Material>();
    [SerializeField] private List<Material> _shirtMaterials = new List<Material>();

    void Awake()
    {
        Randomise();
    }

    public void Randomise()
    {
        _head.material = GetRandomMaterialFrom(_skinMaterials);
        _body.material = GetRandomMaterialFrom(_shirtMaterials);
    }

    Material GetRandomMaterialFrom(List<Material> materials)
    {
        int randomIndex = Random.Range(0, materials.Count);
        return materials[randomIndex];
    }
}
