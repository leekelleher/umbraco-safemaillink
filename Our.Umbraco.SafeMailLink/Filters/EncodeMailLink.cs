using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Our.Umbraco.SafeMailLink.Filters
{
	public class EncodeMailLink : MemoryStream
	{
		private readonly string HtmlBodyClosing = "</body>";

		private readonly string RegExMailToLink = "(?<email>mailto:\\w+([-+.]\\w+)*?@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*?(\\?.*?)?)(?<restOfTag>\".*?>)(?<text>(.|\\s)*?)</a>";

		private Encoding TextEncoding;

		private Stream OutputStream = null;

		public EncodeMailLink(Stream output, Encoding encoding)
		{
			this.OutputStream = output;
			this.TextEncoding = encoding;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			// get the content string
			string content = this.TextEncoding.GetString(buffer, offset, count);

			int startIndex = 0;
			var output = new StringBuilder();
			var regex = new Regex(this.RegExMailToLink, RegexOptions.IgnoreCase);
			var matches = regex.Matches(content);

			// loop through each of the 'mailto' link matches
			foreach (Match match in matches)
			{
				var encodedEmail = BitConverter.ToString(this.TextEncoding.GetBytes(match.Groups["email"].Value)).Replace("-", string.Empty);
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

			// check if the closing </body> tag is matched
			if (Regex.IsMatch(content, this.HtmlBodyClosing, RegexOptions.IgnoreCase))
			{
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
				var matchBody = Regex.Match(content, this.HtmlBodyClosing, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
				output.Append(content.Substring(startIndex, matchBody.Index - startIndex));
				output.Append(javascript);
				output.Append(content.Substring(matchBody.Index));
			}

			// check if any output has been appended...
			if (output.Length == 0)
			{
				// ... if not, then append all content.
				output.Append(content);
			}

			// write out the output!
			var outputBuffer = this.TextEncoding.GetBytes(output.ToString());
			this.OutputStream.Write(outputBuffer, 0, outputBuffer.Length);
		}
	}
}