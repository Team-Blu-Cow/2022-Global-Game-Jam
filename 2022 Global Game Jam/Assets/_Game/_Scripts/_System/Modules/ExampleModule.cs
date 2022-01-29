using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace blu
{
    public class ExampleModule : Module
    {
        //Call Order:
        // Awake > Enable > Init > Start
        protected override void Awake()
        {
            Debug.Log("Awake!");
            base.Awake();
        }

        private void OnEnable()
        {
            Debug.Log("Enable!");
        }

        public override void Initialize() //
        {
            base.Initialize();
            Debug.Log("Init!");
        }

        public void Start()
        {
            Debug.Log("Start!");

            //Talking to the application class
            blu.App.GetModule<ExampleModule>().Boop();
        }

        public void Boop()
        {
            Debug.Log("Function Called From App!");
        }
    }
}