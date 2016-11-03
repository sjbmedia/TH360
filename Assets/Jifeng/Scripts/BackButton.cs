using UnityEngine;
using System.Collections;

namespace Jifeng.SoftVrEngine
{
    public class BackButton : MonoBehaviour
    {
        public void OnButtonConfirmed()
        {
            Application.LoadLevel("JfEntryMenu");
        }
    }
    
}
