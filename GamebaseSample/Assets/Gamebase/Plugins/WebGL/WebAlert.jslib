mergeInto(LibraryManager.library, {

Alert: function(title, message)
{
	var alertTitle = UTF8ToString(title);
	var alertMessage = UTF8ToString(message);

	console.log("title = ", alertTitle, " message = ", alertMessage);

	alert(alertTitle + "\n\n" + alertMessage);
},

Confirm: function(title, message)
{
	var alertTitle = UTF8ToString(title);
	var alertMessage = UTF8ToString(message);

	console.log("title = ", alertTitle, " message = ", alertMessage);

	return confirm(alertTitle + "\n\n" + alertMessage);
},

});