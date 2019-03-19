using Newtonsoft.Json;
using Snakezzzzzzzz.Models;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using WebSocketManager;
using WebSocketManager.Common;

namespace Snakezzzzzzzz.Code
{
	public class GameHandler : WebSocketHandler
	{
		private GameManager manager = GameManager.Instance;

		public GameHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
		{
			new Timer(Callback, null, 0, 1000 / 15);
		}

		private void Callback(object state)
		{
			var listOfSnakes = JsonConvert.SerializeObject(manager.Snakes.Values);
			this.InvokeClientMethodToAllAsync("GetSnakes", listOfSnakes).Wait();
		}

		private async Task GetSnakes(object state)
		{
			var listOfSnakes = JsonConvert.SerializeObject(manager.Snakes.Values);
		}

		public async Task ConnectSnake(string id, string serializedSnake)
		{
			var snake = JsonConvert.DeserializeObject<Snake>(serializedSnake);
			var exists = this.manager.Snakes.ContainsKey(id);

			if (!exists)
				this.manager.Snakes.TryAdd(id, snake);
		}

		public async Task DisconnectSnake(string id, string serializedSnake)
		{
			this.manager.Snakes.TryRemove(id, out Snake removedSnake);
		}

		public async Task Move(string id, string serializedSnake)
		{
			var snake = JsonConvert.DeserializeObject<Snake>(serializedSnake);
			this.manager.Snakes.TryGetValue(id, out Snake existingSnake);

			if (existingSnake != null)
			{
				existingSnake.X = snake.X;
				existingSnake.Y = snake.Y;
				existingSnake.Tail = snake.Tail;
				existingSnake.Trail = snake.Trail;
			}
		}

		public override async Task OnConnected(WebSocket socket)
		{
			await base.OnConnected(socket);
		}

		public override async Task OnDisconnected(WebSocket socket)
		{
			await base.OnDisconnected(socket);
		}
	}
}
