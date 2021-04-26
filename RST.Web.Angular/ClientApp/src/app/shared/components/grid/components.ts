
import * as $ from 'jquery';
export function getNumericCellEditor(): any {
    function isCharNumeric(charStr) {
        return (!!/\d/.test(charStr) || charStr =='.');
    }
    function isKeyPressedNumeric(event) {
        var charCode = getCharCodeFromEvent(event);
        var charStr = String.fromCharCode(charCode);
        return isCharNumeric(charStr);
    }
    function getCharCodeFromEvent(event) {
        event = event || window.event;
        return typeof event.which === "undefined" ? event.keyCode : event.which;
    }
    function NumericCellEditor() { }
    NumericCellEditor.prototype.init = function (params) {
        this.focusAfterAttached = params.cellStartedEdit;
        this.eInput = document.createElement("input");
        this.eInput.style.width = "100%";
        this.eInput.style.height = "100%";
        this.eInput.value = isCharNumeric(params.charPress) ? params.charPress : params.value;
        var that = this;
        this.eInput.addEventListener("keypress", function (event) {
            if (!isKeyPressedNumeric(event)) {
                that.eInput.focus();
                if (event.preventDefault) event.preventDefault();
            }
        });
    };
    NumericCellEditor.prototype.getGui = function () {
        return this.eInput;
    };
    NumericCellEditor.prototype.afterGuiAttached = function () {
        if (this.focusAfterAttached) {
            this.eInput.focus();
            this.eInput.select();
        }
    };
    NumericCellEditor.prototype.isCancelBeforeStart = function () {
        return this.cancelBeforeStart;
    };
    NumericCellEditor.prototype.isCancelAfterEnd = function () { };
    NumericCellEditor.prototype.getValue = function () {
        return this.eInput.value;
    };
    NumericCellEditor.prototype.focusIn = function () {
        var eInput = this.getGui();
        eInput.focus();
        eInput.select();
        console.log("NumericCellEditor.focusIn()");
    };
    NumericCellEditor.prototype.focusOut = function () {
        console.log("NumericCellEditor.focusOut()");
    };
    return NumericCellEditor;
}

export function getdatePickerCellEditor(): any {
  function Datepicker() { }
  Datepicker.prototype.init = function (params) {
    var date = new Date();
    this.eInput = document.createElement("input");
    this.eInput.value = params.value;
    (<any>$(this.eInput)).datepicker({
      dateFormat: 'yy-mm-dd',
      changeMonth: true,
      changeYear: true });
  };
  Datepicker.prototype.getGui = function () {
    return this.eInput;
  };
  Datepicker.prototype.afterGuiAttached = function () {
    this.eInput.focus();
    this.eInput.select();
  };
  Datepicker.prototype.getValue = function () {
    return this.eInput.value;
  };
  Datepicker.prototype.destroy = function () { };
  Datepicker.prototype.isPopup = function () {
    return false;
  };
  return Datepicker;
}

export function getMaxLengthTextCellEditor(maxLength: number): any {
    
    function MaxLengthTextCellEditor() { }
    MaxLengthTextCellEditor.prototype.init = function (params) {
        console.log(params);
        this.focusAfterAttached = params.cellStartedEdit;
        this.eInput = document.createElement("input");
        this.eInput.style.width = "100%";
        this.eInput.style.height = "100%";
        this.eInput.value =  params.value;
        var that = this;
        this.eInput.addEventListener("keypress", function (event) {                       
           if (that.eInput.value.length +1 > maxLength) {
                that.eInput.focus();
                if (event.preventDefault) event.preventDefault();
            }
        });
       
    };
    MaxLengthTextCellEditor.prototype.getGui = function () {
        return this.eInput;
    };
    MaxLengthTextCellEditor.prototype.afterGuiAttached = function () {
        if (this.focusAfterAttached) {
            this.eInput.focus();
            this.eInput.select();
        }
    };
    MaxLengthTextCellEditor.prototype.isCancelBeforeStart = function () {
        return this.cancelBeforeStart;
    };
    MaxLengthTextCellEditor.prototype.isCancelAfterEnd = function () { };
    MaxLengthTextCellEditor.prototype.getValue = function () {
        return this.eInput.value;
    };
    MaxLengthTextCellEditor.prototype.focusIn = function () {
        var eInput = this.getGui();
        eInput.focus();
        eInput.select();
        console.log("NumericCellEditor.focusIn()");
    };
    MaxLengthTextCellEditor.prototype.focusOut = function () {
        console.log("NumericCellEditor.focusOut()");
    };
    return MaxLengthTextCellEditor;
}
export function getCutomSelectCellEditor(): any {

    function CutomSelectCellEditor() { }
    CutomSelectCellEditor.prototype.init = function (params) {
        console.log(params);
        this.focusAfterAttached = params.cellStartedEdit;
        this.eInput = document.createElement("select");
        if (params.values != null && params.values.length > 0) {
            for (var i = 0; i < params.values.length; i++)
            {
                
                var option = document.createElement("option");
                option.value = params.values[i];
                option.innerText = params.values[i];
                this.eInput.appendChild(option);
            }
        }
        this.eInput.style.width = "100%";
        this.eInput.style.height = "100%";
        this.eInput.value = params.value;
    };
    CutomSelectCellEditor.prototype.getGui = function () {
        return this.eInput;
    };
    CutomSelectCellEditor.prototype.afterGuiAttached = function () {
        if (this.focusAfterAttached) {
            this.eInput.focus();
            this.eInput.select();
        }
    };
    CutomSelectCellEditor.prototype.isCancelBeforeStart = function () {
        return this.cancelBeforeStart;
    };
    CutomSelectCellEditor.prototype.isCancelAfterEnd = function () { };
    CutomSelectCellEditor.prototype.getValue = function () {
        return this.eInput.value;
    };
    CutomSelectCellEditor.prototype.focusIn = function () {
        var eInput = this.getGui();
        eInput.focus();
        eInput.select();
        console.log("NumericCellEditor.focusIn()");
    };
    CutomSelectCellEditor.prototype.focusOut = function () {
        console.log("NumericCellEditor.focusOut()");
    };
    return CutomSelectCellEditor;
}
