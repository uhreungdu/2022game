using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApplicationButton : MonoBehaviour
{
   public GameObject window;
   public GameObject darkBackground;
   public void OnClickQuit()
   {
      Application.Quit();
   }
   
   public void OnClickCancel()
   {
      window.SetActive(false);
      darkBackground.SetActive(false);
   }
}
