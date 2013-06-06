var theme = ( function() {
    var default_theme = 'darkhive';
  
    function init() {
      var select_close = $('#theme_option .cancelicon');
      select_close.click(function() {
        $('#theme_option').slideUp();
      });
    
      $.themes.init({
        themes : ['blitzer', 'darkhive', 'trontastic', 'humanity'],
        defaultTheme : default_theme,
        onSelect : reload_IE
      });
      $('#theme_body').themes();
      $("#main_options").click(show_options);
      utils.bind_hover($("#main_options"));
    }
  
    function show_options() {
      $('#theme_option').slideToggle();
    }
  
    function current_theme() {
      return $.themes.currentTheme;
    }
  
    function reload_IE(id, display, url) {
    }
  
    return {
      init : init,
      current_theme : current_theme
    };
}());