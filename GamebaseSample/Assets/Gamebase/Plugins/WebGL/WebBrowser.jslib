mergeInto(LibraryManager.library, {

OpenBrowser: function(url)
{
	var openUrl = Pointer_stringify(url);
	
	window.open(openUrl, "_blank");
},

});