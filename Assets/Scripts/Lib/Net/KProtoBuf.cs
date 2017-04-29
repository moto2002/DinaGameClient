using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Assets.Scripts;
using Assets.Scripts.Lib.Log;
using System.Runtime.InteropServices;
using Assets.Scripts.Proto;

namespace Assets.Scripts.Lib.Net
{
    public delegate void ProtocolHandler(KProtoBuf ar);

    class KSign
    {
	    public string name = null;
	    public string type = null;
	    public int bytes = 0;
	    public int count = 1;
    }

	public class KProtoBuf
	{
        private static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(KProtoBuf));
        private IList<KSign> signs = new List<KSign>();

        public KProtoBuf()
        {
        }


        public event ProtocolHandler OnProtocolHandler;

        public void ClearSigns()
		{
			signs.Clear();
			signs = null;
		}

        public void AddHandler(ProcessInfo info)
        {
            foreach(ProtocolHandler handler in info.handlerList)
            {
                OnProtocolHandler += handler;
            }
        }

        public void RemoveHandler(ProcessInfo info)
        {
            foreach(ProtocolHandler handler in info.handlerList)
            {
                OnProtocolHandler -= handler;
            }
        }

        public void DispatchDelegate()
        {
            if(OnProtocolHandler != null)
            {
                OnProtocolHandler(this);
            }
        }

        public void Register(string name, string type, int bytes , int count)
		{
			KSign sign = new KSign();
			sign.name = name;
			sign.type = type;
			sign.bytes = bytes;
			sign.count = count;
			signs.Add(sign);
            
		}

        public string ToString()
		{
			string text = "class " + this.GetType().Name + " [";
			for (int i = 0; i < signs.Count; i++)
			{
				KSign sign = signs[i];
				text += sign.name + ":" + sign.type + sign.bytes + "(" + sign.count + ")";
				text += " = "  + "; ";
			}
            text += "]";

			return text;
		}

        
		private bool IsList(Object value)
		{
			string className = value.GetType().Name;
			return className.IndexOf("List") != -1;
		}
		
        private void SetPropertyValue(string name, object value)
        {
            try
            {
                FieldInfo fieldInfo = this.GetType().GetField(name);
                fieldInfo.SetValue(this, value);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
            }
        }

        private Object GetPropertyValue(string name)
        {
            FieldInfo fieldInfo = this.GetType().GetField(name);
            return fieldInfo.GetValue(this);
        }

        private int GetListPropertyCount(string name)
        {
            FieldInfo fieldInfo = this.GetType().GetField(name);
            object obj = fieldInfo.GetValue(this);
            PropertyInfo propertyInfo = obj.GetType().GetProperty("Count");
            return (int)propertyInfo.GetValue(obj, null);
        }

