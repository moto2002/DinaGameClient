using UnityEngine;
using Assets.Scripts.Logic.Item;

namespace Assets.Scripts.View.Drag
{
    public interface IDragable
    {
        string DragType { get; set; }

        UISprite DragIcon { get; set; }

        IDragInfo DragItemVO { get; set; }

        void OnDrag(Vector2 delta);

        void OnDrop(GameObject go);
    }
}
