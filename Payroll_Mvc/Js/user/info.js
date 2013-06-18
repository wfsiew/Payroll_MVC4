var info = ( function() {
    var url = {
      update : '/user/info/update/'
    };

    function func_save() {
      var data = get_data();
      $('#save-form input[type="text"]').next().remove();
      $('#save-form select').next().remove();
      $('#save-form').find('#lb_gender_f').next().remove();
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
                url : '/Js/tpl/label_error.html',
                ext : '.html'
              }).render(o);
              if (e == 'gender')
                $('#save-form #lb_gender_f').after(h);
              
              else if (e == 'marital_status')
                $('#save-form #id_marital_status').after(h);
              
              else
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
        first_name : form.find('#id_first_name').val(),
        middle_name : form.find('#id_middle_name').val(),
        last_name : form.find('#id_last_name').val(),
        new_ic : form.find('#id_new_ic').val(),
        old_ic : form.find('#id_old_ic').val(),
        passport_no : form.find('#id_passport_no').val(),
        gender : form.find("input:radio[name='gender']:checked").val(),
        marital_status : form.find('#id_marital_status').val(),
        nationality : form.find('#id_nationality').val(),
        dob : form.find('#id_dob').val(),
        place_of_birth : form.find('#id_place_of_birth').val(),
        race : form.find('#id_race').val(),
        religion : form.find('#id_religion').val(),
        is_bumi : form.find('#id_is_bumi').prop('checked')
      };

      return {
        employee : data
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
      return menu.get('/user/info/', init);
    }

    return {
      load : load
    };
}()); 