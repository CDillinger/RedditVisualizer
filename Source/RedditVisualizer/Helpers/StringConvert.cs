namespace RedditVisualizer.Helpers
{
	class StringConvert
	{
		public static string XmlStringToNormal(string xmlString)
		{
			while (xmlString.Contains("&quot;"))
			{
				string part1 = xmlString.Substring(0, xmlString.IndexOf("&quot;"));
				string part2 = xmlString.Substring(xmlString.IndexOf("&quot;") + 6);
				xmlString = part1 + '"' + part2;
			}

			while (xmlString.Contains("&amp;"))
			{
				string part1 = xmlString.Substring(0, xmlString.IndexOf("&amp;"));
				string part2 = xmlString.Substring(xmlString.IndexOf("&amp;") + 5);
				xmlString = part1 + '&' + part2;
			}

			while (xmlString.Contains("&apos;"))
			{
				string part1 = xmlString.Substring(0, xmlString.IndexOf("&apos;"));
				string part2 = xmlString.Substring(xmlString.IndexOf("&apos;") + 6);
				xmlString = part1 + "'" + part2;
			}

			while (xmlString.Contains("&lt;"))
			{
				string part1 = xmlString.Substring(0, xmlString.IndexOf("&lt;"));
				string part2 = xmlString.Substring(xmlString.IndexOf("&lt;") + 4);
				xmlString = part1 + '<' + part2;
			}

			while (xmlString.Contains("&gt;"))
			{
				string part1 = xmlString.Substring(0, xmlString.IndexOf("&gt;"));
				string part2 = xmlString.Substring(xmlString.IndexOf("&gt;") + 4);
				xmlString = part1 + '>' + part2;
			}

			return xmlString;
		}
	}
}
