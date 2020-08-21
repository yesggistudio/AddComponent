using UnityEngine;

namespace UnityTemplateProjects.Jaeyun.Script.Actor
{
    public class Actor : MonoBehaviour
    {
        private Material _myMat;

        private Shader _defaultShader;
        private Shader _OutlineShader;
        
        private void Awake()
        {
            _myMat = GetComponent<SpriteRenderer>().material;
            _defaultShader = Shader.Find("Custom/2D Sprite");
            _OutlineShader = Shader.Find("Shader Graphs/2D DrawOutline");
        }

        public void DrawNormal()
        {
            _myMat.shader = _defaultShader;
        }
        
        public void DrawOutline()
        {
            _myMat.shader = _OutlineShader;
        }
        
    }
}