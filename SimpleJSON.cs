//#define USE_SharpZipLib

#if !UNITY_WEBPLAYER
#define USE_FileIO
#endif
/* * * * *
 * A simple JSON Parser / builder
 * ------------------------------
 * 
 * It mainly has been written as a simple JSON parser. It can build a JSON string
 * from the node-tree, or generate a node tree from any valid JSON string.
 * 
 * If you want to use compression when saving to file / stream / B64 you have to include
 * SharpZipLib ( http://www.icsharpcode.net/opensource/sharpziplib/ ) in your project and
 * define "USE_SharpZipLib" at the top of the file
 * 
 * Written by Bunny83 
 * 2012-06-09
 * 
 * Modified by oPless, 2014-09-21 to round-trip properly
 * 
 * Modified by Henrik Poulsen, 2015-01-12, improved performance by using StringBuilder and added support for long.
 *
 * Features / attributes:
 * - provides strongly typed node classes and lists / dictionaries
 * - provides easy access to class members / array items / data values
 * - the parser ignores data types. Each value is a string.
 * - only double quotes (") are used for quoting strings.
 * - values and names are not restricted to quoted strings. They simply add up and are trimmed.
 * - There are only 3 types: arrays(JSONArray), objects(JSONClass) and values(JSONData)
 * - provides "casting" properties to easily convert to / from those types:
 *   int / float / double / bool / long
 * - provides a common interface for each node so no explicit casting is required.
 * - the parser try to avoid errors, but if malformed JSON is parsed the result is undefined
 * 
 * 
 * 2012-12-17 Update:
 * - Added internal JSONLazyCreator class which simplifies the construction of a JSON tree
 *   Now you can simple reference any item that doesn't exist yet and it will return a JSONLazyCreator
 *   The class determines the required type by it's further use, creates the type and removes itself.
 * - Added binary serialization / deserialization.
 * - Added support for BZip2 zipped binary format. Requires the SharpZipLib ( http://www.icsharpcode.net/opensource/sharpziplib/ )
 *   The usage of the SharpZipLib library can be disabled by removing or commenting out the USE_SharpZipLib define at the top
 * - The serializer uses different types when it comes to store the values. Since my data values
 *   are all of type string, the serializer will "try" which format fits best. The order is: int, float, double, bool, string.
 *   It's not the most efficient way but for a moderate amount of data it should work on all platforms.
 * 
 * * * * */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleJSON
{
    public enum JSONBinaryTag
    {
        Array = 1,
        Class = 2,
        Value = 3,
        IntValue = 4,
        DoubleValue = 5,
        BoolValue = 6,
        FloatValue = 7,
        LongValue = 8,
        Null = 9
    }

    public abstract class JSONNode
    {
        #region common interface

        public virtual void Add(string aKey, JSONNode aItem)
        {
        }

        public virtual JSONNode this[int aIndex]
        {
            get { return null; }
            set { }
        }

        public virtual JSONNode this[string aKey]
        {
            get { return null; }
            set { }
        }

        public virtual string Value
        {
            get { return ""; }
            set { }
        }

        public virtual int Count
        {
            get { return 0; }
        }

        public virtual void Add(JSONNode aItem)
        {
            Add("", aItem);
        }

        public virtual JSONNode Remove(string aKey)
        {
            return null;
        }

        public virtual JSONNode Remove(int aIndex)
        {
            return null;
        }

        public virtual JSONNode Remove(JSONNode aNode)
        {
            return aNode;
        }

        public virtual IEnumerable<JSONNode> Children
        {
            get { yield break; }
        }

        public IEnumerable<JSONNode> DeepChildren
        {
            get
            {
                foreach (var C in Children)
                {
                    foreach (var D in C.DeepChildren)
                    {
                        yield return D;
                    }
                }
            }
        }

        public override string ToString()
        {
            return "JSONNode";
        }

        public virtual string ToString(string aPrefix)
        {
            return "JSONNode";
        }

        public abstract string ToJSON(int prefix);

        #endregion common interface

        #region typecasting properties

        public virtual JSONBinaryTag Tag { get; set; }

        public virtual bool IsNull
        {
            get { return Tag == JSONBinaryTag.Null; }
        }

        public virtual int? AsInt
        {
            get
            {
                if (IsNull) return null;
                var v = 0;
                if (int.TryParse(Value, out v))
                    return v;
                return 0;
            }
            set
            {
                Value = value.ToString();
                Tag = JSONBinaryTag.IntValue;
            }
        }

        public virtual long? AsLong
        {
            get
            {
                if (IsNull) return null;
                long v = 0;
                if (long.TryParse(Value, out v))
                    return v;
                return 0;
            }
            set
            {
                Value = value.ToString();
                Tag = JSONBinaryTag.LongValue;
            }
        }

        public virtual float? AsFloat
        {
            get
            {
                if (IsNull) return null;
                var v = 0.0f;
                if (float.TryParse(Value, NumberStyles.Any, CultureInfo.InvariantCulture, out v))
                    return v;
                return 0.0f;
            }
            set
            {
                Value = value.Value.ToString(CultureInfo.InvariantCulture);
                Tag = JSONBinaryTag.FloatValue;
            }
        }

        public virtual double? AsDouble
        {
            get
            {
                if (IsNull) return null;
                var v = 0.0;
                if (double.TryParse(Value, NumberStyles.Any, CultureInfo.InvariantCulture, out v))
                    return v;
                return 0.0;
            }
            set
            {
                Value = value.Value.ToString(CultureInfo.InvariantCulture);
                Tag = JSONBinaryTag.DoubleValue;
            }
        }

        public virtual bool? AsBool
        {
            get
            {
                if (IsNull) return null;
                var v = false;
                if (bool.TryParse(Value, out v))
                    return v;
                return !string.IsNullOrEmpty(Value);
            }
            set
            {
                if (value == null)
                {
                    Value = "null";
                }
                else
                {
                    Value = (value.Value) ? "true" : "false";
                }
                Tag = JSONBinaryTag.BoolValue;
            }
        }

        public virtual JSONArray AsArray
        {
            get { return this as JSONArray; }
        }

        public virtual JSONClass AsObject
        {
            get { return this as JSONClass; }
        }

        #endregion typecasting properties

        #region operators

        public static implicit operator JSONNode(string s)
        {
            return new JSONData(s);
        }

        public static implicit operator string(JSONNode d)
        {
            return (d == null) ? null : d.Value;
        }

        public static bool operator ==(JSONNode a, object b)
        {
            if (b == null && a is JSONLazyCreator)
                return true;
            return ReferenceEquals(a, b);
        }

        public static bool operator !=(JSONNode a, object b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion operators

        internal static string Escape(string aText)
        {
            var result = new StringBuilder("");
            foreach (var c in aText)
            {
                switch (c)
                {
                    case '/':
                        result.Append("\\/");
                        break;
                    case '\\':
                        result.Append("\\\\");
                        break;
                    case '\"':
                        result.Append("\\\"");
                        break;
                    case '\n':
                        result.Append("\\n");
                        break;
                    case '\r':
                        result.Append("\\r");
                        break;
                    case '\t':
                        result.Append("\\t");
                        break;
                    case '\b':
                        result.Append("\\b");
                        break;
                    case '\f':
                        result.Append("\\f");
                        break;
                    default:
                        result.Append(c);
                        break;
                }
            }
            return result.ToString();
        }

        private static JSONData Numberize(string token)
        {
            var flag = false;
            var integer = 0;
            var longInteger = 0L;
            double real = 0;

            if (token.Equals("null"))
            {
                return new JSONData(null);
            }

            if (int.TryParse(token, out integer))
            {
                return new JSONData(integer);
            }

            if (long.TryParse(token, out longInteger))
            {
                return new JSONData(longInteger);
            }

            if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out real))
            {
                return new JSONData(real);
            }

            if (bool.TryParse(token, out flag))
            {
                return new JSONData(flag);
            }

            throw new NotImplementedException(token);
        }

        private static void AddElement(JSONNode ctx, string token, string tokenName, bool tokenIsString)
        {
            if (tokenIsString)
            {
                if (ctx is JSONArray)
                    ctx.Add(token);
                else
                    ctx.Add(tokenName, token); // assume dictionary/object
            }
            else
            {
                var number = Numberize(token);
                if (ctx is JSONArray)
                    ctx.Add(number);
                else
                    ctx.Add(tokenName, number);
            }
        }

        public static JSONNode Parse(string aJSON)
        {
            var stack = new Stack<JSONNode>();
            JSONNode ctx = null;
            var i = 0;
            var Token = new StringBuilder("");
            var TokenName = "";
            var QuoteMode = false;
            var TokenIsString = false;
            while (i < aJSON.Length)
            {
                var currentChar = aJSON[i];
                switch (currentChar)
                {
                    case '{':
                        if (QuoteMode)
                        {
                            Token.Append(currentChar);
                            break;
                        }
                        stack.Push(new JSONClass());
                        if (ctx != null)
                        {
                            TokenName = TokenName.Trim();
                            if (ctx is JSONArray)
                                ctx.Add(stack.Peek());
                            else if (TokenName.Length != 0)
                                ctx.Add(TokenName, stack.Peek());
                        }
                        TokenName = "";
                        Token = new StringBuilder();
                        ctx = stack.Peek();
                        break;

                    case '[':
                        if (QuoteMode)
                        {
                            Token.Append(currentChar);
                            break;
                        }

                        stack.Push(new JSONArray());
                        if (ctx != null)
                        {
                            TokenName = TokenName.Trim();

                            if (ctx is JSONArray)
                                ctx.Add(stack.Peek());
                            else if (TokenName.Length != 0)
                                ctx.Add(TokenName, stack.Peek());
                        }
                        TokenName = "";
                        Token = new StringBuilder();
                        ctx = stack.Peek();
                        break;

                    case '}':
                    case ']':
                        if (QuoteMode)
                        {
                            Token.Append(currentChar);
                            break;
                        }
                        if (stack.Count == 0)
                            throw new Exception("JSON Parse: Too many closing brackets");

                        stack.Pop();
                        if (Token.Length != 0)
                        {
                            TokenName = TokenName.Trim();
                            /*
                            if (ctx is JSONArray)
                                ctx.Add (Token);
                            else if (TokenName != "")
                                ctx.Add (TokenName, Token);
                                */
                            AddElement(ctx, Token.ToString(), TokenName, TokenIsString);
                            TokenIsString = false;
                        }
                        TokenName = "";
                        Token = new StringBuilder();
                        if (stack.Count > 0)
                            ctx = stack.Peek();
                        break;

                    case ':':
                        if (QuoteMode)
                        {
                            Token.Append(currentChar);
                            break;
                        }
                        if (TokenName.Length > 0)
                            throw new Exception("JSON Parse: The json seems to be broken");
                        TokenName = Token.ToString();
                        Token = new StringBuilder();
                        TokenIsString = false;
                        break;

                    case '"':
                        QuoteMode ^= true;
                        TokenIsString = QuoteMode ? true : TokenIsString;
                        break;

                    case ',':
                        if (QuoteMode)
                        {
                            Token.Append(currentChar);
                            break;
                        }
                        if (Token.Length != 0)
                        {
                            /*
                            if (ctx is JSONArray) {
                                ctx.Add (Token);
                            } else if (TokenName != "") {
                                ctx.Add (TokenName, Token);
                            }
                            */
                            AddElement(ctx, Token.ToString(), TokenName, TokenIsString);
                            TokenIsString = false;
                        }
                        TokenName = "";
                        Token = new StringBuilder();
                        TokenIsString = false;
                        break;

                    case '\r':
                    case '\n':
                        break;

                    case ' ':
                    case '\t':
                        if (QuoteMode)
                            Token.Append(currentChar);
                        break;

                    case '\\':
                        ++i;
                        if (QuoteMode)
                        {
                            var C = aJSON[i];
                            // The sequences \/, \" and \\ we remove the backslash from when parsing
                            // for \u we convert it into the character it represents
                            // and all others we leave alone
                            switch (C)
                            {
                                case '/':
                                    Token.Append('/');
                                    break;
                                case '"':
                                    Token.Append('"');
                                    break;
                                case '\\':
                                    Token.Append('\\');
                                    break;
                                case 'u':
                                {
                                    var s = aJSON.Substring(i + 1, 4);
                                    Token.Append((char) int.Parse(
                                        s,
                                        NumberStyles.AllowHexSpecifier));
                                    i += 4;
                                    break;
                                }
                                default:
                                    Token.Append('\\');
                                    Token.Append(C);
                                    break;
                            }
                        }
                        break;

                    default:
                        if (!QuoteMode)
                        {
                            // We check that we dont have illegal characters outside the quotes
                            switch (currentChar)
                            {
                                case '1':
                                case '2':
                                case '3':
                                case '4':
                                case '5':
                                case '6':
                                case '7':
                                case '8':
                                case '9':
                                case '0':
                                case '+':
                                case '-':
                                case 'e':
                                case 'E':
                                case '.':
                                    break;
                                case 'n':
                                {
                                    var s = aJSON.Substring(i, 4);
                                    if (s == "null")
                                    {
                                        i += 4;
                                        Token.Append(s);
                                        continue;
                                    }
                                    throw new Exception("Json format seems invalid");
                                }
                                case 'f':
                                {
                                    var s = aJSON.Substring(i, 5);
                                    if (s == "false")
                                    {
                                        i += 5;
                                        Token.Append(s);
                                        continue;
                                    }
                                    throw new Exception("Json format seems invalid");
                                }
                                case 't':
                                {
                                    var s = aJSON.Substring(i, 4);
                                    if (s == "true")
                                    {
                                        i += 4;
                                        Token.Append(s);
                                        continue;
                                    }
                                    throw new Exception("Json format seems invalid");
                                }
                                default:
                                    throw new Exception("Json format seems invalid");
                            }
                        }

                        Token.Append(currentChar);
                        break;
                }
                ++i;
            }
            if (QuoteMode)
            {
                throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
            }
            if (stack.Count != 0)
            {
                throw new Exception("There are unclosed {} or [] in the string");
            }
            return ctx;
        }

        public virtual void Serialize(BinaryWriter aWriter)
        {
        }

        public void SaveToStream(Stream aData)
        {
            var W = new BinaryWriter(aData);
            Serialize(W);
        }

#if USE_SharpZipLib
		public void SaveToCompressedStream(System.IO.Stream aData)
		{
			using (var gzipOut = new ICSharpCode.SharpZipLib.BZip2.BZip2OutputStream(aData))
			{
				gzipOut.IsStreamOwner = false;
				SaveToStream(gzipOut);
				gzipOut.Close();
			}
		}

		public void SaveToCompressedFile(string aFileName)
		{
		
#if USE_FileIO
			System.IO.Directory.CreateDirectory((new System.IO.FileInfo(aFileName)).Directory.FullName);
			using(var F = System.IO.File.OpenWrite(aFileName))
			{
				SaveToCompressedStream(F);
			}
		
#else
			throw new Exception("Can't use File IO stuff in webplayer");
#endif
		}
		public string SaveToCompressedBase64()
		{
			using (var stream = new System.IO.MemoryStream())
			{
				SaveToCompressedStream(stream);
				stream.Position = 0;
				return System.Convert.ToBase64String(stream.ToArray());
			}
		}
		
#else
        public void SaveToCompressedStream(Stream aData)
        {
            throw new Exception(
                "Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public void SaveToCompressedFile(string aFileName)
        {
            throw new Exception(
                "Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public string SaveToCompressedBase64()
        {
            throw new Exception(
                "Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }
#endif

        public void SaveToFile(string aFileName)
        {
#if USE_FileIO
            Directory.CreateDirectory((new FileInfo(aFileName)).Directory.FullName);
            using (var F = File.OpenWrite(aFileName))
            {
                SaveToStream(F);
            }
#else
			throw new Exception ("Can't use File IO stuff in webplayer");
#endif
        }

        public string SaveToBase64()
        {
            using (var stream = new MemoryStream())
            {
                SaveToStream(stream);
                stream.Position = 0;
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public static JSONNode Deserialize(BinaryReader aReader)
        {
            var type = (JSONBinaryTag) aReader.ReadByte();
            switch (type)
            {
                case JSONBinaryTag.Array:
                {
                    var count = aReader.ReadInt32();
                    var tmp = new JSONArray();
                    for (var i = 0; i < count; i++)
                        tmp.Add(Deserialize(aReader));
                    return tmp;
                }
                case JSONBinaryTag.Class:
                {
                    var count = aReader.ReadInt32();
                    var tmp = new JSONClass();
                    for (var i = 0; i < count; i++)
                    {
                        var key = aReader.ReadString();
                        var val = Deserialize(aReader);
                        tmp.Add(key, val);
                    }
                    return tmp;
                }
                case JSONBinaryTag.Value:
                {
                    return new JSONData(aReader.ReadString());
                }
                case JSONBinaryTag.IntValue:
                {
                    return new JSONData(aReader.ReadInt32());
                }
                case JSONBinaryTag.DoubleValue:
                {
                    return new JSONData(aReader.ReadDouble());
                }
                case JSONBinaryTag.BoolValue:
                {
                    return new JSONData(aReader.ReadBoolean());
                }
                case JSONBinaryTag.FloatValue:
                {
                    return new JSONData(aReader.ReadSingle());
                }
                case JSONBinaryTag.LongValue:
                {
                    return new JSONData(aReader.ReadInt64());
                }
                case JSONBinaryTag.Null:
                {
                    return new JSONData(null);
                }

                default:
                {
                    throw new Exception("Error deserializing JSON. Unknown tag: " + type);
                }
            }
        }

#if USE_SharpZipLib
		public static JSONNode LoadFromCompressedStream(System.IO.Stream aData)
		{
			var zin = new ICSharpCode.SharpZipLib.BZip2.BZip2InputStream(aData);
			return LoadFromStream(zin);
		}
		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
#if USE_FileIO
			using(var F = System.IO.File.OpenRead(aFileName))
			{
				return LoadFromCompressedStream(F);
			}
#else
			throw new Exception("Can't use File IO stuff in webplayer");
#endif
		}
		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			var tmp = System.Convert.FromBase64String(aBase64);
			var stream = new System.IO.MemoryStream(tmp);
			stream.Position = 0;
			return LoadFromCompressedStream(stream);
		}
#else
        public static JSONNode LoadFromCompressedFile(string aFileName)
        {
            throw new Exception(
                "Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public static JSONNode LoadFromCompressedStream(Stream aData)
        {
            throw new Exception(
                "Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public static JSONNode LoadFromCompressedBase64(string aBase64)
        {
            throw new Exception(
                "Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }
#endif

        public static JSONNode LoadFromStream(Stream aData)
        {
            using (var R = new BinaryReader(aData))
            {
                return Deserialize(R);
            }
        }

        public static JSONNode LoadFromFile(string aFileName)
        {
#if USE_FileIO
            using (var F = File.OpenRead(aFileName))
            {
                return LoadFromStream(F);
            }
#else
			throw new Exception ("Can't use File IO stuff in webplayer");
#endif
        }

        public static JSONNode LoadFromBase64(string aBase64)
        {
            var tmp = Convert.FromBase64String(aBase64);
            var stream = new MemoryStream(tmp);
            stream.Position = 0;
            return LoadFromStream(stream);
        }
    }

    // End of JSONNode

    public class JSONArray : JSONNode, IEnumerable
    {
        private readonly List<JSONNode> m_List = new List<JSONNode>();

        public override JSONNode this[int aIndex]
        {
            get
            {
                if (aIndex < 0 || aIndex >= m_List.Count)
                    return new JSONLazyCreator(this);
                return m_List[aIndex];
            }
            set
            {
                if (aIndex < 0 || aIndex >= m_List.Count)
                    m_List.Add(value);
                else
                    m_List[aIndex] = value;
            }
        }

        public override JSONNode this[string aKey]
        {
            get { return new JSONLazyCreator(this); }
            set { m_List.Add(value); }
        }

        public override int Count
        {
            get { return m_List.Count; }
        }

        public override IEnumerable<JSONNode> Children
        {
            get
            {
                foreach (var N in m_List)
                    yield return N;
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var N in m_List)
                yield return N;
        }

        public override void Add(string aKey, JSONNode aItem)
        {
            m_List.Add(aItem);
        }

        public override JSONNode Remove(int aIndex)
        {
            if (aIndex < 0 || aIndex >= m_List.Count)
                return null;
            var tmp = m_List[aIndex];
            m_List.RemoveAt(aIndex);
            return tmp;
        }

        public override JSONNode Remove(JSONNode aNode)
        {
            m_List.Remove(aNode);
            return aNode;
        }

        public override string ToString()
        {
            var result = new StringBuilder("[ ");
            foreach (var N in m_List)
            {
                if (result.Length > 2)
                    result.Append(", ");
                result.Append(N.ToString());
            }
            result.Append(" ]");
            return result.ToString();
        }

        public override string ToString(string aPrefix)
        {
            var result = new StringBuilder("[ ");
            foreach (var N in m_List)
            {
                if (result.Length > 3)
                    result.Append(", ");
                result.Append("\n");
                result.Append(aPrefix);
                result.Append("   ");
                result.Append(N.ToString(string.Format("{0}   ", aPrefix)));
            }
            result.Append("\n");
            result.Append(aPrefix);
            result.Append("]");
            return result.ToString();
        }

        public override string ToJSON(int prefix)
        {
            var s = new string(' ', (prefix + 1)*2);
            var result = new StringBuilder("[ ");
            foreach (var n in m_List)
            {
                if (result.Length > 3)
                    result.Append(", ");
                result.Append("\n");
                result.Append(s);
                result.Append(n.ToJSON(prefix + 1));
            }
            result.Append("\n");
            result.Append(s);
            result.Append("]");
            return result.ToString();
        }

        public override void Serialize(BinaryWriter aWriter)
        {
            aWriter.Write((byte) JSONBinaryTag.Array);
            aWriter.Write(m_List.Count);
            for (var i = 0; i < m_List.Count; i++)
            {
                m_List[i].Serialize(aWriter);
            }
        }
    }

    // End of JSONArray

    public class JSONClass : JSONNode, IEnumerable
    {
        private readonly Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

        public override JSONNode this[string aKey]
        {
            get
            {
                if (m_Dict.ContainsKey(aKey))
                    return m_Dict[aKey];
                return new JSONLazyCreator(this, aKey);
            }
            set
            {
                if (m_Dict.ContainsKey(aKey))
                    m_Dict[aKey] = value;
                else
                    m_Dict.Add(aKey, value);
            }
        }

        public override JSONNode this[int aIndex]
        {
            get
            {
                if (aIndex < 0 || aIndex >= m_Dict.Count)
                    return null;
                return m_Dict.ElementAt(aIndex).Value;
            }
            set
            {
                if (aIndex < 0 || aIndex >= m_Dict.Count)
                    return;
                var key = m_Dict.ElementAt(aIndex).Key;
                m_Dict[key] = value;
            }
        }

        public override int Count
        {
            get { return m_Dict.Count; }
        }

        public override IEnumerable<JSONNode> Children
        {
            get
            {
                foreach (var N in m_Dict)
                    yield return N.Value;
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var N in m_Dict)
                yield return N;
        }

        public override void Add(string aKey, JSONNode aItem)
        {
            if (!string.IsNullOrEmpty(aKey))
            {
                if (m_Dict.ContainsKey(aKey))
                    m_Dict[aKey] = aItem;
                else
                    m_Dict.Add(aKey, aItem);
            }
            else
                m_Dict.Add(Guid.NewGuid().ToString(), aItem);
        }

        public override JSONNode Remove(string aKey)
        {
            if (!m_Dict.ContainsKey(aKey))
                return null;
            var tmp = m_Dict[aKey];
            m_Dict.Remove(aKey);
            return tmp;
        }

        public override JSONNode Remove(int aIndex)
        {
            if (aIndex < 0 || aIndex >= m_Dict.Count)
                return null;
            var item = m_Dict.ElementAt(aIndex);
            m_Dict.Remove(item.Key);
            return item.Value;
        }

        public override JSONNode Remove(JSONNode aNode)
        {
            try
            {
                var item = m_Dict.Where(k => k.Value == aNode).First();
                m_Dict.Remove(item.Key);
                return aNode;
            }
            catch
            {
                return null;
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder("{");
            foreach (var N in m_Dict)
            {
                if (result.Length > 2)
                    result.Append(", ");
                result.Append("\"");
                result.Append(Escape(N.Key));
                result.Append("\":");
                result.Append(N.Value.ToString());
            }
            result.Append("}");
            return result.ToString();
        }

        public override string ToString(string aPrefix)
        {
            var result = new StringBuilder("{ ");
            foreach (var N in m_Dict)
            {
                if (result.Length > 3)
                    result.Append(", ");
                result.Append("\n");
                result.Append(aPrefix);
                result.Append("   ");
                result.Append("\"");
                result.Append(Escape(N.Key));
                result.Append("\" : ");
                result.Append(N.Value.ToString(string.Format("{0}   ", aPrefix)));
            }
            result.Append("\n");
            result.Append(aPrefix);
            result.Append("}");
            return result.ToString();
        }

        public override string ToJSON(int prefix)
        {
            var s = new string(' ', (prefix + 1)*2);
            var result = new StringBuilder("{ ");
            foreach (var n in m_Dict)
            {
                if (result.Length > 3)
                    result.Append(", ");
                result.Append("\n");
                result.Append(s);
                result.Append("\"");
                result.Append(n.Key);
                result.Append("\": ");
                result.Append(n.Value.ToJSON(prefix + 1));
            }
            result.Append("\n");
            result.Append(s);
            result.Append("}");
            return result.ToString();
        }

        public override void Serialize(BinaryWriter aWriter)
        {
            aWriter.Write((byte) JSONBinaryTag.Class);
            aWriter.Write(m_Dict.Count);
            foreach (var K in m_Dict.Keys)
            {
                aWriter.Write(K);
                m_Dict[K].Serialize(aWriter);
            }
        }
    }

    // End of JSONClass

    public class JSONData : JSONNode
    {
        private string m_Data;

        public JSONData(string aData)
        {
            if (aData == null)
            {
                m_Data = "null";
                Tag = JSONBinaryTag.Null;
                return;
            }
            m_Data = aData;
            Tag = JSONBinaryTag.Value;
        }

        public JSONData(float aData)
        {
            AsFloat = aData;
        }

        public JSONData(double aData)
        {
            AsDouble = aData;
        }

        public JSONData(bool aData)
        {
            AsBool = aData;
        }

        public JSONData(int aData)
        {
            AsInt = aData;
        }

        public JSONData(long aData)
        {
            AsLong = aData;
        }

        public override string Value
        {
            get { return m_Data; }
            set
            {
                m_Data = value;
                Tag = JSONBinaryTag.Value;
            }
        }

        public override string ToString()
        {
            if (Tag == JSONBinaryTag.BoolValue ||
                Tag == JSONBinaryTag.IntValue ||
                Tag == JSONBinaryTag.LongValue ||
                Tag == JSONBinaryTag.FloatValue ||
                Tag == JSONBinaryTag.DoubleValue ||
                Tag == JSONBinaryTag.Null)
            {
                return Escape(m_Data);
            }
            var result = new StringBuilder("\"");
            result.Append(Escape(m_Data));
            result.Append("\"");
            return result.ToString();
        }

        public override string ToString(string aPrefix)
        {
            if (Tag == JSONBinaryTag.BoolValue ||
                Tag == JSONBinaryTag.IntValue ||
                Tag == JSONBinaryTag.LongValue ||
                Tag == JSONBinaryTag.FloatValue ||
                Tag == JSONBinaryTag.DoubleValue ||
                Tag == JSONBinaryTag.Null)
            {
                return Escape(m_Data);
            }
            var result = new StringBuilder("\"");
            result.Append(Escape(m_Data));
            result.Append("\"");
            return result.ToString();
        }

        public override string ToJSON(int prefix)
        {
            switch (Tag)
            {
                case JSONBinaryTag.DoubleValue:
                case JSONBinaryTag.FloatValue:
                case JSONBinaryTag.IntValue:
                case JSONBinaryTag.LongValue:
                case JSONBinaryTag.BoolValue:
                case JSONBinaryTag.Null:
                    return m_Data;
                case JSONBinaryTag.Value:
                    var result = new StringBuilder("\"");
                    result.Append(Escape(m_Data));
                    result.Append("\"");
                    return result.ToString();
                default:
                    throw new NotSupportedException("This shouldn't be here: " + Tag);
            }
        }

        public override void Serialize(BinaryWriter aWriter)
        {
            var tmp = new JSONData("");

            if (m_Data == null)
            {
                aWriter.Write((byte) JSONBinaryTag.Null);
                return;
            }
            tmp.AsInt = AsInt;
            if (tmp.m_Data == m_Data)
            {
                aWriter.Write((byte) JSONBinaryTag.IntValue);
                aWriter.Write(AsInt.Value);
                return;
            }
            tmp.AsLong = AsLong;
            if (tmp.m_Data == m_Data)
            {
                aWriter.Write((byte) JSONBinaryTag.LongValue);
                aWriter.Write(AsLong.Value);
                return;
            }
            tmp.AsFloat = AsFloat;
            if (tmp.m_Data == m_Data)
            {
                aWriter.Write((byte) JSONBinaryTag.FloatValue);
                aWriter.Write(AsFloat.Value);
                return;
            }
            tmp.AsDouble = AsDouble;
            if (tmp.m_Data == m_Data)
            {
                aWriter.Write((byte) JSONBinaryTag.DoubleValue);
                aWriter.Write(AsDouble.Value);
                return;
            }

            tmp.AsBool = AsBool;
            if (tmp.m_Data == m_Data)
            {
                aWriter.Write((byte) JSONBinaryTag.BoolValue);
                aWriter.Write(AsBool.Value);
                return;
            }
            aWriter.Write((byte) JSONBinaryTag.Value);
            aWriter.Write(m_Data);
        }
    }

    // End of JSONData

    internal class JSONLazyCreator : JSONNode
    {
        private readonly string m_Key;
        private JSONNode m_Node;

        public JSONLazyCreator(JSONNode aNode)
        {
            m_Node = aNode;
            m_Key = null;
        }

        public JSONLazyCreator(JSONNode aNode, string aKey)
        {
            m_Node = aNode;
            m_Key = aKey;
        }

        public override JSONNode this[int aIndex]
        {
            get { return new JSONLazyCreator(this); }
            set
            {
                var tmp = new JSONArray();
                tmp.Add(value);
                Set(tmp);
            }
        }

        public override JSONNode this[string aKey]
        {
            get { return new JSONLazyCreator(this, aKey); }
            set
            {
                var tmp = new JSONClass();
                tmp.Add(aKey, value);
                Set(tmp);
            }
        }

        public override int? AsInt
        {
            get
            {
                var tmp = new JSONData(null);
                Set(tmp);
                return 0;
            }
            set
            {
                var tmp = new JSONData(value.Value);
                Set(tmp);
            }
        }

        public override long? AsLong
        {
            get
            {
                var tmp = new JSONData(null);
                Set(tmp);
                return 0;
            }
            set
            {
                var tmp = new JSONData(value.Value);
                Set(tmp);
            }
        }

        public override float? AsFloat
        {
            get
            {
                var tmp = new JSONData(null);
                Set(tmp);
                return 0.0f;
            }
            set
            {
                var tmp = new JSONData(value.Value);
                Set(tmp);
            }
        }

        public override double? AsDouble
        {
            get
            {
                var tmp = new JSONData(null);
                Set(tmp);
                return 0.0;
            }
            set
            {
                var tmp = new JSONData(value.Value);
                Set(tmp);
            }
        }

        public override bool? AsBool
        {
            get
            {
                var tmp = new JSONData(null);
                Set(tmp);
                return false;
            }
            set
            {
                var tmp = new JSONData(value.Value);
                Set(tmp);
            }
        }

        public override JSONArray AsArray
        {
            get
            {
                var tmp = new JSONArray();
                Set(tmp);
                return tmp;
            }
        }

        public override JSONClass AsObject
        {
            get
            {
                var tmp = new JSONClass();
                Set(tmp);
                return tmp;
            }
        }

        private void Set(JSONNode aVal)
        {
            if (m_Key == null)
            {
                m_Node.Add(aVal);
            }
            else
            {
                m_Node.Add(m_Key, aVal);
            }
            m_Node = null; // Be GC friendly.
        }

        public override void Add(JSONNode aItem)
        {
            var tmp = new JSONArray();
            tmp.Add(aItem);
            Set(tmp);
        }

        public override void Add(string aKey, JSONNode aItem)
        {
            var tmp = new JSONClass();
            tmp.Add(aKey, aItem);
            Set(tmp);
        }

        public static bool operator ==(JSONLazyCreator a, object b)
        {
            if (b == null)
                return true;
            return ReferenceEquals(a, b);
        }

        public static bool operator !=(JSONLazyCreator a, object b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return true;
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "";
        }

        public override string ToString(string aPrefix)
        {
            return "";
        }

        public override string ToJSON(int prefix)
        {
            return "";
        }
    }

    // End of JSONLazyCreator

    public static class JSON
    {
        public static JSONNode Parse(string aJSON)
        {
            return JSONNode.Parse(aJSON);
        }
    }
}