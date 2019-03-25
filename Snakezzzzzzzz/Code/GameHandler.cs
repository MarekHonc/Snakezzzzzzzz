using Newtonsoft.Json;
using Snakezzzzzzzz.Models;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WebSocketManager;

namespace Snakezzzzzzzz.Code
{
	public class GameHandler : WebSocketHandler
	{
		private GameManager manager;

		public GameHandler(WebSocketConnectionManager webSocketConnectionManager, GameManager manager, SingleTimer timer) : base(webSocketConnectionManager)
		{
			timer.Run(Callback);
			this.manager = manager;
		}

		private void Callback(object state)
		{
			var listOfSnakes = JsonConvert.SerializeObject(this.manager.Snakes.Values);
			this.InvokeClientMethodToAllAsync("GetSnakes", listOfSnakes).Wait();
		}

		private async Task GetSnakes(object state)
		{
			var listOfSnakes = JsonConvert.SerializeObject(manager.Snakes.Values);
		}

		public async Task ConnectSnake(string id, string serializedSnake, string fieldDimensions)
		{
			var snake = JsonConvert.DeserializeObject<Snake>(serializedSnake);
			var exists = this.manager.Snakes.ContainsKey(id);

			if (!exists)
				this.manager.Snakes.TryAdd(id, snake);

			if (!this.manager.Apple.IsGenerated)
			{
				var convertedDimenstions = JsonConvert.DeserializeObject<int[]>(fieldDimensions);
				this.manager.Apple.GenerateNew(convertedDimenstions[0], convertedDimenstions[1]);
			}

			var apple = JsonConvert.SerializeObject(this.manager.Apple);
			await this.InvokeClientMethodAsync(id, "SpawnApple", new[] { apple });
		}

		public async Task RespawnApple(string fieldDimensions)
		{
			var convertedDimenstions = JsonConvert.DeserializeObject<int[]>(fieldDimensions);
			this.manager.Apple.GenerateNew(convertedDimenstions[0], convertedDimenstions[1]);

			var apple = JsonConvert.SerializeObject(this.manager.Apple);
			await this.InvokeClientMethodToAllAsync("SpawnApple", apple);
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
