using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BusinessLogic
{
    public static class ObjectExtension
    {
        public static T CopyAs<T>(this object source,
            Func<KeyValuePair<string, object>, object> fallback = null) where T : class, new()
        {
            T target = Activator.CreateInstance<T>();
            source.CopyTo(target, fallback);
            return target;
        }

        public static void CopyTo(this object source, object target, Func<KeyValuePair<string, object>, object> fallback = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            PropertyInfo[] properties = source.GetType().GetProperties();
            Type type = target.GetType();
            foreach (PropertyInfo info in properties)
            {
                PropertyInfo property = type.GetProperty(info.Name);
                if ((property != null) && property.CanWrite)
                {
                    object obj2 = info.GetValue(source, null);
                    if (property.PropertyType == info.PropertyType)
                    {
                        property.SetValue(target, obj2, null);
                    }
                    else if ((property.PropertyType.IsGenericType && (property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        && (Nullable.GetUnderlyingType(property.PropertyType) == info.PropertyType))
                    {
                        property.SetValue(target, obj2, null);
                    }
                    else if ((info.PropertyType.IsGenericType && (info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))) && (Nullable.GetUnderlyingType(info.PropertyType) == property.PropertyType))
                    {
                        if (obj2 != null)
                        {
                            property.SetValue(target, obj2, null);
                        }
                    }
                    else
                    {
                        try
                        {
                            if (fallback != null)
                            {
                                object obj3 = fallback(new KeyValuePair<string, object>(property.Name, obj2));
                                if (obj3 != null)
                                {
                                    property.SetValue(target, obj3, null);
                                }
                            }
                            else
                            {
                                TypeConverter converter = TypeDescriptor.GetConverter(obj2);
                                if ((converter != null) && converter.CanConvertTo(property.PropertyType))
                                {
                                    object obj4 = converter.ConvertTo(obj2, property.PropertyType);
                                    property.SetValue(target, obj4, null);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //ex.Log(true);
                        }
                    }
                }
            }

        }

        public static T GetFieldValue<T>(this object entity, string propertyName)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Object is null.");
            }
            PropertyInfo property = entity.GetType().GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException(string.Format("Object ({0}) either does not contain a property called {1} or the property is not accessable due to its protection level.", entity.GetType().FullName, propertyName));
            }
            if (!property.CanRead)
            {
                throw new InvalidOperationException(string.Format("{1} property of [{0}] is a write-only property. Its value cannot be set.", entity.GetType().FullName, propertyName));
            }
            return (T)property.GetValue(entity, null);
        }

        public static T Or<T>(this T @this, T fallback)
        {
            if (@this != null)
            {
                return @this;
            }
            return fallback;
        }

        public static byte[] SerializeUsingBinaryFormat(this object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, obj);
                return stream.GetBuffer();
            }
        }

        public static void SetFieldValue(this object entity, string propertyName, object value)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Object is null.");
            }
            PropertyInfo property = entity.GetType().GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException(string.Format("Object ({0}) either does not contain a property called {1} or the property is not accessable due to its protection level.", entity.GetType().FullName, propertyName));
            }
            if (!property.CanWrite)
            {
                throw new InvalidOperationException(string.Format("{1} property of [{0}] is a read-only property. Its value cannot be set.", entity.GetType().FullName, propertyName));
            }
            value = Convert.ChangeType(value, property.PropertyType);
            property.SetValue(entity, value, null);
        }

        public static void ThrowIfNull<T>(this T @this, string name, string message = "") where T : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException(name, message ?? string.Empty);
            }
        }

        /// <summary>
        /// Serialize object to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string XmlSerialize(this object value)
        {
            XmlSerializer serializer = new XmlSerializer(value.GetType());
            XmlWriterSettings settings = new XmlWriterSettings();

            settings.Indent = true;
            settings.IndentChars = " ";
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true;

            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                serializer.Serialize(writer, value);
            }

            return sb.ToString();
        }

        public static T XmlDeserialize<T>(this string xml) where T : class
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(xml))
            {
                var obj = deserializer.Deserialize(reader) as T;
                return obj;
            }
        }
        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }
        public static T FromJson<T>(this string s)
        {
            T result = JsonConvert.DeserializeObject<T>(s);
            return result;
        }
    }
}
