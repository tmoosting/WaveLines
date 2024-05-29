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
          if (Input.GetMouseButtonUp(0))
          { 
               WaveController.Instance.LeftClickLineWithIndex(lineIndex);
          }
     
          else if (Input.GetMouseButtonUp(1))
          {
               WaveController.Instance.RightClickLineWithIndex(lineIndex);

          }
     }


   

     public void SetMaterial(Material selectedMarkerMaterial)
     {
          GetComponent<MeshRenderer>().material = selectedMarkerMaterial;
     }
}
