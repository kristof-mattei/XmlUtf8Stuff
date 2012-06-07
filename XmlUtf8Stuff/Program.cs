using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XmlUtf8Stuff
{
	class Program
	{
		static void Main(string[] args)
		{
			var entity = new Entity()
			             	{
			             		Bar = "Bar",
			             		Foo = "Foo",
			             	};

			var result = entity.SerializeXml();

			if(result[0] == 60)
			{
				Console.WriteLine("YAY");
			}
			else
			{
				Console.WriteLine("Y U NO START WITH <?");
			}

			//XDocument.

			//XmlSerializer foo = new XmlSerializer(typeof (Entity));
			//var entity2 = (Entity)foo.Deserialize(new StringReader(result));

			//var foo = XDocument.Parse(result);

			Console.ReadLine();
		}
	}

	public class Entity
	{
		public string Foo { get; set; }
		public string Bar { get; set; }
	}


	static class Helper
	{
		public static string SerializeXml<T>(this T toSerialize)
		{
			string xml;

			using (var stream = new MemoryStream())
			{
				var serializer = new XmlSerializer(typeof(T));

				var xmlnsEmpty = new XmlSerializerNamespaces();
				xmlnsEmpty.Add("", "");

				var xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8); 
				serializer.Serialize(xmlTextWriter, toSerialize, xmlnsEmpty);

				xml = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
			}

			return xml;
		} 

	}
}
