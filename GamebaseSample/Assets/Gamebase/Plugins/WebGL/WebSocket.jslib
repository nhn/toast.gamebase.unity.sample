var LibraryWebSockets = {
$webSocketInstances: [],

SocketCreate: function(url)
{
	var str = Pointer_stringify(url);
	var socket = {
		socket: new WebSocket(str),
		error: null,
		messages: []
	}
	
	socket.socket.onmessage = function (e) {
		socket.messages.push(e.data);
	};

	socket.socket.onclose = function (e) {
		if (e.code != 1000)
		{
			if (e.reason != null && e.reason.length > 0)
				socket.error = e.reason;
			else
			{
				switch (e.code)
				{
					case 1001: 
						socket.error = "Endpoint going away.";
						break;
					case 1002: 
						socket.error = "Protocol error.";
						break;
					case 1003: 
						socket.error = "Unsupported message.";
						break;
					case 1005: 
						socket.error = "No status.";
						break;
					case 1006: 
						socket.error = "Abnormal disconnection.";
						break;
					case 1009: 
						socket.error = "Data frame too large.";
						break;
					default:
						socket.error = "Error "+e.code;
				}
			}
		}
	}
	var instance = webSocketInstances.push(socket) - 1;
	return instance;
},

SocketState: function (socketInstance)
{
	var socket = webSocketInstances[socketInstance];
	if(!socket){
		return 3;
	}
	return socket.socket.readyState;
},

SocketError: function (socketInstance, ptr, bufsize)
{
 	var socket = webSocketInstances[socketInstance];
	
	if(!socket){
		return 0;
	}
	
 	if (socket.error == null){
 		return 0;
	}
		
    var str = socket.error.slice(0, Math.max(0, bufsize - 1));
	stringToUTF8(str, ptr, bufsize);
	return 1;
},

SocketSend: function (socketInstance, ptr)
{
	var socket = webSocketInstances[socketInstance];
	if(!socket){
		return;
	}
	var str = Pointer_stringify(ptr);
	socket.socket.send (str);
},

SocketRecv: function (socketInstance)
{
	var socket = webSocketInstances[socketInstance];
	if(!socket){
		return "";
	}
	
	if (socket.messages.length == 0)
		return "";
	
	var message = socket.messages[0];
	var buffer = _malloc(lengthBytesUTF8(message) + 1);
	stringToUTF8(message, buffer, lengthBytesUTF8(message) + 1);
	
	socket.messages = socket.messages.slice(1);	
	return buffer;
},

SocketClose: function (socketInstance)
{
	var socket = webSocketInstances[socketInstance];
	if(!socket){
		return ;
	}
	socket.socket.close();
}
};

autoAddDeps(LibraryWebSockets, '$webSocketInstances');
mergeInto(LibraryManager.library, LibraryWebSockets);
