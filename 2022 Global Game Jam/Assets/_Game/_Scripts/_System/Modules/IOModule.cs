using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace blu
{
    public class IOModule : Module
    {

        const string filename = "savedata.blu";
        const string listKey = "LevelsComplete";
        Dictionary<int, bool> levelsComplete;

        protected override void Awake()
        {
            base.Awake();
            Read();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public bool IsLevelCompleted(int level)
        {
            if(levelsComplete.ContainsKey(level))
            {
                return levelsComplete[level];
            }

            return false;
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

            Write();
        }

        private void Read()
        {
            ES3.Load(listKey, filename, levelsComplete);
        }

        private void Write()
        {
            ES3.Save(listKey, levelsComplete, filename);
        }

    }
}