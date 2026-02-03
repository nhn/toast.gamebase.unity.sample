mergeInto(LibraryManager.library, {

OpenBrowser: function(url)
{
	var openUrl = UTF8ToString(url);
	
	window.open(openUrl, "_blank");
},

});