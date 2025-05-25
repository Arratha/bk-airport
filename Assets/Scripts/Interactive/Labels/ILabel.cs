using UnityEngine;

namespace Interactive.Labels
{
    public interface ILabel
    {
        public Transform transform { get; }
 
        public string text { get; set; }
    }
}