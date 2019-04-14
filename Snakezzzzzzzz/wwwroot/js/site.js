var snake;
var apple;
var map;
var snakes = [];
var connection;

window.onload = function () {
	canv = document.getElementById("gc");
	ctx = canv.getContext("2d");
	document.addEventListener("keydown", keyPush);

	map = new Map();
	snake = new Snake();

	connection = new WebSocketManager.Connection("ws://localhost:8000/server");

	connection.connectionMethods.onConnected = () => {
		snake.id = connection.connectionId;
		connection.invoke("ConnectSnake", connection.connectionId, JSON.stringify(snake), JSON.stringify([Math.floor(Math.random() * map.tilecount), Math.floor(Math.random() * map.tilecount)]));
	}

	connection.connectionMethods.onDisconnected = () => {
		connection.invoke("DisconnectSnake", connection.connectionId);
	}

	window.addEventListener('beforeunload', function (event) {
		connection.invoke("DisconnectSnake", connection.connectionId);
	}, false);

	connection.clientMethods["GetSnakes"] = (sersnakes) => {
		snakes = JSON.parse(sersnakes);
	};

	connection.clientMethods["SpawnApple"] = (newApple) => {
		var appleObj = JSON.parse(newApple);
		apple = new Apple(appleObj.x, appleObj.y);
	}

	connection.clientMethods["SnakeCollision"] = (snake) => {
		window.location.href = "/GameOver";
	}

	connection.start();

	setInterval(game, 1000 / 15);
}

function Snake() {
	this.id = "";
	this.x = Math.floor(Math.random() * map.tilecount);
	this.y = Math.floor(Math.random() * map.tilecount);
	this.xvel = 1;
	this.yvel = 0;
	this.tail = 5;
	this.trail = [];
}

Snake.prototype.eat = function () {
	this.tail++;
}

Snake.prototype.die = function () {
	this.tail = 5;
}

function Map() {
	this.gridsize = 20;
	this.tilecount = 40;
}

function Apple(x, y) {
	this.x = x;
	this.y = y;
}

function game() {
	// Move
	snake.x += snake.xvel;
	snake.y += snake.yvel;

	// Edges of map
	if (snake.x < 0) {
		snake.x = map.tilecount - 1;
	}

	if (snake.x > map.tilecount - 1) {
		snake.x = 0;
	}
	if (snake.y < 0) {
		snake.y = map.tilecount - 1;
	}

	if (snake.y > map.tilecount - 1) {
		snake.y = 0;
	}

	ctx.fillStyle = "black";
	ctx.fillRect(0, 0, canv.width, canv.height);

	// Add snakes
	snakes.forEach(function (sna) {
		if (sna.id != snake.id) {
			for (var i = 0; i < sna.trail.length; i++) {
				ctx.fillStyle = "lime";
				ctx.fillRect(sna.trail[i].x * map.gridsize, sna.trail[i].y * map.gridsize, map.gridsize - 2, map.gridsize - 2);
			}
		}
	});

	ctx.fillStyle = "orange";

	// Render current snake
	for (var i = 0; i < snake.trail.length; i++) {
		ctx.fillRect(snake.trail[i].x * map.gridsize, snake.trail[i].y * map.gridsize, map.gridsize - 2, map.gridsize - 2);
		if (snake.trail[i].x == snake.x && snake.trail[i].y == snake.y) {
			snake.tail = 5;
		}
	}
	snake.trail.push({ x: snake.x, y: snake.y });
	while (snake.trail.length > snake.tail) {
		snake.trail.shift();
	}

	// Notify server
	connection.invoke("Move", connection.connectionId, JSON.stringify(snake));

	// Get apple
	if (apple != undefined) {
		if (apple.x == snake.x && apple.y == snake.y) {
			snake.tail++;
			connection.invoke("RespawnApple", JSON.stringify([Math.floor(Math.random() * map.tilecount), Math.floor(Math.random() * map.tilecount)]));
		}
		ctx.fillStyle = "red";
		ctx.fillRect(apple.x * map.gridsize, apple.y * map.gridsize, map.gridsize - 2, map.gridsize - 2);
	}
}

function keyPush(evt) {
	switch (evt.keyCode) {
		case 37:
			snake.xvel = -1; snake.yvel = 0;
			break;
		case 38:
			snake.xvel = 0; snake.yvel = -1;
			break;
		case 39:
			snake.xvel = 1; snake.yvel = 0;
			break;
		case 40:
			snake.xvel = 0; snake.yvel = 1;
			break;
	}
}