var contact = ( function() {
    var url = {
      update : '/user/contact/update/'
    };

    function func_save() {
      var data = get_data();
      $('#save-form input[type="text"]').next().remove();
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
        address_1 : form.find('#id_address_1').val(),
        address_2 : form.find('#id_address_2').val(),
        address_3 : form.find('#id_address_3').val(),
        city : form.find('#id_city').val(),
        state : form.find('#id_state').val(),
        postcode : form.find('#id_postcode').val(),
        country : form.find('#id_country').val(),
        home_phone : form.find('#id_home_phone').val(),
        mobile_phone : form.find('#id_mobile_phone').val(),
        work_email : form.find('#id_work_email').val(),
        other_email : form.find('#id_other_email').val()
      };

      return {
        employee_contact : data
      };
    }

    function init() {
      $('.save_button.save').click(func_save);
      $('#save-form').tooltip({track : true});
      utils.bind_hover($('.save_button'));
      utils.init_alert_dialog('#dialog-message');
    }

    function load() {
      return menu.get('/user/contact/', init);
    }

    return {
      load : load
    };
}()); 