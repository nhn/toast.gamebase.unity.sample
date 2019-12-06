mergeInto(LibraryManager.library, {

Alert: function(title, message)
{
	var alertTitle = Pointer_stringify(title);
	var alertMessage = Pointer_stringify(message);

	console.log("title = ", alertTitle, " message = ", alertMessage);

	alert(alertTitle + "\n\n" + alertMessage);
},

Confirm: function(title, message)
{
	var alertTitle = Pointer_stringify(title);
	var alertMessage = Pointer_stringify(message);

	console.log("title = ", alertTitle, " message = ", alertMessage);

	return confirm(alertTitle + "\n\n" + alertMessage);
},

});