var utils = ( function() {
    var typing_timer = null;
    var done_typing_interval = 2000;
    var date_format = 'dd-mm-yy';

    /**
     * @public
     * This function makes the selected id to appear as popup dialog to show alert message.
     * @param id The element id.
     */
    function init_alert_dialog(id) {
      $(id).dialog({
        autoOpen : false,
        modal : true,
        buttons : {
          OK : function() {
            $(this).dialog('close');
          }
        }
      });
    }

    /**
     * @public
     * This function makes the element error-dialog to appear as popup dialog.
     * It also attach the ajaxError event to monitor ajax error.
     */
    function init_server_error_dialog() {
      $(document).ajaxError(function(evt, jqXHR, ajaxOptions, errorThrown) {
        show_error_dialog(jqXHR.responseText);
      });
      $('#error-dialog').dialog({
        autoOpen : false,
        modal : true,
        width : 700,
        height : 500,
        buttons : {
          OK : function() {
            $(this).dialog('close');
          }
        }
      });
    }

    /**
     * @public
     * This function monitor the progress of an ajax request by showing the progress Loading ...
     */
    function init_progress() {
      $(document).ajaxSend(function(evt, jqXHR, ajaxOptions) {
        $('#progress_status').show();
      });
      $(document).ajaxComplete(function() {
        $('#progress_status').hide();
      });
    }

    /**
     * @public
     * This function removes a dialog specified by the dialog id.
     * @param id The dialog id.
     */
    function remove_dialog(id) {
      var o = $(id);
      try {
        o.dialog('destroy');
      }
      
      catch (e) {}
      
      o.remove();
    }

    /**
     * @public
     * This function removes all the dialogs from the DOM.
     */
    function clear_dialogs() {
      remove_dialog("div[id^='dialog-message']");
      remove_dialog("div[id^='dialog']");
    }

    /**
     * @public
     * This function shows a message in a popup dialog.
     * @param arg The parameter to determine whether to show html (1) or plain text (2) message.
     * @param msg The message.
     */
    function show_dialog(arg, msg) {
      if (arg == 1)
        $('#dialog_msg').html(msg);
        
      else
        $('#dialog_msg').text(msg);

      $('#dialog-message').dialog('open');
    }

    /**
     * @public
     * This function shows the error message in a popup dialog.
     * @param msg The error message.
     */
    function show_error_dialog(msg) {
      $('#error_dialog').html(msg);
      $('#error_dialog style').remove();
      $('#error-dialog').dialog('open');
    }

    /**
     * @public
     * This function binds the hover event to the selected object.
     * @param selector The object selected using $
     */
    function bind_hover(selector) {
      selector.hover(function() {
        $(this).toggleClass('ui-state-hover');
      }, function() {
        $(this).toggleClass('ui-state-hover');
      });
    }

    /**
     * @public
     * This function binds the hover event to the selected list.
     * @param selector The object selected using $
     */
    function bind_hoverlist(selector) {
      selector.hover(function() {
        $(this).toggleClass('ui-state-highlight hover');
      }, function() {
        $(this).toggleClass('ui-state-highlight hover');
      });
    }

    /**
     * @public
     * This function executes the specified function when the typing interval has elapsed.
     * @param func The function to be executed.
     */
    function countdown_filter(func) {
      stop_filter_timer();
      typing_timer = setTimeout(func, done_typing_interval);
    }

    /**
     * @public
     * This function stops the typing timer.
     */
    function stop_filter_timer() {
      clearTimeout(typing_timer);
    }

    /**
     * @public
     * This function returns the id from a given element id.
     * e.g if the element id is item_1,
     * the id will be 1
     * @param arg The element id.
     * @return The id.
     */
    function get_itemid(arg) {
      var i = arg.indexOf('_');
      if (i >= 0) {
        var s = arg.substr(i + 1);
        return s;
      }

      return null;
    }
    
    /**
     * @public
     * This function enable/disable an element.
     * @param id The element id.
     * @param arg The parameter to enable (0) or disable (1) the element.
     * @param handler The function to be attached to the click event.
     */
    function set_disabled(id, arg, handler) {
      var o = $(id);
      o.unbind('click');
      o.unbind('mouseenter');
      o.unbind('mouseleave');
      if (arg == 1) {
        o.attr('disabled', 'disabled');
        o.removeClass('hover ui-state-hover');
        o.addClass('ui-state-disabled');
      }
      
      else {
        o.removeAttr('disabled');
        o.removeClass('ui-state-disabled');
        o.addClass('hover');
        o.click(handler);
        bind_hover(o);
      }
    }
    
    /**
     * @public
     * This function replace a string.
     * @param s The string to be replaced.
     * @param a The string to be replaced.
     * @param b The replacement string.
     * @return The replaced string.
     */
    function safe_replace(s, a, b) {
      if (s == null)
        return s;
        
      return s.replace(a, b);
    }
    
    /**
     * @public
     * This function returns the options for datepicker. 
     */
    function date_opt() {
      return {
        dateFormat : date_format,
        changeMonth : true,
        changeYear : true
      };
    }

    return {
      init_alert_dialog : init_alert_dialog,
      init_server_error_dialog : init_server_error_dialog,
      init_progress : init_progress,
      remove_dialog : remove_dialog,
      clear_dialogs : clear_dialogs,
      show_dialog : show_dialog,
      show_error_dialog : show_error_dialog,
      bind_hover : bind_hover,
      bind_hoverlist : bind_hoverlist,
      countdown_filter : countdown_filter,
      stop_filter_timer : stop_filter_timer,
      get_itemid : get_itemid,
      set_disabled : set_disabled,
      safe_replace : safe_replace,
      date_format : date_format,
      date_opt : date_opt
    };
}());