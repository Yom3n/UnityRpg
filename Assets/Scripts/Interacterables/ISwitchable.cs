using UnityEditor;
using UnityEngine;

namespace Interactor
{
    
    public interface ISwitchable
    {
        public bool IsActive { get; set; }

        public void OnActivate();
        public void OnDeactivate();
    }
}