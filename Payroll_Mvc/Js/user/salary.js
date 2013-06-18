var salary = ( function() {

    function init() {
      utils.init_alert_dialog('#dialog-message');
    }

    function load() {
      return menu.get('/user/salary/', init);
    }

    return {
      load : load
    };
}());