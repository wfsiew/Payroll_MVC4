var att = ( function() {
    var url = {
      list : '/admin/att/list/'
    };
    
    function get_search_param() {
      var param = {
        work_date : $('#id_work_date').val(),
        employee : $('#id_employee').val()
      };
      
      return param;
    }
    
    function sort_list() {
      var s = sort.set_sort_css($(this));
      nav_list.set_sort(s);
    }
    
    function init_list() {
      $('.sortheader').click(sort_list);
    }

    function init() {
      $('.date_input').datepicker(utils.date_opt());
      $('#id_find').click(nav_list.show_list);
      $('#id_display').change(nav_list.show_list);
      $('#id_employee').tooltip({track: true});
      utils.init_alert_dialog('#dialog-message');
      utils.bind_hover($('#id_find'));
      nav_list.config.list_url = url.list;
      nav_list.config.list_func = init_list;
      nav_list.config.search_param_func = get_search_param;
      nav_list.init();
    }
    
    function load() {
      return menu.get('/admin/att/', init);
    }
    
    return {
      load : load
    };
}()); 