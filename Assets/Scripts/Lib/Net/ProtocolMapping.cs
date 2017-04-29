using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Net;
using Assets.Scripts.Proto;

namespace Assets.Scripts.Lib.Net
{

    public class ProcessInfo
    {
        public int protocolId;
        public IList<ProtocolHandler> handlerList = new List<ProtocolHandler>();
        public Type type;

        public ProcessInfo(int _protocolId)
        {
            protocolId = _protocolId;
        }
    }

    class ProtocolMapping
    {
        private Dictionary<int, ProcessInfo> processInfoDict;

        public ProtocolMapping()
        {
            processInfoDict = new Dictionary<int, ProcessInfo>();
        }

        public void AddProtocolHandler(int protocolId, ProtocolHandler handler, Type type)
        {
            ProcessInfo info;
            processInfoDict.TryGetValue(protocolId, out info);
            if (info == null)
            {
                info = new ProcessInfo(protocolId);
                processInfoDict.Add(protocolId, info);
            }
            info.type = type;
            if (handler != null && !info.handlerList.Contains(handler))
            {
                info.handlerList.Add(handler);
            }
        }

        public void AddProcessInfo(ProcessInfo info)
        {
            processInfoDict.Add((int)info.protocolId, info);
        }

        public ProcessInfo GetProcessInfo(int protocolId)
        {
            ProcessInfo info;
            processInfoDict.TryGetValue(protocolId,out info);
            if (info == null)
            {
                info = new ProcessInfo(protocolId);
                processInfoDict.Add(protocolId, info);
            }
            return info;
        }

    }
}
