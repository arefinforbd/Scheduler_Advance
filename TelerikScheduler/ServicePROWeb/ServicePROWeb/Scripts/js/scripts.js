function updatePieChart(element) {
    var mrefreshinterval = 600; // update display every 500ms
    var min = 0;
    var max = 100;
    var parent = $(element).parent();

    var mdraw = function() {
      var random = Math.floor(Math.random() * (max - min + 1)) + min;
      $(element).data('easyPieChart').update(random);
      $('.chart-data', parent).html(random + '%');
    }

    // We could use setInterval instead, but I prefer to do it this way
    //setInterval(mdraw, mrefreshinterval);
} // updatePiechart

function updateProgressBar(element) {
    var mrefreshinterval = 1200; // update display every 500ms
    var min = 0;
    var max = 100;
    var progress_bar = $('.progress-bar', element);

    var mdraw = function() {
      var random = Math.floor(Math.random() * (max - min + 1)) + min;

      $(progress_bar).removeClass('progress-bar-danger');
      $(progress_bar).removeClass('progress-bar-warning');
      $(progress_bar).removeClass('progress-bar-success');
      if (random > 50 && random < 75) {
        $(progress_bar).addClass('progress-bar-warning');
      } else if (random > 75) {
        $(progress_bar).addClass('progress-bar-danger');
      } else {
        $(progress_bar).addClass('progress-bar-success');
      }


      $(progress_bar).attr('aria-valuenow', random);
      $(progress_bar).css('width', random + '%');
      $('.progress-bar-percent', element).html(random);
    }

    // We could use setInterval instead, but I prefer to do it this way
    setInterval(mdraw, mrefreshinterval);
} // updateProgressBar

function random(){
  var value = [];

  for(var i=0;i<=15;i++) {
    value.push(Math.round(Math.floor((Math.random() * 100) + 1)));
  }

  return value;
} // random

