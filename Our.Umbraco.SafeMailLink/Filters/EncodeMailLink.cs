using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Our.Umbraco.SafeMailLink.Filters
{
	public class EncodeMailLink : MemoryStream
	{
		const string HTML_BODY_CLOSING = "</body>";

		const string REGEX_MAILTO_LINK = "(?<email>mailto:\\w+([-+.]\\w+)*?@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*?(\\?.*?)?)(?<restOfTag>\".*?>)(?<text>(.|\\s)*?)</a>";

		private Stream OutputStream = null;

		public EncodeMailLink(Stream output)
		{
			this.OutputStream = output;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			string content = UTF8Encoding.UTF8.GetString(buffer, offset, count);

			if (!Regex.IsMatch(content, HTML_BODY_CLOSING, RegexOptions.IgnoreCase))
			{
				this.OutputStream.Write(buffer, offset, count);
				return;
			}

			int startIndex = 0;
			var output = new StringBuilder();
			var regex = new Regex(REGEX_MAILTO_LINK, RegexOptions.IgnoreCase);
			var matches = regex.Matches(content);

			if (matches.Count == 0)
			{
				this.OutputStream.Write(buffer, offset, count);
				return;
			}

			foreach (Match match in matches)
			{
				var encodedEmail = BitConverter.ToString(Encoding.ASCII.GetBytes(match.Groups["email"].Value)).Replace("-", string.Empty);
				var disguisedEmail = match.Groups["text"].Value.Replace("@", "@<!--x-->").Replace(".", "<!--y-->.");

				output
					.Append(content.Substring(startIndex, match.Index - startIndex))
					.Append("javascript:if(typeof(sendEmail)=='function'){sendEmail('")
					.Append(encodedEmail)
					.Append("');}")
					.Append(match.Groups["restOfTag"].Value)
					.Append(disguisedEmail)
					.Append("</a>");

				startIndex = match.Index + match.Length;
			}

			var javascript = @"
	<script type=""text/javascript"">
		function sendEmail(encodedEmail) {
			var email = '';
			for (i = 0; i < encodedEmail.length;)
			{
				var letter = encodedEmail.charAt(i) + encodedEmail.charAt(i + 1);
				email += String.fromCharCode(parseInt(letter, 16));
				i += 2;
			}
			location.href = email;
		}
	</script>
";
			var matchBody = Regex.Match(content, HTML_BODY_CLOSING, RegexOptions.IgnoreCase);
			output.Append(content.Substring(startIndex, matchBody.Index - startIndex));
			output.Append(javascript);
			output.Append(content.Substring(matchBody.Index));

			var outputBuffer = UTF8Encoding.UTF8.GetBytes(output.ToString());
			this.OutputStream.Write(outputBuffer, 0, outputBuffer.Length);
		}
	}
}