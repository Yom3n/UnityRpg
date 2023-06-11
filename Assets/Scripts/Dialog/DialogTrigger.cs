using UnityEngine;

namespace Dialog
{
    public class DialogTrigger : MonoBehaviour
    {
        public void OpenDialogBox(TextAsset inkJson)
        {
            DialogManager.GetInstance().EnterDialogMode(inkJson);
        }
    }
}