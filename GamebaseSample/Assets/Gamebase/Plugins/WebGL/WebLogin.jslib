mergeInto(LibraryManager.library, {

  OpenLoginBrowser: function (url) {
      if (typeof window === 'undefined') {
          console.error('OpenLoginBrowser: window is not defined. This function must be run in a browser.');
          return;
      }
      
      var urlString = UTF8ToString(url);
      try {
          var popup = window.open(urlString, "_blank", "toolbar=no,location=no,menubar=no,status=no,scrollbars=yes,resizable=yes");
          if (!popup) {
              console.error('Popup blocked or failed to open');
          }
      } catch (e) {
          console.error('Failed to open popup:', e);
      }
  }
});