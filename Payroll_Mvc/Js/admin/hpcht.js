var hpcht = ( function() {
    var url = {
      data : '/admin/hourly/chart/data/'
    };

    // Radialize the colors
    Highcharts.getOptions().colors = $.map(Highcharts.getOptions().colors, function(color) {
      return {
        radialGradient : {
          cx : 0.5,
          cy : 0.3,
          r : 0.7
        },
        stops : [[0, color], [1, Highcharts.Color(color).brighten(-0.3).get('rgb')] // darken
        ]
      };
    });
    
    function chart1(result) {
      // Build the chart
      var chart = new Highcharts.Chart({
        chart : {
          renderTo : 'cht1',
          plotBackgroundColor : null,
          plotBorderWidth : null,
          plotShadow : false
        },
        title : {
          text : result.title
        },
        tooltip : {
          pointFormat : '{series.name}: <b>{point.percentage}%</b>',
          percentageDecimals : 1
        },
        plotOptions : {
          pie : {
            allowPointSelect : true,
            cursor : 'pointer',
            dataLabels : {
              enabled : true,
              color : '#000000',
              connectorColor : '#000000',
              formatter : function() {
                return '<b>' + this.point.name + '</b>: ' + this.percentage.toFixed(2) + ' %';
              }
            }
          }
        },
        series : [{
          type : 'pie',
          name : result.title,
          data : result.pie
        }]
      });
    }

    function chart2(result) {
      var chart = new Highcharts.Chart({
        chart : {
          renderTo : 'cht2',
          type : 'column'
        },
        title : {
          text : result.title
        },
        subtitle : {
          text : ''
        },
        xAxis : {
          categories : result.column.categories
        },
        yAxis : {
          min : 0,
          title : {
            text : result.column.yaxis
          }
        },
        legend : {
          layout : 'vertical',
          backgroundColor : '#FFFFFF',
          align : 'left',
          verticalAlign : 'top',
          x : 100,
          y : 70,
          floating : true,
          shadow : true
        },
        tooltip : {
          formatter : function() {
            return '' + this.x + ': ' + this.y;
          }
        },
        plotOptions : {
          column : {
            pointPadding : 0.2,
            borderWidth : 0
          }
        },
        series : [{
          name : 'Hourly Payroll',
          data : result.column.data,
          showInLegend : false
        }]
      });
    }
    
    function draw_chart() {
      var data = get_search_param();
      $.post(url.data, data, function(result) {
        chart1(result);
        chart2(result);
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
        staff_id : $('#id_staff_id').val(),
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
      $('#id_staff_id').tooltip({track: true});
      utils.init_alert_dialog('#dialog-message');
      utils.bind_hover($('#id_gen'));
      draw_chart();
    }

    function load() {
      menu.get('/admin/hourly/chart/', init);
    }

    return {
      load : load
    };
}());