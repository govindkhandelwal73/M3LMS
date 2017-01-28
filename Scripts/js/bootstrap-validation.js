// compare validator
var NewPassword = $('#NewPassword').val();
var ConfirmPassword = $('#ConfirmNewPassword').val();
if ($('#NewPassword').val() === $('#ConfirmNewPassword').val()) {
    document.getElementById('ConfirmPassLabel').innerHTML="Password Does Not Match"
}