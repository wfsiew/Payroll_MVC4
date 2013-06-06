var stat = ( function() {
    var info = 'ui-state-highlight ui-corner-all';
    var info_icon = 'ui-icon ui-icon-check';
    var error = 'ui-state-error ui-corner-all';
    var error_icon = 'ui-icon ui-icon-alert';
    var status_timer = null;

    function show_status(arg, msg) {
      clearTimeout(status_timer);
      var id = '#status-panel';
      var p = '#status_msg';
      var opt = {};
      if (arg == 0) {
        set_info_class(id);
        $(p).html(msg);
        $(id).show('drop', opt, 500, function() {
          status_timer = setTimeout(function() {
            $(id).hide('scale', opt, 500, null);
          }, 5000);
        });
      }
      
      else {
        set_error_class(id);
        $(p).html(msg);
        $(id).show();
      }
    }

    function set_info_class(id) {
      var div = id + '_outer';
      var span = id + '_inner';
      remove_status_class(div, span);
      $(div).addClass(info);
      $(span).addClass(info_icon);
    }

    function set_error_class(id) {
      var div = id + '_outer';
      var span = id + '_inner';
      remove_status_class(div, span);
      $(div).addClass(error);
      $(span).addClass(error_icon);
    }

    function remove_status_class(div, span) {
      if ($(div).hasClass(info)) {
        $(div).removeClass(info);
        $(span).removeClass(info_icon);
      }

      if ($(div).hasClass(error)) {
        $(div).removeClass(error);
        $(span).removeClass(error_icon);
      }
    }

    return {
      show_status : show_status
    };
}()); 