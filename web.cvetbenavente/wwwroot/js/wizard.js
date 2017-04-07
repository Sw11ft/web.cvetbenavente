(function ($) {
  'use strict';

  //datepickers
  $(".datepicker").datepicker({
      format: "dd/mm/yyyy",
      language: "pt",

      endDate: "0d"
  });
        
  // Jquery validator
  var $validator = $('#wizardForm').validate({
    rules: {
        "Cliente.NrCliente": {
            required: true,
            number: true
        },
        "Cliente.Nome": {
            required: true,
            lettersonly: true,
            minlength: 1
        },
        "Cliente.Apelido": {
            required: true,
            minlength: 1
        },
        "Cliente.Email": {
            required: false,
            email: true,
            minlength: 3
        },
        "Cliente.DataNascimento": {
            required: true,
            date: true
        },
        "Cliente.Contacto1": {
            required: false,
            number: true
        },
        "Cliente.Contacto2": {
            required: false,
            number: true
        },
        "Cliente.Ativo": {
            required: true
        },
        "Cliente.Morada": {
            required: false
        },
        "Cliente.CodPostal": {
            required: false
        },
        "Cliente.Localidade": {
            required: false
        },
        "Cliente.Pais": {
            required: true
        }
    },
    errorElement: 'span',
    errorClass: 'error',
    errorPlacement: function (error, element) {
        if (element.parent('.input-group').length) {
            error.insertAfter(element.parent());
        } else {
            error.insertAfter(element);
        }
    }
  });


  function checkValidation() {
    var $valid = $('#wizardForm').valid();
    if (!$valid) {
      $validator.focusInvalid();
      return false;
    }
  }

  // Twitter bootstrap wizard
  $('#rootwizard').bootstrapWizard({
    tabClass: '',
    'nextSelector': '.button-next',
    'previousSelector': '.button-previous',
    onNext: checkValidation,
    onLast: checkValidation,
    onTabClick: checkValidation
  });

})(jQuery);