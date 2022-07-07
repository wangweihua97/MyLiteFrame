using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Examples.ProgressBar
{
    public class SelectScene : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SelectSceneA()
        {
            ProgressBarGlobal.ProgressBarSceneMgr.StartScene("ProgressBarSceneA" ,ProgressBarLoadingViewMgr.ProgressBarLoadingView);
        }
        
        public void SelectSceneB()
        {
            ProgressBarGlobal.ProgressBarSceneMgr.StartScene("ProgressBarSceneB" ,ProgressBarLoadingViewMgr.ProgressBarLoadingView);
        }
    }
}
