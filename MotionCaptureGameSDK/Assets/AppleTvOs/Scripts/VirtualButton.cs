using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AppleTvOs
{
    public class VirtualButton : MonoBehaviour
    {
        [SerializeField] public int ButtonId;
        public void Selected(bool isActive)
        {
            transform.Find("Image_selected").gameObject.SetActive(isActive);
        }

        public void SetText(string txt)
        {
            transform.Find("Text").GetComponent<Text>().text = txt;
        }
    }
}
