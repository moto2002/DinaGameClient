using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts.Model.Application
{
    public class FPS
    {
        private int m_lastTime = 0;
        private int m_frameCount = 0;

        private int m_fps = 0;
        public int Fps
        {
            get
            {
                return m_fps;
            }
        }

        public void Update()
        {
            int currentTime = Environment.TickCount;
            if (m_lastTime == 0 || currentTime - m_lastTime > 1000)
            {
                m_fps = m_frameCount * 1000 / (currentTime - m_lastTime);
                m_frameCount = 0;
                m_lastTime = currentTime;
            }
            else
                ++m_frameCount;
        }
    }
}