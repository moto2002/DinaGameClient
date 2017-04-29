using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Proto;
using Assets.Scripts.Lib.Net;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Reflection;
using Assets.Scripts.Manager;

namespace Assets.Scripts.Logic.RemoteCall
{
    class RemoteCall : BaseLogic
    {
        public enum KLuaValueDef
        {
            eLuaPackNill,
            eLuaPackNumber,
            eLuaPackBoolean,
            eLuaPackString,
            eLuaPackTable,
        };

        protected override void InitListeners()
        {
            RegistSocketListener(KS2C_Protocol.s2c_call_script, OnCallScript, typeof(S2C_CALL_SCRIPT));
        }

        private void OnCallScript(KProtoBuf buf)
        {
            S2C_CALL_SCRIPT respond = buf as S2C_CALL_SCRIPT;

            ArrayList array = UnpackAll(respond.data);
            string strFunc = array[0] as string;

            Type type = typeof(RemoteCallLogic);
            MethodInfo methodinfo = type.GetMethod(strFunc, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			
			if ( ConfigManager.GetInstance().DebugMode )
			{
				ParameterInfo[] paramsInfo = methodinfo.GetParameters();
                object[] parameters = new object[array.Count - 1];

                for (int i = 1; i < array.Count; i++)
                {
                    if (paramsInfo[i - 1].ParameterType == typeof(ulong))
                    {
                        string uid = array[i] as string;
                        parameters[i - 1] = ulong.Parse(uid);
                    }
                    else
                    {
                        parameters[i - 1] = array[i];
                    }
                }

                if (methodinfo != null)
                    methodinfo.Invoke(this, parameters);
			}
			else
			{
				try
	            {
	                ParameterInfo[] paramsInfo = methodinfo.GetParameters();
	                object[] parameters = new object[array.Count - 1];
	
	                for (int i = 1; i < array.Count; i++)
	                {
	                    if (paramsInfo[i - 1].ParameterType == typeof(ulong))
	                    {
	                        string uid = array[i] as string;
	                        parameters[i - 1] = ulong.Parse(uid);
	                    }
	                    else
	                    {
	                        parameters[i - 1] = array[i];
	                    }
	                }
	
	                if (methodinfo != null)
	                    methodinfo.Invoke(this, parameters);
	            }
	            catch (System.Exception ex)
	            {
	                Debug.LogError("远程调用 " + strFunc + " 异常" + ex.ToString());
	            }

				
			}
           
        }

        private ArrayList UnpackAll(MemoryStream stream)
        {
            ArrayList retArray = new ArrayList();
            
            while(stream.Position != stream.Length)
            {
                UnPackUp(stream, retArray);
            }

            return retArray;
        }

        private KLuaValueDef UnPackUp(MemoryStream stream, ArrayList retArray)
        {
            uint size = 0;
            KLuaValueDef eRet = (KLuaValueDef)stream.ReadByte();
            switch (eRet)
            {
                case KLuaValueDef.eLuaPackNumber:
                    size = UnPackupNumber(stream, retArray);
                    break;
                case KLuaValueDef.eLuaPackBoolean:
                    size = UnPackupBoolean(stream, retArray);
                    break;
                case KLuaValueDef.eLuaPackString:
                    size = UnPackupString(stream, retArray);
                    break;
                case KLuaValueDef.eLuaPackNill:
                    size = UnPackNull(stream, retArray);
                    break;
                case KLuaValueDef.eLuaPackTable:
                    size = UnPackTable(stream, retArray);
                    break;
            }
            size += sizeof(byte);
            return eRet;
        }

        private uint UnPackupBoolean(MemoryStream stream, ArrayList retArray)
        {
            BinaryReader read = new BinaryReader(stream);
            bool v = read.ReadBoolean();
            retArray.Add(v);
            return sizeof(bool);
        }

        private uint UnPackupNumber(MemoryStream stream, ArrayList retArray)
        {
            BinaryReader read = new BinaryReader(stream);
            int v = read.ReadInt32();
            retArray.Add(v);
            return sizeof(int);
        }

        private uint UnPackupString(MemoryStream stream, ArrayList retArray)
        {
            BinaryReader read = new BinaryReader(stream);
            long startPos = stream.Position;
            int length = 0;

            byte curByte = read.ReadByte();

            length++;
            while (curByte != 0)
            {
                curByte = read.ReadByte();
                length++;
            }

            stream.Position = startPos;
            byte[] b = read.ReadBytes(length - 1);
            read.ReadByte();

            string v = Encoding.ASCII.GetString(b, 0, length - 1);

            retArray.Add(v);
            return (uint)length;
        }

        private uint UnPackNull(MemoryStream stream, ArrayList retArray)
        {
            retArray.Add(null);
            return 0;
        }

        private uint UnPackTable(MemoryStream stream, ArrayList retArray)
        {
            BinaryReader read = new BinaryReader(stream);
            uint tabSize = read.ReadUInt32();
            uint tabEnd = (uint)(stream.Position) + tabSize;
            RemoteTable luaTab = new RemoteTable();

            while (stream.Position < tabEnd)
            {
                ArrayList tabKV = new ArrayList();

                KLuaValueDef keyType = UnPackUp(stream, tabKV);
                KLuaValueDef valueType = UnPackUp(stream, tabKV);

                if (keyType == KLuaValueDef.eLuaPackNumber)
                {
                    int index = (int)tabKV[0];
                    switch (valueType)
                    {
                        case KLuaValueDef.eLuaPackNumber:
                            luaTab[index] = (RemoteInt)((int)tabKV[1]);
                            break;
                        case KLuaValueDef.eLuaPackBoolean:
                            luaTab[index] = (RemoteBool)((bool)tabKV[1]);
                            break;
                        case KLuaValueDef.eLuaPackString:
                            luaTab[index] = (RemoteString)(tabKV[1] as string);
                            break;
                        case KLuaValueDef.eLuaPackTable:
                            luaTab[index] = tabKV[1] as RemoteTable;
                            break;
                        default:
                            log.Error("UnKnow ValueType");
                            break;
                    }
                }
                else
                {
                    string name = tabKV[0] as string;
                    switch (valueType)
                    {
                        case KLuaValueDef.eLuaPackNumber:
                            luaTab[name] = (RemoteInt)((int)tabKV[1]);
                            break;
                        case KLuaValueDef.eLuaPackBoolean:
                            luaTab[name] = (RemoteBool)((bool)tabKV[1]);
                            break;
                        case KLuaValueDef.eLuaPackString:
                            luaTab[name] = (RemoteString)(tabKV[1] as string);
                            break;
                        case KLuaValueDef.eLuaPackTable:
                            luaTab[name] = tabKV[1] as RemoteTable;
                            break;
                        default:
                            log.Error("UnKnow ValueType");
                            break;
                    }
                }
            }
            retArray.Add(luaTab);
            return tabSize;
        }

        private void WriteParam(BinaryWriter write, string strFunc, params object[] values)
        {
            var str = strFunc.ToCharArray();
            write.Write((byte)(KLuaValueDef.eLuaPackString));
            write.Write(str);
            write.Write((byte)0);

            foreach (object o in values)
            {
                if (o == null)
                {
                    write.Write((byte)(KLuaValueDef.eLuaPackNill));
                    continue;
                }

                Type type = o.GetType();
                if (type == typeof(int) || type == typeof(uint))
                {
                    write.Write((byte)(KLuaValueDef.eLuaPackNumber));
                    write.Write((int)o);
                }
                else if (type == typeof(bool))
                {
                    write.Write((byte)(KLuaValueDef.eLuaPackBoolean));
                    write.Write((bool)o);
                }
                else if (type == typeof(string))
                {
                    str = (o as string).ToCharArray();
                    write.Write((byte)(KLuaValueDef.eLuaPackString));
                    write.Write(str);
                    write.Write((byte)0);
                }
                else
                {
                    log.Error("Call Script Has UnSuport Type");
                }
            }
        }
        public void CallGS(string strFunc, params object[] values)
        {
            C2S_CALL_GS msg = new C2S_CALL_GS();
            BinaryWriter write = new BinaryWriter(msg.data);

            msg.protocolID = (byte)KC2S_Protocol.c2s_call_gs;

            WriteParam(write, strFunc, values);

            SendMessage(msg);
        }

        public void CallLS(string strFunc, params object[] values)
        {
            C2S_CALL_LS msg = new C2S_CALL_LS();
            BinaryWriter write = new BinaryWriter(msg.data);

            msg.protocolID = (byte)KC2S_Protocol.c2s_call_ls;

            WriteParam(write, strFunc, values);

            SendMessage(msg);
        }

        public void CallPlayer(ulong uPlayerID, string strFunc, params object[] values)
        {
            C2S_CALL_PLAYER msg = new C2S_CALL_PLAYER();
            BinaryWriter write = new BinaryWriter(msg.data);

            msg.uPlayerID = uPlayerID;
            msg.protocolID = (byte)KC2S_Protocol.c2s_call_player;

            WriteParam(write, strFunc, values);

            SendMessage(msg);
        }
    }
}
