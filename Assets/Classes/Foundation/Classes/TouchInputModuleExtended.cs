using UnityEngine.EventSystems;

namespace Assets.Classes.Foundation.Classes
{
    public class TouchInputModuleExtended : TouchInputModule
    {
        public PointerEventData GetPointerEventData(int id)
        {
            return GetLastPointerEventData(id);
        }
    }
}
