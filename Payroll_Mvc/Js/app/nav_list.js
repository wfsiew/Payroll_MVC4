var nav_list = ( function() {
    var config = {
      list_url : '',
      list_func : null,
      del_func : null,
      save_func : null,
      search_param_func : null
    };

    /**
     * @public
     * This function shows the list, and reset back the page number to 1.
     */
    function show_list() {
      $('#id_display').data('pgnum', 1);
      utils.stop_filter_timer();
      update_list();
    }

    /**
     * @public
     * This function reloads the list by using the same navigation parameters such as the
     * display size, current page no., and search parameters.
     */
    function update_list() {
      var pgsize = $('#id_display').val();
      var pgnum = $('#id_display').data('pgnum');
      var sort = get_sort();
      var param = ($.isFunction(config.search_param_func) ? config.search_param_func() : get_search_param());
      param['pgnum'] = pgnum;
      param['pgsize'] = pgsize;
      
      if (sort != null) {
        param['sortcolumn'] = sort['column'];
        param['sortdir'] = sort['dir'];
      }
      
      $.post(config.list_url, param, function(result) {
        $('#right_box').html(result);
        init_navigate();
      });
    }

    /**
     * @private
     * This function navigate the list to the previous page.
     */
    function go_prev() {
      var val = $('#id_pg').val();
      var arr = val.split(',');
      var d = $('#id_display').val();
      $('#id_display').data('pgnum', arr[2]);
      update_list();
    }

    /**
     * @private
     * This function navigate the list to the next page.
     */
    function go_next() {
      var val = $('#id_pg').val();
      var arr = val.split(',');
      var d = $('#id_display').val();
      $('#id_display').data('pgnum', arr[3]);
      update_list();
    }
    
    /**
     * @private
     * This function navigate the list to the specified page. 
     */
    function go_page() {
      var pg = $('#id_pagenum').val();
      var page = 1;
      if ($.trim(pg) != '' && $.isNumeric(pg))
        page = parseInt(pg);
        
      $('#id_display').data('pgnum', page);
      update_list();
    }

    /**
     * @public
     * This function handles the keypress event on the search textbox.
     * It checks for <enter> key.
     */
    function query_keypress(evt) {
      if (evt.keyCode == '13') {
        evt.preventDefault();
        evt.stopPropagation();
        show_list();
      }
    }

    /**
     * @public
     * This function handles the keyup event on the search textbox.
     * It checks for <enter> key.
     */
    function query_keyup(evt) {
      if (evt.keyCode != '13')
        utils.countdown_filter(show_list);
    }

    /**
     * @public
     * This function sets the item text,
     * i.e 1 to 2 of 2
     * @param arg The text.
     */
    function set_item_msg(arg) {
      $('.item_display').text(arg);
    }
    
    /**
     * @public
     * This function sets the sort info.
     * @param s The sort info in {}.
     */
    function set_sort(s) {
      $('#id_display').data('sort', s);
      update_list();
    }
    
    /**
     * @private
     * This function gets the current sort info in {}.
     * @return The sort info in {}.
     */
    function get_sort() {
      var s = $('#id_display').data('sort');
      return s;
    }

    /**
     * @private
     * This function initialize the navigation elements after the list has been loaded.
     * The initialization includes :<br>
     * 1. sets the page no. to 1<br>
     * 2. enable/disable the next/prev buttons<br>
     * 3. initialize the edit/delete/clone buttons
     */
    function init_navigate() {
      $('#id_display').data('pgnum', 1);
      if ($.isFunction(config.list_func))
        config.list_func();

      var val = $('#id_pg').val();
      var arr = val.split(',');
      if (arr[0] == '0') {
        utils.set_disabled('#id_prev', 1, null);
      }
      
      else {
        utils.set_disabled('#id_prev', 0, go_prev);
      }

      if (arr[1] == '0') {
        utils.set_disabled('#id_next', 1, null);
      }
      
      else {
        utils.set_disabled('#id_next', 0, go_next);
      }
      
      if (arr[0] == '1' || arr[1] == '1') {
        utils.set_disabled('#id_go', 0, go_page);
      }
      
      else if (arr[0] == '0' && arr[1] == '0') {
        utils.set_disabled('#id_go', 1, null);
      }
      
      if ($.isFunction(config.del_func)) {
        if ($('.list_table')[0] != null)
          utils.set_disabled('#id_delete', 0, config.del_func);
          
        else
          utils.set_disabled('#id_delete', 1, null);
      }
      
      if ($.isFunction(config.save_func)) {
        if ($('.list_table')[0] != null)
          utils.set_disabled('#id_save', 0, config.save_func);
          
        else
          utils.set_disabled('#id_save', 1, null);
      }
      
      $('#id_pagenum').val(arr[7]);
      
      sort.init_sort($('#hd_' + utils.safe_replace(arr[5], '.', '-')), arr[6]);
      set_item_msg(arr[4]);
    }

    /**
     * @private
     * This function returns the search parameters in {}.
     * @return The search paramaters in {}.
     */
    function get_search_param() {
      var id_selection = $('#id_selection');
      var keyword = $('#id_query').val();
      var param = {};
      if (id_selection[0] != null)
        param['find'] = id_selection.val();
        
      if (param['find'] == '0' && keyword == '')
        delete param['find']
        
      else
        param['keyword'] = keyword;
      
      return param;
    }

    function init() {
      init_navigate();
    }

    return {
      init : init,
      config : config,
      show_list : show_list,
      update_list : update_list,
      query_keypress : query_keypress,
      query_keyup : query_keyup,
      set_item_msg : set_item_msg,
      set_sort : set_sort
    };
}());