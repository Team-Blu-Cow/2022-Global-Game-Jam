using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace blu
{
    public class IOModule : Module
    {
        private const string filename = "savedata.blu";
        private const string listKey = "LevelsComplete";
        private Dictionary<int, bool> levelsComplete;

        protected override void Awake()
        {
            base.Awake();
            ReadDataFromDisk();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public bool IsLevelCompleted(int level)
        {
            if (levelsComplete.ContainsKey(level))
            {
                return levelsComplete[level];
            }

            return false;
        }

        public void SetLevelComplete(string sceneName, bool complete)
        {
            int space = sceneName.LastIndexOf(' ');
            string name = sceneName.Substring(space + 1);

            SetLevelComplete(Int32.Parse(name), complete);
        }

        public void SetLevelComplete(int level, bool complete)
        {
            if (levelsComplete.ContainsKey(level))
            {
                levelsComplete[level] = complete;
            }
            else
            {
                levelsComplete.Add(level, complete);
            }

            WriteDataToDisk();
        }

        private void ReadDataFromDisk()
        {
            // @Matthew: don't think this actually does anything
            // it returns a dictionary of <int, bool> and nothing is using it?
            // The final parameter is a default value if ES3 cant find anything
            // but you're sending in the dictionary you want to assign to I think
            // also having a function called read/write is a little ambiguous
            // when this module also deals with loading/saving settings from disk
            //
            // for now imma just fire the settings data to player prefs but ideally
            // it should be in here

            // @Adam: apparently tired me cant fucking read docs,

            levelsComplete = ES3.Load(listKey, filename, new Dictionary<int, bool>());
        }

        private void WriteDataToDisk()
        {
            ES3.Save(listKey, levelsComplete, filename);
        }
    }
}