var app = ( function() {

    function init() {
      menu.init();
      utils.init_progress();
      utils.init_server_error_dialog();
      theme.init();
      utils.bind_hover($('#logout'));
      $('#menu_info').addClass('menu_active');
      info.load();
    }

    return {
      init : init
    };
}());

$(app.init);