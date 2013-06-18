var payslip = ( function() {
    var url = {
      salaryslip : '/user/payslip/slip/'
    };
    
    function func_generate() {
      var month = $('#id_month').val();
      var year = $('#id_year').val();
      var f = 'width=800,height=400,menubar=1';
      open(url.salaryslip + month + '/' + year, '_blank', f);
      return false;
    }

    function init() {
      $('#id_gen').click(func_generate);
      utils.init_alert_dialog('#dialog-message');
      utils.bind_hover($('#id_gen'));
    }

    function load() {
      return menu.get('/user/payslip/', init);
    }
    
    return {
      load : load
    };
}()); 