        public void pack(BinaryWriter writer)
		{			
			KSign sign = null;
			int j = 0;
			for (int i = 0; i < signs.Count; i++)  
			{
				sign = signs[i];            //取得一个属性
				int count = sign.count;
				if (count == 0)             //属性签名是0的时候是List
				{
					count = GetListPropertyCount(sign.name);
					writer.Write((short)count);     //写一个list的长度
				}

				Object value = GetPropertyValue(sign.name);     //获得这个属性所代表的值对象
				log.Assert(count == 1 || IsList(value), "If count != 1 then value is a List!");
				switch (sign.type)
				{
					case "System.Boolean":		
					{
						if (sign.count == 1)
						{
							writer.Write((bool)value);
						}
                        else if (sign.count == 0)
                        {
                            for (j = 0; j < count; j++)
                            {
                                writer.Write(((List<bool>)value)[j]);
                            }
                        }
                        else
                        {
                            for (j = 0; j < count; j++)
                            {
                                writer.Write(((bool[])value)[j]);
                            }
                        }
						break;
					}
					case "System.Int32":
					{
						if (sign.count == 1)
						{
						    switch(sign.bytes)
						    {
							    case 1:
                                    writer.Write((sbyte)value);
								    break;
							    case 2:
								    writer.Write((short)value);
								    break;
							    case 4:
								    writer.Write((int)value);
								    break;
                                case 8:
                                    writer.Write((long)value);
                                    break;
							    default:
								    log.Assert(false, "");
								    break;
						    }
						}
                        else if (sign.count == 0)
                        {
                            switch (sign.bytes)
                            {
                                case 1:
                                    List<sbyte> sbyteList = (List<sbyte>)value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write((sbyte)(sbyteList[j]));
                                    }
                                    break;
                                case 2:
                                    List<short> shortList = (List<short>)value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write((short)(shortList[j]));
                                    }
                                    break;
                                case 4:
                                    List<int> intList = (List<int>)value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write((int)(intList[j]));
                                    }
                                    break;
                                case 8:
                                    List<long> longList = (List<long>)value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write((long)(longList[j]));
                                    }
                                    break;
                                default:
                                    log.Assert(false, "");
                                    break;
                            }
                        }
                        else
                        {
                            switch (sign.bytes)
                            {
                                case 1:
                                    sbyte[] sbyteList = (sbyte[])value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write((sbyte)(sbyteList[j]));
                                    }
                                    break;
                                case 2:
                                    short[] shortList = (short[])value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write((short)(shortList[j]));
                                    }
                                    break;
                                case 4:
                                    int[] intList = (int[])value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write((int)(intList[j]));
                                    }
                                    break;
                                case 8:
                                    long[] longList = (long[])value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write((long)(longList[j]));
                                    }
                                    break;
                                default:
                                    log.Assert(false, "");
                                    break;
                            }
                        }
						break;
					}
                    case "System.UInt32":
                    {
                        if (sign.count == 1)
                        {
                            switch (sign.bytes)
                            {
                                case 1:
                                    writer.Write((byte)value);
                                    break;
                                case 2:
                                    writer.Write((ushort)value);
                                    break;
                                case 4:
                                    writer.Write((uint)value);
                                    break;
                                case 8:
                                    writer.Write((ulong)value);
                                    break;
                                default:
                                    log.Assert(false, "");
                                    break;
                            }
                        }
                        else if(sign.count == 0)
                        {
                            switch (sign.bytes)
                            {
                                case 1:
                                    List<byte> byteList = (List<byte>)value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write(byteList[j]);
                                    }
                                    break;
                                case 2:
                                    List<ushort> ushortList = (List<ushort>)value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write(ushortList[j]);
                                    }
                                    break;
                                case 4:
                                    List<uint> uintList = (List<uint>)value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write(uintList[j]);
                                    }
                                    break;
                                case 8:
                                    List<ulong> ulongList = (List<ulong>)value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write(ulongList[j]);
                                    }
                                    break;
                                default:
                                    log.Assert(false, "");
                                    break;
                            }
                        }
                        else
                        {
                            switch (sign.bytes)
                            {
                                case 1:
                                    byte[] byteList = (byte[])value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write(byteList[j]);
                                    }
                                    break;
                                case 2:
                                    ushort[] ushortList = (ushort[])value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write(ushortList[j]);
                                    }
                                    break;
                                case 4:
                                    uint[] uintList = (uint[])value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write(uintList[j]);
                                    }
                                    break;
                                case 8:
                                    ulong[] ulongList = (ulong[])value;
                                    for (j = 0; j < count; j++)
                                    {
                                        writer.Write(ulongList[j]);
                                    }
                                    break;
                                default:
                                    log.Assert(false, "");
                                    break;
                            }
                        }
                        break;
                    }
					case "System.Single":
                    {
                        if (sign.bytes != 4)
                        {
                            log.Assert(false, "");
                        }
                        if (sign.count == 1)
                        {
                            writer.Write((float)value);
                        }
                        else if(sign.count == 0)
                        {
                            List<float> floatList = (List<float>)value;
                            for (j = 0; j < count; j++)
                            {
                                writer.Write(floatList[j]);
                            }
                        }
                        else
                        {
                            float[] floatList = (float[])value;
                            for (j = 0; j < count; j++)
                            {
                                writer.Write(floatList[j]);
                            }
                        }
                        break;
                    }
					case "System.Double":
					{
                        if (sign.bytes != 8)
                        {
                            log.Assert(false, "");
                        }
						if (sign.count == 1)
						{
                            writer.Write((double)value);
						}
						else
						{
                            List<double> doubleList = (List<double>)value;
                            for (j = 0; j < count; j++)
                            {
                                writer.Write(doubleList[j]);
                            }
						}
						break;
					}
					case "System.String":
					{
						if (sign.count == 1)
						{
                            WriteString(writer,(string)value, sign.bytes);
						}
						else if(sign.count == 0)
						{
                            List<string> stringList = (List<string>)value;
							for (j = 0; j < count; j++)
							{
                                WriteString(writer, stringList[j], sign.bytes);
							}	
						}
                        else
						{
                            string[] stringList = (string[])value;
							for (j = 0; j < count; j++)
							{
                                WriteString(writer, stringList[j], sign.bytes);
							}	
						}
						break;
					}
                    case "System.IO.MemoryStream":
                    {
                        if (sign.count == 1)
                        {
                            if (sign.bytes == 0)
                            {
                                WriteMemoryStream(writer, (MemoryStream)value);
                            }
                            else
                            {
                                WriteMemoryStream(writer, (MemoryStream)value, sign.bytes);
                            }
                        }
                        else if(sign.count == 0)
                        {
                            List<MemoryStream> steamList = (List<MemoryStream>)value;
                            for (j = 0; j < count; j++)
                            {
                                if (sign.bytes == 0)
                                {
                                    WriteMemoryStream(writer, steamList[j]);
                                }
                                else
                                {
                                    WriteMemoryStream(writer, steamList[j], sign.bytes);
                                }
                            }
                        }
                        else
                        {
                            MemoryStream[] steamList = (MemoryStream[])value;
                            for (j = 0; j < count; j++)
                            {
                                if (sign.bytes == 0)
                                {
                                    WriteMemoryStream(writer, steamList[j]);
                                }
                                else
                                {
                                    WriteMemoryStream(writer, steamList[j], sign.bytes);
                                }
                            }
                        }

                        break;
                    }
					default:
					{
                        Type type = Type.GetType(sign.type);
						log.Assert(sign.bytes == 0);
						
						if (sign.count == 1)
						{
                            (value as KProtoBuf).pack(writer);
						}
						else if(sign.count == 0)
						{
                            log.Assert(type != null, "undefine objClass: ");
                           
                            MethodInfo methodinfo = type.GetMethod("pack");
                            FieldInfo fieldInfo = this.GetType().GetField(sign.name);
                            object obj = fieldInfo.GetValue(this);
                            PropertyInfo propertyInfo = obj.GetType().GetProperty("Item");
                            
                            for (j = 0; j < count; j++)
							{
                                object[] parameters = new object[1];
                                parameters[0] = writer;
                                object[] indexArgs = { j };
                                object objIndex = propertyInfo.GetValue(obj, indexArgs);
                                methodinfo.Invoke(objIndex, parameters);
							}
						}
                        else
                        {
                            log.Assert(type != null, "undefine objClass: ");
                            Assembly _Assembly = Assembly.Load("mscorlib");
                            Type _TypeArray = Type.GetType(type.FullName + "[]");
                            MethodInfo methodinfo = _TypeArray.GetMethod("GetValue", new Type[] { typeof(int) });
                            MethodInfo packmethod = type.GetMethod("pack");
                            for (j = 0; j < count; j++)
                            {
                                object[] parameters = new object[1];
                                parameters[0] = j;
                                object ret = methodinfo.Invoke(value, parameters);
                                parameters[0] = writer;
                                packmethod.Invoke(ret, parameters);
                            }
                        }
						break;
					}
				}
			}
		}
		

        public void unpack(BinaryReader buffer)
		{
			KSign sign = null;
			int j = 0;
            object reader = 0;
			for (int i = 0; i < signs.Count; i++)
			{
				sign = signs[i];
				int count = sign.count;
				if (count == 0)//0表示是变长
				{
					count = buffer.ReadInt16();
				}
				
				switch (sign.type)
				{
					case "System.Boolean": //bool  1个字节
					{
						if (sign.count == 1)
						{
                            SetPropertyValue(sign.name, buffer.ReadBoolean());
						}
						else if(sign.count == 0)
						{
                            List<bool> list = new List<bool>();
							for (j = 0; j < count; j++)
							{
								list.Add(buffer.ReadBoolean());
							}
                            SetPropertyValue(sign.name, list);
						}
                        else
                        {
                            bool[] list = new bool[count];
                            for (j = 0; j < count; j++)
                            {
                                list[j] = buffer.ReadBoolean();
                            }
                            SetPropertyValue(sign.name, list);
                        }
						break;
					}
					case "System.Int32":
					{	
						if (sign.count == 1)
						{
                            switch(sign.bytes)
						    {
							    case 1:
								    reader = buffer.ReadSByte();
								    break;
							    case 2:
								    reader = buffer.ReadInt16();
								    break;
							    case 4:
								    reader = buffer.ReadInt32();
								    break;
                                case 8:
                                    reader = buffer.ReadInt64();
                                    break;
							    default:
								    log.Assert(false);
								    break;
						    }
                            SetPropertyValue(sign.name, reader);
						}
						else if(sign.count == 0)
						{
                            
							switch(sign.bytes)
						    {
							    case 1:
                                    List<sbyte> sbyteList = new List<sbyte>();
							        for (j = 0; j < count; j++)
							        {
								        sbyteList.Add(buffer.ReadSByte());
                                    }
                                        SetPropertyValue(sign.name, sbyteList);
								    break;
							    case 2:
								    List<short> shortList = new List<short>();
							        for (j = 0; j < count; j++)
							        {
								        shortList.Add(buffer.ReadInt16());
                                    }
                                        SetPropertyValue(sign.name, shortList);
								    break;
							    case 4:
								    List<int> intList = new List<int>();
							        for (j = 0; j < count; j++)
							        {
								        intList.Add(buffer.ReadInt32());
                                    }
                                        SetPropertyValue(sign.name, intList);
								    break;
                                case 8:
								    List<long> longList = new List<long>();
							        for (j = 0; j < count; j++)
							        {
								        longList.Add(buffer.ReadInt64());
                                    }
                                        SetPropertyValue(sign.name, longList);
								    break;
							    default:
								    log.Assert(false);
								    break;
						    }
						}
                        else
                        {

                            switch (sign.bytes)
                            {
                                case 1:
                                    sbyte[] sbyteList = new sbyte[count];
                                    for (j = 0; j < count; j++)
                                    {
                                        sbyteList[j] = buffer.ReadSByte();
                                    }
                                    SetPropertyValue(sign.name, sbyteList);
                                    break;
                                case 2:
                                    short[] shortList = new short[count];
                                    for (j = 0; j < count; j++)
                                    {
                                        shortList[j] = buffer.ReadInt16();
                                    }
                                    SetPropertyValue(sign.name, shortList);
                                    break;
                                case 4:
                                    int[] intList = new int[count];
                                    for (j = 0; j < count; j++)
                                    {
                                        intList[j] = buffer.ReadInt32();
                                    }
                                    SetPropertyValue(sign.name, intList);
                                    break;
                                case 8:
                                    long[] longList = new long[count];
                                    for (j = 0; j < count; j++)
                                    {
                                        longList[j] = buffer.ReadInt64();
                                    }
                                    SetPropertyValue(sign.name, longList);
                                    break;
                                default:
                                    log.Assert(false);
                                    break;
                            }
                        }
						break;
					}
                    case "System.UInt32":
                    {
                        if (sign.count == 1)
                        {
                            switch (sign.bytes)
                            {
                                case 1:
                                    reader = buffer.ReadByte();
                                    break;
                                case 2:
                                    reader = buffer.ReadUInt16();
                                    break;
                                case 4:
                                    reader = buffer.ReadUInt32();
                                    break;
                                case 8:
                                    reader = buffer.ReadUInt64();
                                    break;
                                default:
                                    log.Assert(false);
                                    break;
                            }
                            SetPropertyValue(sign.name, reader);
                        }
                        else if(sign.count == 0)
                        {
                            switch (sign.bytes)
                            {
                                case 1:
                                    List<byte> byteList = new List<byte>();
                                    for (j = 0; j < count; j++)
                                    {
                                        byteList.Add(buffer.ReadByte());
                                    }
                                    SetPropertyValue(sign.name, byteList);
                                    break;
                                case 2:
                                    List<ushort> ushortList = new List<ushort>();
                                    for (j = 0; j < count; j++)
                                    {
                                        ushortList.Add(buffer.ReadUInt16());
                                    }
                                    SetPropertyValue(sign.name, ushortList);
                                    break;
                                case 4:
                                    List<uint> uintList = new List<uint>();
                                    for (j = 0; j < count; j++)
                                    {
                                        uintList.Add(buffer.ReadUInt32());
                                    }
                                    SetPropertyValue(sign.name, uintList);
                                    break;
                                case 8:
                                    List<ulong> ulongList = new List<ulong>();
                                    for (j = 0; j < count; j++)
                                    {
                                        ulongList.Add(buffer.ReadUInt64());
                                    }
                                    SetPropertyValue(sign.name, ulongList);
                                    break;
                                default:
                                    log.Assert(false);
                                    break;
                            }
                        }
                        else
                        {
                            switch (sign.bytes)
                            {
                                case 1:
                                    byte[] byteList = new byte[count];
                                    for (j = 0; j < count; j++)
                                    {
                                        byteList[j] = buffer.ReadByte();
                                    }
                                    SetPropertyValue(sign.name, byteList);
                                    break;
                                case 2:
                                    ushort[] ushortList = new ushort[count];
                                    for (j = 0; j < count; j++)
                                    {
                                        ushortList[j] = buffer.ReadUInt16();
                                    }
                                    SetPropertyValue(sign.name, ushortList);
                                    break;
                                case 4:
                                    uint[] uintList = new uint[count];
                                    for (j = 0; j < count; j++)
                                    {
                                        uintList[j] = buffer.ReadUInt32();
                                    }
                                    SetPropertyValue(sign.name, uintList);
                                    break;
                                case 8:
                                    ulong[] ulongList = new ulong[count];
                                    for (j = 0; j < count; j++)
                                    {
                                        ulongList[j] = buffer.ReadUInt64();
                                    }
                                    SetPropertyValue(sign.name, ulongList);
                                    break;
                                default:
                                    log.Assert(false);
                                    break;
                            }
                        }
                        break;
                    }
					case "System.Double":
                    {
                        if (sign.bytes != 8)
                        {
                            log.Assert(false);
                        }
                        if (sign.count == 1)
                        {
                            reader = buffer.ReadDouble();
                            SetPropertyValue(sign.name, reader);
                        }
                        else if (sign.count == 0)
                        {
                            List<double> doubleList = new List<double>();
                            for (j = 0; j < count; j++)
                            {
                                doubleList.Add(buffer.ReadDouble());
                            }
                            SetPropertyValue(sign.name, doubleList);
                        }
                        else
                        {
                            double[]    doubleList = new double[count];
                            for (j = 0; j < count; j++)
                            {
                                doubleList[j] = buffer.ReadDouble();
                            }
                            SetPropertyValue(sign.name, doubleList);
                        }
                        break;
                    }
                    case "System.Single":
                    {
                        if (sign.bytes != 4)
                        {
                            log.Assert(false);
                        }
                        if (sign.count == 1)
                        {
                            reader = buffer.ReadSingle();
                            SetPropertyValue(sign.name, reader);
                        }
                        else if(sign.count == 0)
                        {
                            List<float> floatList = new List<float>();
                            for (j = 0; j < count; j++)
                            {
                                floatList.Add(buffer.ReadSingle());
                            }
                            SetPropertyValue(sign.name, floatList);
                        }
                        else
                        {
                            float[] floatList = new float[count];
                            for (j = 0; j < count; j++)
                            {
                                floatList[j] = buffer.ReadSingle();
                            }
                            SetPropertyValue(sign.name, floatList);
                        }
                        break;
                    }
					case "System.String":
					{
						if (sign.count == 1)
						{
                             SetPropertyValue(sign.name, ReadString(buffer, sign.bytes));
						}
						else if(sign.count == 0)
						{
                            List<string> list = new List<string>();
							for (j = 0; j < count; j++)
							{
								 list.Add(ReadString(buffer, sign.bytes));
							}
                            SetPropertyValue(sign.name, list);
						}
                        else
                        {
                            string[] list = new string[count];
                            for (j = 0; j < count; j++)
                            {
                                list[j] = ReadString(buffer, sign.bytes);
                            }
                            SetPropertyValue(sign.name, list);
                        }
						break;
					}
                    case "System.IO.MemoryStream":
                    {
                        SetPropertyValue(sign.name, ReadMemoryStream(buffer, sign.bytes));
                        break;
                    }
					default:
					{
                        Type type = Type.GetType(sign.type);
						log.Assert(sign.bytes == 0);
						if (sign.count == 1)
						{
                           KProtoBuf obj = Activator.CreateInstance(type) as KProtoBuf;
						   obj.unpack(buffer);
						   SetPropertyValue(sign.name, obj);
						}
						else if(sign.count == 0)
						{
							log.Assert(type != null, "undefine objClass: ");
                            Assembly _Assembly = Assembly.Load("mscorlib");
                            Type _TypeList = _Assembly.GetType("System.Collections.Generic.List`1[[" + type.FullName + "," + type.Assembly.FullName + "]]");
                            object _List = System.Activator.CreateInstance(_TypeList);
                            MethodInfo methodinfo = _TypeList.GetMethod("Add");
							for (j = 0; j < count; j++)
							{
								KProtoBuf obj = Activator.CreateInstance(type) as KProtoBuf;
								obj.unpack(buffer);
                                object[] parameters = new object[1];
                                parameters[0] = obj;
                                methodinfo.Invoke(_List, parameters); 
							}
                            SetPropertyValue(sign.name, _List);
						}
                        else
                        {
                            log.Assert(type != null, "undefine objClass: ");
                            Assembly _Assembly = Assembly.Load("mscorlib");
                            Type _TypeArray = Type.GetType(type.FullName + "[]");

                            object _Array = System.Activator.CreateInstance(_TypeArray, count);
                            MethodInfo methodinfo = _TypeArray.GetMethod("SetValue", new Type[] { typeof(object), typeof(int) });
                            for (j = 0; j < count; j++)
                            {
                                KProtoBuf obj = Activator.CreateInstance(type) as KProtoBuf;
                                obj.unpack(buffer);
                                object[] parameters = new object[2];
                                parameters[0] = obj;
                                parameters[1] = j;
                                methodinfo.Invoke(_Array, parameters);
                            }
                            SetPropertyValue(sign.name, _Array);
                        }
						break;
					}
				}
			}
			
		}

		private static void WriteBytes(BinaryWriter bytes, byte[] src, int length)
		{
            bytes.Write(length);
            if(length > 0)
            {
                 bytes.Write(src, 0, length);
            }
           
		}
		
		private static byte[] ReadBytes(BinaryReader bytes, int length)
		{	
		    length = bytes.ReadInt32();
			if (length > 0)
			{
                return bytes.ReadBytes(length);
			}
            return null;
		}
		
		private static void WriteString(BinaryWriter bytes, string src, int length)
		{
            byte[] srcByByte = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(src);
			long begin = 0;
			long end = 0;
			if (length == 0)//没有指定发送长度,把整个字符串发过去
			{
				bytes.Write((short)0);
				begin = bytes.Seek(0, SeekOrigin.Current);
				bytes.Write(srcByByte);
				bytes.Write((byte)0);
				end = bytes.Seek(0, SeekOrigin.Current);
				
				length = (int)(end - begin);
                bytes.Seek(-length - KConstants.SIZE_OF_WORD, SeekOrigin.Current);
				bytes.Write((short)length);
				bytes.Seek(length, SeekOrigin.Current);
			}
			else//指定发送长度,把整个字符串的前length长度的字节发送过去
			{
				begin = bytes.Seek(0, SeekOrigin.Current);
                int readSize = srcByByte.Count() > length ? length : srcByByte.Count();
				bytes.Write(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(src), 0, readSize);
                bytes.Write((byte)0);
                end = bytes.Seek(0, SeekOrigin.Current);
				if (end - begin < length)
				{
                    bytes.Seek((int)(length - end + begin), SeekOrigin.Current);
				}
				
			}
		}
		
		private string ReadString(BinaryReader bytes, int length)
		{
			if (length == 0)
			{
				length = bytes.ReadInt16();
			}
			byte[] b = bytes.ReadBytes(length);
            return Encoding.ASCII.GetString(b, 0, length);
		}

        private static void WriteMemoryStream(BinaryWriter bytes, MemoryStream ms)
        {
            bytes.Write((short)ms.Length);
            bytes.Write(ms.ToArray(), 0, (int)ms.Length);
        }

        private static void WriteMemoryStream(BinaryWriter bytes, MemoryStream ms, int byteCount)
        {
            bytes.Write(ms.ToArray(), 0, byteCount);
        }

        private MemoryStream ReadMemoryStream(BinaryReader bytes, int length)
		{
			if (length == 0)
			{
				length = bytes.ReadInt16();
			}
			byte[] b = bytes.ReadBytes(length);
            return new MemoryStream(b);
		}	

        /**
		public static void GetClassByName(name:String):Class
		{
			try
			{
				return getDefinitionByName(name) as Class
			} 
			catch(error:Error) 
			{
				try 
				{   
					return getClassByAlias(name);  
				} 
				catch(e2:ReferenceError) 
				{     
					return null;
				} 
			}
			return null;
		}
         * **/
	}
}
