using Snakezzzzzzzz.Models;
using System.Collections.Concurrent;

namespace Snakezzzzzzzz.Code
{
	public class GameManager
	{
		public ConcurrentDictionary<string, Snake> Snakes { get; set; }

		public GameManager()
		{
			Snakes = new ConcurrentDictionary<string, Snake>();
		}
	}
}