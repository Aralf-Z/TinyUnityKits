/*

*/

using UnityEngine;

namespace ZToolKit.Editor
{
    public abstract class PanelBase
    {
        public abstract int Priority { get; }
        public abstract string PanelName  { get; }
        public abstract void Init();
        public abstract void DrawPanel(Rect windowRect);
    }
}
