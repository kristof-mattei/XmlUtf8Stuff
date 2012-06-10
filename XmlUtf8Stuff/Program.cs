namespace XmlUtf8Stuff
{
	using System.Xml;
	using System.Xml.Serialization;
	using System.Text;
	using System;
	using System.IO;

	public class Program
	{
		private static void Main()
		{
			Entity entity = new Entity
			{
				Bar = "Bar",
				Foo = "Foo",
			};

			var result = entity.SerializeXml();

			if (result[0] == 60)
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


	public static class XmlSerializerHelper
	{
		public static string SerializeXml<TObject>(this TObject objectToSerialize)
		{
			using (var memoryStream = new MemoryStream())
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(TObject));

				XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces(new []
					{
						new XmlQualifiedName(string.Empty,string.Empty),
					});

				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
				xmlSerializer.Serialize(xmlTextWriter, objectToSerialize, xmlnsEmpty);

				return Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			}
		}
	}
}