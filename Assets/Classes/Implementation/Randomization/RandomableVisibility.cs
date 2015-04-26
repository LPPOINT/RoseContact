using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Classes.Foundation.Classes;

namespace Assets.Classes.Implementation.Randomization
{
    public class RandomableVisibility : ChunkRandomable
    {
        public override void Randomize()
        {
            gameObject.SetActive(RandomableBoolUtils.RandomBool());
            base.Randomize();
        }
    }
}
