var qualification = ( function() {
    var url = {
      update : '/user/qualification/update/'
    };

    function func_save() {
      var data = get_data();
      $('#save-form input[type="text"]').next().remove();
      $('#save-form select').next().remove();
      $.post(url.update, data, function(result) {
        if (result.success == 1)
          stat.show_status(0, result.message);
        
        else if (result.error == 1) {
          for (var e in result.errors) {
            var d = $('#save-form #error_' + e).get(0);
            if (!d) {
              var o = {
                field : e,
                msg : result.errors[e][0]
              };
              var h = new EJS({
                url : '/Js/tpl/label_error_inline.html',
                ext : '.html'
              }).render(o);
              $("#save-form input[name='" + e + "']").after(h);
            }
          }
        }
        
        else
          utils.show_dialog(2, result);
      });

      return false;
    }

    function get_data() {
      var form = $('#save-form');

      var data = {
        level : form.find('#id_level').val(),
        institute : form.find('#id_institute').val(),
        major : form.find('#id_major').val(),
        year : form.find('#id_year').val(),
        gpa : form.find('#id_gpa').val(),
        start_date : form.find('#id_start_date').val(),
        end_date : form.find('#id_end_date').val()
      };

      return {
        employee_qualification : data
      };
    }

    function init() {
      $('.date_input').datepicker(utils.date_opt());
      $('.save_button.save').click(func_save);
      $('#save-form').tooltip({track : true});
      utils.bind_hover($('.save_button'));
      utils.init_alert_dialog('#dialog-message');
    }

    function load() {
      return menu.get('/user/qualification/', init);
    }

    return {
      load : load
    };
}()); 