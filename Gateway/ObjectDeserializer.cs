using Gracie.ETF;
using Gracie.Gateway.Payload;
using Gracie.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;

namespace Gracie.Gateway
{
    public class ObjectDeserializer
    {
        private delegate bool Handle(Type t, object value, StringBuilder trace, ref object result, IEnumerator<Handle> nexts);
        private readonly ILogger<ObjectDeserializer> logger;
        private readonly Encoding textEncoding;

        public ObjectDeserializer(ILogger<ObjectDeserializer> logger, Encoding textEncoding)
        {
            this.logger = logger;
            this.textEncoding = textEncoding;
        }

        //public T DeserializePayload<T>(List<(string, object)> data, int? sequenceNumber, string eventName) where T : Payload.Payload
        //{
        //    var type = typeof(T);
        //    return (T)DeserializePayload(type, data, sequenceNumber, eventName);
        //}

        //public object DeserializePayload(Type t, List<(string, object)> data, int? sequenceNumber, string eventName)
        //{
        //    if (t.IsGenericType)
        //    {
        //        var subType = t.GenericTypeArguments.First();
        //        var trace = new StringBuilder().Append(t.Name).Append('<').Append(subType.Name).Append('>');
        //        var subInstance = Deserialize(subType, data, trace);
        //        var instance = Activator.CreateInstance(t, subInstance, sequenceNumber, eventName);
        //        return instance;
        //    }
        //    else
        //    {
        //        var trace = new StringBuilder().Append(t.Name);
        //        var instance = Activator.CreateInstance(t, sequenceNumber, eventName);
        //        if (data == null)
        //        {
        //            return instance;
        //        }
        //        return Deserialize(t, data, instance, trace);
        //    }
        //}



        public T Deserialize<T>(Dictionary<string, object> data)
        {
            var type = typeof(T);
            var trace = new StringBuilder(type.Name);
            return (T)Deserialize(type, data, trace);
        }

        public object Deserialize(Type t, Dictionary<string, object> data, StringBuilder trace = null)
        {
            var subtrace = new StringBuilder();
            if (trace != null)
                subtrace.Append(trace).Append('.');
            subtrace.Append(t.Name);
            var instance = Activator.CreateInstance(t);
            return Deserialize(t, data, instance, subtrace);
        }

        private  object Deserialize(Type t, Dictionary<string, object> data, object instance, StringBuilder trace)
        {
            var propertyInfo = t.GetProperties();
            foreach (var (key, value) in data)
            {
                if (key.StartsWith('_'))
                    continue;
                var p = propertyInfo.FirstOrDefault(x => x.GetCustomAttribute<EtfProperty>()?.Name == key);
                var subtrace = new StringBuilder().Append(trace).Append('.').Append(key);
                if (p == null)
                {
                    logger.Log(LogLevel.Information, "No property found for trace {trace}", subtrace.ToString());
                    continue;
                }
                object result = null;
                if (HandleTypes(p.PropertyType, value, subtrace, ref result))
                {
                    p.SetValue(instance, result);
                }
                else
                {
                    logger.Log(LogLevel.Warning, "No deserialization found for trace {trace}", subtrace.ToString());
                }
            }
            return instance;
        }

        [DebuggerStepThrough]
        private IEnumerable<Handle> HandleOrder()
        {
            yield return HandleNullableTypes;
            yield return HandleEnumTypes;
            yield return HandleSameTypes;
            yield return HandleBigIntToUlong;
            yield return HandleByteToInt;
            yield return HandleByteToString;
            yield return HandleStringToBool;
            yield return HandleListType;
            yield return HandleObjectType;
        }

        [DebuggerStepThrough]
        private bool HandleTypes(Type t, object value, StringBuilder trace, ref object result)
        {
            var nexts = HandleOrder().GetEnumerator();
            return nexts.MoveNext() ? nexts.Current(t, value, trace, ref result, nexts) : false;
        }

        [DebuggerStepThrough]
        private static bool NextHelper(Type t, object value, StringBuilder trace, ref object result, IEnumerator<Handle> nexts)
        {
            return nexts.MoveNext() ? nexts.Current(t, value, trace, ref result, nexts) : false;
        }