jQuery(document).ready(function($){
  
  
  // Collapsed sidebar child
  $(document).on('click', '.collapsed-sidebar .menu li.parent>a', function(e){
    
    var li = $(this).parent();
    $(li).toggleClass('active');
    $('ul.child', li).toggleClass('open');
    
    e.preventDefault();
    return false;
  });
  
  // Display menu children if it has some
  $('li.parent>a', 'ul.menu').on('click', function(e){
    
    if (!$('body').hasClass('collapsed-sidebar')) {
      var link = $(this);
      var obj = $(this).parent();
      var child = $('ul.child', obj);

      if ($(child).is(':visible')) {
        $(child).slideUp(400);
        $(link).removeClass('close-child');
      } else {
        $(child).slideDown(400);
        $(link).addClass('close-child');
      }
      e.preventDefault();
    return false;
    }
  });
  
  // Panel Actions
  $('.action-minimize').on('click', function(){
    var link = $(this);
    var box = $(this).parents('.panel');
    var content = $('.panel-body', box);
    if ($(content).is(':visible')) {
      $(content).slideUp(500);
      $('i', link).removeClass('fa-minus').addClass('fa-plus');
    } else {
      $(content).slideDown(500);
      $('i', link).removeClass('fa-plus').addClass('fa-minus');
    }
    return false;
  });

  $('.action-remove').on('click', function(){
    var link = $(this);
    var box = $(this).parents('.panel');
    var content = $('.panel-body', box);
    $(box).slideUp();
    return false;
  });

  $('.action-colorize').on('click', function(){
    var link = $(this);
    var box = $(link).parents('.panel');
    var content = $('.panel-body', box);

    $('.color-picker', box).slideToggle();

    return false;
  });

  // ColorPicker Per Box!!
  $('.panel .color-picker li a').on('click', function(){
    var parent = $(this).parents('.panel');
    var color = $(this).data('style');
    var selected = $('.panel .color-picker li a[data-selected="true"]').data('style');

    $('.panel .color-picker li a[data-selected="true"]').removeAttr('data-selected');
    $(this).attr('data-selected', 'true');

    $('.panel-heading', parent).attr('class', 'panel-heading');
    $('.panel-heading', parent).attr('data-color', color);
    $('.panel-heading', parent).removeClass(selected).addClass(color);
    return false;
  });

  // ToDo List
  if ($('.todo-list').length) {
    $( ".todo-list, .todo-completed" ).sortable({
      connectWith: ".connectedSortable"
    }).disableSelection();
  }

  // iCheck
  $('input[type="checkbox"],input[type="radio"]').each(function(i, item) {
    var type = $(item).data('type');
    if (!type || type == '') type = 'flat';
    
    var color = $(item).data('color');
    if (!color || color == '') color = 'blue';
    
    if (type == 'line') {
      
      var self = $(item),
      label = self.next(),
      label_text = label.text();

      label.remove();
      self.iCheck({
        checkboxClass: 'icheckbox_line-' + color,
        radioClass: 'iradio_line-' + color,
        insert: '<div class="icheck_line-icon"></div>' + label_text
      });
      
    } else {
    
      $(item).iCheck({
        checkboxClass: 'icheckbox_' + type + '-' + color,
        radioClass: 'iradio_' + type + '-' + color,
      });
      
    }
  });

  // Spacer
  $('.spacer').each(function(i, item){
    $(item).css('height', $(item).data('height'));
  });

  // Left Sidebar Collapse
  $('.sidebar-collapse').on('click', function(){
    $('body').toggleClass('sidebar-collapsed');
    return false;
  });

  // Toggle Right Sidebar
  $('.toggle-right-sidebar').on('click', function(){
    $('.main-container,.top-bar').toggleClass("right-sidebar-view");
    $('.right-sidebar').toggleClass("in-view");
    $('.left-sidebar,.logo-container').toggleClass("out-view");
    return false;
  });

  // Toggle Right Sidebar - Pull
  $('.sidebar-pull-arrow>a').on('click', function(){
    $(this).toggleClass('close');
    $('.main-container,.top-bar').toggleClass("right-sidebar-view");
    $('.right-sidebar').toggleClass("in-view");
    $('.left-sidebar,.logo-container').toggleClass("out-view");

    return false;
  });

  // Toggle - Top Bar - User Profile - Dropdown
  $('.user-profile>a').on('click', function(){
    $('.user-profile-dropdown-container').toggleClass('appear');
    return false;
  });

  

  // Activate Chat
  $('.chat-icon>a').on('click', function(){
    $('#chat-box').toggleClass('visible');
    return false;
  });

  // Close Chat
  $('.close-chat-box').on('click', function(){
    $('#chat-box').toggleClass('visible');
    return false;
  });

  // Send chat
  $('.chat-box').submit(function(){
    var form = $(this);
    var value = $('input', form).val();
    $('input', form).val('');

    var message = $('li:first-child', '.chat-list').clone();
    $(message).find('p').html(value);
    $('.chat-list').append(message);

    return false;
  });

  // Dashboard Sparkline Top
  if ($('.flot-dashboard-demo-1').length) {
    //$('.flot-dashboard-demo-1')
  }

  // Mini Header - Sparkline
  if ($("#sparkline-mini-01").length) {
  $("#sparkline-mini-01").sparkline(
  random(),
  {
    type: 'bar',
    height: '25px',
    barWidth: 5,
    barSpacing: 2,
    zeroAxis: false,
    barColor: '#e3e3e6',
    negBarColor: '#07a8d7'
  });
  }


  if ($("#sparkline-mini-02").length) {
  $("#sparkline-mini-02").sparkline(
  random(),
  {
    type: 'bar',
    height: '25px',
    barWidth: 5,
    barSpacing: 2,
    zeroAxis: false,
    barColor: '#e3e3e6',
    negBarColor: '#07a8d7'
  });
  }

  // Easy Pie chart
  if ($('#easy-pie-chart-01').length) {
  $('#easy-pie-chart-01').easyPieChart({
    barColor: '#65cea7',
    scaleColor: false,
    trackColor:false,
    scaleLength:0
  });
  }

  if ($('#easy-pie-chart-02').length) {
  $('#easy-pie-chart-02').easyPieChart({
    barColor: '#65cea7',
    scaleColor: false,
    trackColor:false,
    scaleLength:0
  });
  }

  // Progress Bar Update
  updateProgressBar('#cpu-load-bar-01');
  updateProgressBar('#cpu-load-bar-02');

  // Morris Chart
  if ($('#hero-area').length) {
  var revenue_chart = Morris.Area({
    element: 'hero-area',
    data: [
      {period: '2013 Q1', iphone: 2666, ipad: null, itouch: 2647},
      {period: '2013 Q2', iphone: 2778, ipad: 2294, itouch: 2441},
      {period: '2013 Q3', iphone: 4912, ipad: 1969, itouch: 2501},
      {period: '2013 Q4', iphone: 3767, ipad: 3597, itouch: 5689},
      {period: '2014 Q1', iphone: 6810, ipad: 1914, itouch: 2293},
      {period: '2014 Q2', iphone: 5670, ipad: 4293, itouch: 1881},
      {period: '2014 Q3', iphone: 3670, ipad: 2593, itouch: 1381},
      {period: '2014 Q4', iphone: 7670, ipad: 2293, itouch: 2081},
    ],
    xkey: 'period',
    ykeys: ['iphone', 'ipad', 'itouch'],
    labels: ['iPhone', 'iPad', 'iPod Touch'],
    pointSize: 2,
    hideHover: 'auto',
    lineColors: ['#07a8d7', '#32323a', '#27c24c'],
    pointStrokeColors: ['#e7e7e7']
  });
  }

  // WYSIWYG
  if ($('.quick-post-message').length) {
  $('.quick-post-message').summernote({height:150});
  }

  // Prevent Dropdown menu close
  $('.header-bar .icon-list .dropdown-menu input, .header-bar .icon-list .dropdown-menu label').click(function(e) {
    e.stopPropagation();
  });
  
  $('.collapse-sidebar').on('click', function(){
      var body = $('body');
      $(body).toggleClass('collapsed-sidebar');
      return false;
  });
  
  
  $('.modal-add-task').submit(function(){
    var form = $(this);
    var desc = $('textarea[name="task-description"]', form).val();
    var task_for = $('select[name="task-for"]', form).val();

    var recent_tasks = $('.timeline-container.recent-tasks');

    var clone = $('.timeline-row:first-child', recent_tasks).clone();

    $(clone).find('.content.left .avatar>img').attr('src', '_demo/users/10.jpg');
    $(clone).find('.content.left .text-muted').html(desc);
    $(clone).find('.timeline-panel .avatar>img').attr('src', '_demo/users/0' + task_for + '.jpg');
    $(clone).find('.timeline-panel .notification').attr('class', 'notification pending').html('Pending');

    $(recent_tasks).prepend('<div class="timeline-row" style="display:none;">' + $(clone).html() + '</div>');

    $('.timeline-row:first-child', recent_tasks).slideDown(1000);

    $('#new-task-modal').modal('hide');

    return false;
  });
  
  
  // Various resize actions
  jQuery(window).resize(function(){
  
    if (jQuery(window).width() > 980) {
      $('ul.menu', '.sidebar').show();
    }
    
    if (typeof revenue_chart != 'undefined') 
    revenue_chart.redraw();
    
    
    
  });

});

