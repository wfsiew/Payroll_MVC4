var wkcht = ( function() {
    var url = {
      data : '/user/workhours/chart/data/'
    };

    function chart1(result) {
      var chart = new Highcharts.Chart({
        chart : {
          renderTo : 'cht1',
          type : 'line'
        },
        title : {
          text : result.title
        },
        subtitle : {
          text : ''
        },
        xAxis : {
          categories : result.categories
        },
        yAxis : {
          title : {
            text : result.yaxis
          },
          plotLines : [{
            value : 0,
            width : 1,
            color : '#808080'
          }]
        },
        tooltip : {
          formatter : function() {
            return '' + this.x + ': ' + this.y + ' hours';
          }
        },
        plotOptions : {
          line : {
            dataLabels : {
              enabled : true
            }
          }
        },
        legend : {
          layout : 'vertical',
          align : 'right',
          verticalAlign : 'top',
          x : -10,
          y : 100,
          borderWidth : 0
        },
        series : [{
          name : 'Hours Worked',
          data : result.data,
          showInLegend : false
        }]
      });
    }
    
    function draw_chart() {
      var data = get_search_param();
      $.post(url.data, data, function(result) {
        chart1(result);
      });
    }
    
    function get_checked_month() {
      if ($('.chkall').attr('checked') == 'checked')
        return 0;
        
      else {
        var a = [];
        $('.chkmonth:checked').each(function(idx, elm) {
          a.push($(this).val());
        });
        return (a == [] ? 0 : a);
      }
    }
    
    function get_search_param() {
      var month = get_checked_month();
      
      var param = {
        year : $('#id_year').val()
      };
      if (month == 0)
        param['month'] = 0;
        
      else
        param['month[]'] = month;
      
      return param;
    }
    
    function uncheck_all_month() {
      var a = $(this).attr('checked');
      if (a == 'checked')
        $('.chkmonth').removeAttr('checked');
    }
    
    function uncheck_all() {
      var a = $(this).attr('checked');
      if (a == 'checked')
        $('.chkall').removeAttr('checked');
    }

    function init() {
      $('#id_gen').click(draw_chart);
      $('.chkall').click(uncheck_all_month);
      $('.chkmonth').click(uncheck_all);
      utils.init_alert_dialog('#dialog-message');
      utils.bind_hover($('#id_gen'));
      draw_chart();
    }

    function load() {
      return menu.get('/user/workhours/chart/', init);
    }

    return {
      load : load
    };
}());