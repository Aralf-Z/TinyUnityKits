/*

*/

using UnityEngine;

namespace ZToolKit.Editor
{
    public abstract class PanelBase
    {
        public virtual int Priority => 1000;
        public virtual string PanelName => GetType().Name;
        public abstract void Init();
        public abstract void DrawPanel(Rect windowRect);
    }
}
