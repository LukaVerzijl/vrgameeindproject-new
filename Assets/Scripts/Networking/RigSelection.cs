using UnityEngine;
using UnityEngine.Events;


namespace Fusion.XR.Shared.Desktop
{
    /**
     * 
     * Script to display an overlay UI to select desktop or VR mode, and active the associated rig, alongside the connexion component
     * 
     **/

    public interface IRigSelection
    {
        public UnityEvent OnSelectRig { get; }
        public bool IsRigSelected { get; }
        public bool IsVRRigSelected { get; }

    }


    public class RigSelection : MonoBehaviour, IRigSelection
    {
        public UnityEvent onSelectRig;
        public UnityEvent OnSelectRig => onSelectRig;
        public bool IsRigSelected => rigSelected;
        public bool IsVRRigSelected => vrRig ;

        public const string RIGMODE_VR = "VR";
        public const string RIGMODE_DESKTOP = "Desktop";
        public const string SETTING_RIGMODE = "RigMode";

        public GameObject connexionHandler;
        
        public GameObject vrRig;
        public GameObject desktopRig;

        Camera rigSelectionCamera;

        public bool forceVROnAndroid = true;

        public bool rigSelected = false;


        public bool isVRSelected = false;

        public bool isPCSelected = false;
        public GameObject cam3;
        public PlayerSpawner PlayerSpawner;



        private void Awake()
        {
            rigSelectionCamera = GetComponentInChildren<Camera>();
            if (connexionHandler)
            {
                connexionHandler.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("No connexion handler provided to RigSelection: risk of connection before choosing the appropriate hardware rig !");
            }

            PlayerSpawner = connexionHandler.GetComponent<PlayerSpawner>();

        }



        protected virtual void OnGUI()
        {
            GUILayout.BeginArea(new Rect(5, 5, Screen.width - 10, Screen.height - 10));
            {
                GUILayout.BeginVertical(GUI.skin.window);
                {

                    if (GUILayout.Button("VR"))
                    {
                        EnableVRRig();
                    }
                    if (GUILayout.Button("Desktop"))
                    {
                        EnableDesktopRig();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }

        void EnableVRRig()
        {
            this.enabled = false;
            PlayerSpawner.isVRSelected = true;
            Debug.Log(PlayerSpawner.isVRSelected);

            OnRigSelected();

       
        }

            void EnableDesktopRig()
            {
                this.enabled= false;
                PlayerSpawner.isPCSelected = true;
            Debug.Log(PlayerSpawner.isPCSelected);  

                OnRigSelected();
            }

        void OnRigSelected()
        {
            if (connexionHandler && connexionHandler.gameObject.activeSelf == false)
            {
                connexionHandler.gameObject.SetActive(true);
                var runner = connexionHandler.GetComponent<NetworkRunner>();
                if (runner)
                {
                    // As the runner was disabled, the runner may not have auto registered its listeners
                    foreach (var listener in runner.GetComponentsInChildren<INetworkRunnerCallbacks>())
                    {
                        runner.AddCallbacks(listener);
                    }
                }
            }

            if (OnSelectRig != null) OnSelectRig.Invoke();
            if (rigSelectionCamera) rigSelectionCamera.gameObject.SetActive(false);
            rigSelected = true;
        }

    }

}