        private static bool HandleBigIntToUlong(Type t, object value, StringBuilder trace, ref object result, IEnumerator<Handle> nexts)
        {
            if (t == typeof(ulong) && value is BigInteger bigIntValue)
            {
                result = (ulong)bigIntValue;
                return true;
            }
            return NextHelper(t, value, trace, ref result, nexts);
        }

        private bool HandleNullableTypes(Type t, object value, StringBuilder trace, ref object result, IEnumerator<Handle> nexts)
        {
            if (t == typeof(string) && value is null)
            {
                return true;
            }
            if (t.IsGenericType && typeof(Nullable<>) == t.GetGenericTypeDefinition())
            {
                if (value is null)
                {
                    return true;
                }
                var subType = t.GenericTypeArguments.First();
                return HandleTypes(subType, value, trace, ref result);
            }
            return NextHelper(t, value, trace, ref result, nexts);
        }

        private bool HandleEnumTypes(Type t, object value, StringBuilder trace, ref object result, IEnumerator<Handle> nexts)
        {
            if (t.BaseType == typeof(Enum))
            {
                var subType = Enum.GetUnderlyingType(t);
                return HandleTypes(subType, value, trace, ref result);
            }
            return NextHelper(t, value, trace, ref result, nexts);
        }

        private static bool HandleSameTypes(Type t, object value, StringBuilder trace, ref object result, IEnumerator<Handle> nexts)
        {
            if (t.IsInstanceOfType(value))
            {
                result = value;
                return true;
            }
            return NextHelper(t, value, trace, ref result, nexts);
        }

        private static bool HandleByteToInt(Type t, object value, StringBuilder trace, ref object result, IEnumerator<Handle> nexts)
        {
            if (t == typeof(int) && value is byte byteVal)
            {
                result = Convert.ToInt32(byteVal);
                return true;
            }
            return NextHelper(t, value, trace, ref result, nexts);
        }

        private bool HandleByteToString(Type t, object value, StringBuilder trace, ref object result, IEnumerator<Handle> nexts)
        {
            if (t == typeof(string) && value is byte[] byteArrVal)
            {
                result = textEncoding.GetString(byteArrVal);
                return true;
            }
            return NextHelper(t, value, trace, ref result, nexts);
        }

        private static bool HandleStringToBool(Type t, object value, StringBuilder trace, ref object result, IEnumerator<Handle> nexts)
        {
            if (t == typeof(bool) && value is string stringValue)
            {
                if (stringValue == "true")
                {
                    result = true;
                    return true;
                }
                else if (stringValue == "false")
                {
                    result = false;
                    return true;
                }
            }
            return NextHelper(t, value, trace, ref result, nexts);
        }

        private bool HandleListType(Type t, object value, StringBuilder trace, ref object result, IEnumerator<Handle> nexts)
        {
            //TODO: check to make sure this check is working correctly
            if (t.IsGenericType && typeof(IList<>) == t.GetGenericTypeDefinition())
            {
                var myType = t.GenericTypeArguments.First();
                Type newListType = typeof(List<>).MakeGenericType(myType);
                var returnList = (IList)Activator.CreateInstance(newListType);

                if (value == null)
                {
                    result = returnList;
                    return true;
                }

                var valueList = ((List<object>)value).Select((x, i) => (x, i));
                foreach (var (item, i) in valueList)
                {
                    var subtrace = new StringBuilder().Append(trace).Append('[').Append(i).Append(']');
                    object subReturnResult = null;
                    if (HandleTypes(myType, item, subtrace, ref subReturnResult))
                    {
                        returnList.Add(subReturnResult);
                    }
                    else
                    {
                        logger.Log(LogLevel.Warning, "No deserialization found for trace {trace}", subtrace.ToString());
                    }
                }
                result = returnList;
                return true;
            }
            return NextHelper(t, value, trace, ref result, nexts);
        }

        private bool HandleObjectType(Type t, object value, StringBuilder trace, ref object result, IEnumerator<Handle> nexts)
        {
            if (value is Dictionary<string, object> sublist)
            {
                result = Deserialize(t, sublist, trace);
                return true;
            }
            return NextHelper(t, value, trace, ref result, nexts);
        }
    }
}
