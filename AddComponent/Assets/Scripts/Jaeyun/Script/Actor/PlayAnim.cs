using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace UnityTemplateProjects.Jaeyun.Script.Actor
{
    public class PlayAnim : MonoBehaviour
    {
        private PlayableGraph _playableGraph;
        
        private Animator m_Animator;

        // Start is called before the first frame update

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        public void PlayAnimationClip(AnimationClip clip)
        {
            _playableGraph = PlayableGraph.Create();

            m_Animator.speed = 0;

            var playableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation", m_Animator);

            // Wrap the clip in a playable

            var clipPlayable = AnimationClipPlayable.Create(_playableGraph, clip);

            // Connect the Playable to an output

            playableOutput.SetSourcePlayable(clipPlayable);
        
            _playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            // Plays the Graph.

            _playableGraph.Play();
        }

        private void OnDisable()
        {
            if (_playableGraph.IsValid())
            {
                _playableGraph.Destroy();
                m_Animator.speed = 1;
            }
        }
    }
}