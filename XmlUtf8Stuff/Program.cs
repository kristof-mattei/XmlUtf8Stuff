namespace XmlUtf8Stuff
{
	using System;
	using System.IO;
	using System.Text;
	using System.Xml;
	using System.Xml.Serialization;

	public class Program
	{
		private static void Main()
		{
			Entity entity = new Entity
				{
					Bar = "Bar",
					Foo = "Foo",
				};

			string withBom = entity.SerializeXmlWithBom();
			string withoutBom = entity.SerializeXmlWithoutBom();
			string withStringReader = entity.SerializeXmlWithStringWriter();

			// validate results with debugger

			if (withBom[0] == 60)
			{
				Console.WriteLine("YAY");
			}
			else
			{
				Console.WriteLine("Y U NO START WITH <?");
			}


			XmlSerializer xmlSerializer = new XmlSerializer(typeof (Entity));
			StringReader stringReader = new StringReader(withBom /* result is the value from the database */);

			Entity deserializedEntity = (Entity) xmlSerializer.Deserialize(stringReader);

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
		private static readonly XmlSerializerNamespaces XmlnsEmpty = new XmlSerializerNamespaces(new[]
			{
				new XmlQualifiedName(string.Empty, string.Empty),
			});

		public static string SerializeXmlWithStringWriter<TObject>(this TObject objectToSerialize)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof (TObject));


			StringWriter stringWriter = new StringWriter();

			xmlSerializer.Serialize(stringWriter, objectToSerialize, XmlnsEmpty);

			return stringWriter.ToString();
		}

		public static string SerializeXmlWithBom<TObject>(this TObject objectToSerialize)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof (TObject));


				XmlTextWriter xmlWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

				xmlSerializer.Serialize(xmlWriter, objectToSerialize, XmlnsEmpty);

				return Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int) memoryStream.Length);
			}
		}


		public static string SerializeXmlWithoutBom<TObject>(this TObject objectToSerialize)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof (TObject));

				XmlTextWriter xmlWriter = new XmlTextWriter(memoryStream, new UTF8Encoding(false));

				xmlSerializer.Serialize(xmlWriter, objectToSerialize, XmlnsEmpty);

				return Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int) memoryStream.Length);
			}
		}
	}
}