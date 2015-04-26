using System;
using System.Collections;

namespace Assets.Plugins.Pixelplacement.iTween
{
    public class TweenArgs
    {

        public Action OnStart { get; set; }
        public Action OnComplete { get; set; }
        public Action<object> OnUpdate { get; set; }

        public global::iTween.EaseType EaseType { get; set; }
        public global::iTween.LoopType LoopType { get; set; }



        public Hashtable CustomDataTable { get; set; }


        public Hashtable ToHashtable()
        {
            var hashtable = new Hashtable();

            return hashtable;
        }
    }
}
