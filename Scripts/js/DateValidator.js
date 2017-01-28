/// <reference path="../jquery.validate.min.js" />
/// <reference path="../jquery.validate.unobtrusive.min.js" />



$.validator.unobtrusive.adapters.addSingleVal("exclude", "chars");

$.validator.addMethod("exclude", function (value, element, exclude) {
    if (value) {
                   if(value<Date())
                return false;
            }
        
    
    return true;
});