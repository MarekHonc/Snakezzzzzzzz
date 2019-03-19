using Snakezzzzzzzz.Models;
using System.Collections.Concurrent;

namespace Snakezzzzzzzz.Code
{
	public class GameManager
	{
		private static GameManager instance;
		private static readonly object padLock = new object();
		public ConcurrentDictionary<string, Snake> Snakes { get; set; }

		public static GameManager Instance
		{
			get
			{
				lock (padLock)
				{
					return instance ?? (instance = new GameManager());
				}
			}
		}

		public GameManager()
		{
			Snakes = new ConcurrentDictionary<string, Snake>();
		}
	}
}