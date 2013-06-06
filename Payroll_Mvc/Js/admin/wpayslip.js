var wpayslip = ( function() {
    function init() {
      utils.bind_hover($('.print_button'));
      $('.print_button').click(function() {
        print();
      });
      var w = opener;
      var _theme = w.theme.current_theme();
      $.themes.init({defaultTheme : _theme});
    }

    return {
      init : init
    };
}());

$(document).ready(wpayslip.init);