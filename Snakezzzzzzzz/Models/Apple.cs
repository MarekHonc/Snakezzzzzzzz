using Newtonsoft.Json;
using System;

namespace Snakezzzzzzzz.Models
{
	public class Apple
	{
		[JsonProperty("x")]
		public int X
		{
			get;
			private set;
		}

		[JsonProperty("y")]
		public int Y
		{
			get;
			private set;
		}

		public bool IsGenerated => !(this.X == 0 && this.Y == 0);

		public void GenerateNew(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
	}
}
