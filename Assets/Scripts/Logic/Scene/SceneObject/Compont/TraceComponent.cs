using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Data;

namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
    class TraceComponent : BaseComponent
    {
        private GameObject PointGameObject;
        //线段对象
        private GameObject LineRenderGameObject;

        //线段渲染器  
        private LineRenderer lineRenderer;

        public override string GetName()
        {
            return GetType().Name;
        }

        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);
            //获得线段游戏对象
            LineRenderGameObject = new GameObject();
            LineRenderGameObject.AddComponent("LineRenderer");
            LineRenderGameObject.AddComponent("Material");
            //获得线渲染器组件
            lineRenderer = (LineRenderer)LineRenderGameObject.GetComponent("LineRenderer");
            //设置线的顶点数
            lineRenderer.SetVertexCount(2);
            //设置线的宽度
            lineRenderer.SetWidth(0.1f, 0.1f);

            PointGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            GameObject.DestroyObject(PointGameObject);
            GameObject.DestroyObject(LineRenderGameObject);
            base.OnDetachFromEntity(ety);
        }

        public override void DoUpdate()
        {
            lineRenderer.SetPosition(1, Owner.Position);
            if (Owner.property.destination.x != 0)
            {
                lineRenderer.SetPosition(0, Owner.Position);
                lineRenderer.SetPosition(1, Owner.property.destination);
            }

            PointGameObject.transform.position = Owner.property.ServerPos;
            PointGameObject.transform.position.Set(PointGameObject.transform.position.x, PointGameObject.transform.position.y + 0.5f, PointGameObject.transform.position.z);

        }
    }
}
