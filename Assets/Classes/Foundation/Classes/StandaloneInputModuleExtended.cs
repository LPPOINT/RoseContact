using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.EventSystems;

namespace Assets.Classes.Foundation.Classes
{
    class StandaloneInputModuleExtended : StandaloneInputModule
    {
        public PointerEventData GetPointerEventData(int id)
        {
            return GetLastPointerEventData(id);
        }
    }
}
