using Newtonsoft.Json;

namespace Snakezzzzzzzz.Models
{
	public class Snake
	{
		[JsonProperty("id")]
		public string Id
		{
			get;
			set;
		}

		[JsonProperty("x")]
		public int X
		{
			get;
			set;
		}

		[JsonProperty("y")]
		public int Y
		{
			get;
			set;
		}

		[JsonProperty("tail")]
		public int Tail
		{
			get;
			set;
		}

		[JsonProperty("trail")]
		public dynamic[] Trail
		{
			get;
			set;
		}
	}
}
