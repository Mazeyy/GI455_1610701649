var websocket = require('ws');

var websocketServer = new websocket.Server({port:25500}, ()=>{
	console.log("Maze server is running")
});

var wslist = [];

websocketServer.on("connection", (ws, rq)=>{

	console.log('client connected.');

	wslist.push(ws);

	ws.on("message", (data)=>{
		
		console.log("send form client : " + data);
		Boardcast(data);
	});

	ws.on("close", ()=>{
		
		wslist = ArrayRemove(wslist, ws);
		console.log("client disconnected.");
	});
});
	
	function ArrayRemove(arr, value)
	{
		return arr.filter((element)=>{
			return element != value;
		})
	}

	function Boardcast(data)
	{
		for(var i = 0; i < wslist.length; i++)
		{
			wslist[i].send(data);
		}
	}