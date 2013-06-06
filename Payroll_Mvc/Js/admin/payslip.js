var payslip = ( function() {
    var url = {
      list : '/admin/payslip/list/',
      salaryslip : '/admin/payslip/slip/'
    };
    
    function func_generate(id) {
      if (!id)
        return false;
        
      id = utils.get_itemid(id);
      var month = $('#id_month').val();
      var year = $('#id_year').val();
      var f = 'width=800,height=400,menubar=1';
      open(url.salaryslip + id + '/' + month + '/' + year, '_blank', f);
      return false;
    }
  
    function get_search_param() {
      var param = {
        employee : $('#id_employee').val(),
        staff_id : $('#id_staff_id').val(),
        employment_status : $('#id_employment_status').val(),
        designation : $('#id_designation').val(),
        dept : $('#id_dept').val()
      };
      
      return param;
    }
  
    function sort_list() {
      var s = sort.set_sort_css($(this));
      nav_list.set_sort(s);
    }

    function init_list() {
      utils.bind_hoverlist($('.list_table tbody tr'));
      $('.list_table tbody').selectable({
        selected : function(evt, ui) {
          var id = ui.selected.id;
          func_generate(id);
        }
      });
      $('.sortheader').click(sort_list);
    }

    function init() {
      $('#id_find').click(nav_list.show_list);
      $('#id_display').change(nav_list.show_list);
      $('#id_employee,#id_staff_id').tooltip({track : true});
      utils.init_alert_dialog('#dialog-message');
      utils.bind_hover($('#id_find'));
      nav_list.config.list_url = url.list;
      nav_list.config.list_func = init_list;
      nav_list.config.search_param_func = get_search_param;
      nav_list.init();
    }

    function load() {
      return menu.get('/admin/payslip/', init);
    }
    
    return {
      load : load
    };
}()); 