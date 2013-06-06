var menu = ( function() {

    function menu_click() {
      $('#menu a').removeClass('menu_active');
      $(this).addClass('menu_active');
    }

    function get(url, func) {
      utils.clear_dialogs();
      $('#contentcolumn div.innertube').load(url, func);
      return false;
    }

    function init() {
      $('#menu').accordion({
        autoHeight : false,
        animated : false
      });
      $('#menu a').click(menu_click);
    }

    return {
      get : get,
      init : init
    };
}());