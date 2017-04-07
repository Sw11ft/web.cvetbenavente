/**
 * Main scripts file
 */
(function ($) {
  'use strict';
  /* Define some variables */
  var $window = $(window),
    app = $('.app'),
    offscreenDirection,
    offscreenDirectionClass,
    rapidClickCheck = false,
    isOffscreenOpen = false,
    psTarg = $('.no-touch .sidebar-panel');

  /* Preloader */
  function preloader() {
    $(window).on('load', function () {
      if ($('body > .pageload').length) {
        if ($('body').hasClass('page-loaded')) {
          return;
        }
        $('body').addClass('page-loaded').removeClass('page-loading');
        $('body > .pageload').fadeOut();
      }
    });
  }

  /* Scrollable areas with perfect scrollbar */
  function scrollable() {
    $('.scrollable').perfectScrollbar({
      wheelPropagation: true,
      suppressScrollX: true
    });
  }

  /* Small menu */
  function smallMenu() {
    $('[data-toggle=layout-small-menu]').on('click', function (e) {
      e.preventDefault();
      e.stopPropagation();
      app.toggleClass('layout-small-menu');
      if (app.hasClass('layout-small-menu')) {
        destroyScrollbars();
        if ($('.quick-launch-apps').is(':visible')) {
          $('.quick-launch-apps').addClass('hide').next().removeClass('hide');
        }
      } else if (!psTarg.hasClass('ps-container')) {
        initScrollbars();
      }
    });
  }

  /* Quick launch */
  function quickLaunch() {
    $('[data-toggle=quick-launch]').on('click', function (e) {
      e.preventDefault();
      if ($('.quick-launch-apps').is(':visible')) {
        $('.quick-launch-apps').addClass('hide').next().removeClass('hide');
      } else {
        $('.quick-launch-apps').removeClass('hide').next().addClass('hide');
      }
    });
  }

  /* Scroll to top */
  function scrollToTop() {
    $('.scroll-up').on('click', function (e) {
      e.preventDefault();
      e.stopPropagation();
      $('html,body').stop().animate({
        scrollTop: $('body').offset().top
      }, 1000);
      return false;
    });
  }

  /* Smooth scroll*/
  function smoothScroll() {
    $('.smooth-scroll').on('click', function (e) {
      e.preventDefault();
      e.stopPropagation();
      if (location.pathname.replace(/^\//, '') === this.pathname.replace(/^\//, '') && location.hostname === this.hostname) {
        var target = $(this.hash);
        target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
        if (target.length) {
          $('html,body').stop().animate({
            scrollTop: target.offset().top
          }, 1000);
          return false;
        }
      }
    });
  }

  /* active state */
  function activeState() {
    $('.toggle-active').on('click', function (e) {
      e.preventDefault();
      e.stopPropagation();
      $(this).toggleClass('active');
    });
  }

  /* Ripple efect */
  function rippleEffect() {
    $('.ripple').on('click', function (e) {
      e.preventDefault();
      var $div = $('<div/>'),
        btnOffset = $(this).offset(),
        xPos = e.pageX - btnOffset.left,
        yPos = e.pageY - btnOffset.top;
      $div.addClass('ripple-effect');
      var $ripple = $('.ripple-effect');
      $ripple.css('height', $(this).height());
      $ripple.css('width', $(this).height());
      $div.css({
        top: yPos - $ripple.height() / 2,
        left: xPos - $ripple.width() / 2,
        background: $(this).data('ripple-color')
      }).appendTo($(this));
      window.setTimeout(function () {
        $div.remove();
      }, 1500);
    });
  }

  function cardControls() {
    /* Card controls */
    $('[data-toggle=card-collapse]').on('click', function (e) {
      var parent = $(this).parents('.card'),
        body = parent.children('.card-block');

      if (body.is(':visible')) {
        parent.addClass('card-collapsed');
        body.slideUp(200);
      } else if (!body.is(':visible')) {
        parent.removeClass('card-collapsed');
        body.slideDown(200);
      }
      e.preventDefault();
      e.stopPropagation();
    });

    $('[data-toggle=card-refresh]').on('click', function (e) {
      var parent = $(this).parents('.card');

      parent.addClass('card-refreshing');
      window.setTimeout(function () {
        parent.removeClass('card-refreshing');
      }, 3000);
      e.preventDefault();
      e.stopPropagation();
    });

    $('[data-toggle=card-remove]').on('click', function (e) {
      var parent = $(this).parents('.card');

      parent.addClass('animated zoomOut');

      parent.bind('animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd', function () {
        parent.remove();
      });

      e.preventDefault();
      e.stopPropagation();
    });
  }

  function accordion() {
    /* Accordion UI Element */
    var accordionBody = $('.accordion > .accordion-container > .accordion-body'),
      accordionTitleTarget = $('.accordion > .accordion-container > .accordion-title > a');

    if ($('.accordion').length) {
      accordionBody.hide();

      $('.accordion').each(function () {
        $(this).find('.accordion-body').first().show();
        $(this).find('.accordion-container').first().addClass('active');
      });
    }

    accordionTitleTarget.on('click', function (e) {
      e.preventDefault();
      e.stopPropagation();

      var thisParent = $(this).parent();

      if (!$(this).closest('.accordion').hasClass('accordion-toggle')) {
        $(this).closest('.accordion').find('.accordion-body').slideUp();
        $(this).closest('.accordion').find('.accordion-container').removeClass('active');
      }

      if (thisParent.next().css('display') !== 'block') {
        thisParent.next().slideDown();
        thisParent.parent().addClass('active');

        return false;
      } else if (thisParent.next().css('display') === 'block') {

        thisParent.next().slideUp();
        thisParent.parent().removeClass('active');

        return false;
      }
      return false;

    });
  }

  /* Sidebar panel */
  function sidebarPanel() {
    $('[data-toggle=offscreen]').on('click', function (e) {
      e.preventDefault();
      e.stopPropagation();
      offscreenDirection = $(this).data('move') ? $(this).data('move') : 'ltr';
      if (offscreenDirection === 'rtl') {
        offscreenDirectionClass = 'move-right';
      } else {
        offscreenDirectionClass = 'move-left';
      }
      if (rapidClickCheck) {
        return;
      }
      rapidClickCheck = true;
      toggleMenu();
    });
    $('.main-panel').on('click', function (e) {
      var target = e.target;
      if (isOffscreenOpen && target !== $('[data-toggle=offscreen]')) {
        toggleMenu();
      }
    });
    /* Sidebar sub-menus */
    $('.sidebar-panel nav a').on('click', function (e) {
      var $this = $(this),
        links = $this.parents('li'),
        parentLink = $this.closest('li'),
        otherLinks = $('.sidebar-panel nav li').not(links),
        subMenu = $this.next();
      if (!subMenu.hasClass('sub-menu')) {
        toggleMenu();
        return;
      }
      if (app.hasClass('layout-small-menu') && parentLink.parent().hasClass('nav') && $(window).width() > 768) {
        return;
      }
      otherLinks.removeClass('open');
      if (subMenu.is('ul') && (subMenu.height() === 0)) {
        parentLink.addClass('open');
      } else if (subMenu.is('ul') && (subMenu.height() !== 0)) {
        parentLink.removeClass('open');
      }
      if ($this.attr('href') === '#') {
        e.preventDefault();
      }
      updateScrollbars();
      if (subMenu.is('ul')) {
        return false;
      }
      e.stopPropagation();
      return true;
    });
    $('.sidebar-panel').find('> li > .sub-menu').each(function () {
      if ($(this).find('ul.sub-menu').length > 0) {
        $(this).addClass('multi-level');
      }
    });
    $('.sidebar-panel').find('.sub-menu').each(function () {
      $(this).parent('li').addClass('menu-accordion');
    });
  }

  function toggleMenu() {
    if (isOffscreenOpen) {
      app.removeClass('move-left move-right');
      setTimeout(function () {
        app.removeClass('offscreen');
      }, 150);
    } else {
      app.addClass('offscreen ' + offscreenDirectionClass);
    }
    isOffscreenOpen = !isOffscreenOpen;
    rapidClickFix();
  }

  function rapidClickFix() {
    setTimeout(function () {
      rapidClickCheck = false;
    }, 150);
  }

  function initScrollbars() {
    if (app.hasClass('layout-small-menu') || app.hasClass('layout-static-sidebar') || app.hasClass('layout-boxed')) {
      return;
    }
    psTarg.perfectScrollbar({
      wheelPropagation: true,
      suppressScrollX: true
    });
  }

  function destroyScrollbars() {
    psTarg.perfectScrollbar('destroy').removeClass('ps-container ps-active-y ps-active-x');
  }

  function updateScrollbars() {
    if (psTarg.hasClass('ps-container') && !app.hasClass('layout-small-menu')) {
      psTarg.perfectScrollbar('update');
    }
  }
  initScrollbars();

  $(window).smartresize(function () {
    updateScrollbars();
  });

  function init() {
    smallMenu();
    sidebarPanel();
    quickLaunch();
    scrollToTop();
    smoothScroll();
    activeState();
    rippleEffect();
    preloader();
    scrollable();
    cardControls();
    accordion();
  }
  init();
})(jQuery);