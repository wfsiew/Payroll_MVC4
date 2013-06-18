var job = ( function() {

    function init() {
      utils.init_alert_dialog('#dialog-message');
    }

    function load() {
      return menu.get('/user/job/', init);
    }

    return {
      load : load
    };
}()); 