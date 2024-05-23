using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMarker : MonoBehaviour
{
     public int lineIndex;


     public void Initialize(int index)
     {
          lineIndex = index;
     }
     
     private void OnMouseUp()
     {
          WaveController.Instance.ClickLineWithIndex(lineIndex);
     }

   

     public void SetMaterial(Material selectedMarkerMaterial)
     {
          GetComponent<MeshRenderer>().material = selectedMarkerMaterial;
     }
}
