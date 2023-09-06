
    $(".custom-file-input").on("change", function () {
            var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
        });


    function CalcTotalExperience(){
            var x = document.getElementsByClassName('YearsWorked');
    var totalExp = 0;
    var i;

    for (i = 0; i < x.length; i++){
        totalExp = totalExp + eval(x[i].value);
            }

    document.getElementById('TotalExperience').value = totalExp;

    return;
        }

    document.addEventListener('change', function (e) {
            if (e.target.classList.contains('YearsWorked')){
        CalcTotalExperience();
            }
        }, false);


    function DeleteItem(btn) {

            var table = document.getElementById('ExpTable');
    var rows = table.getElementsByTagName('tr');
    if (rows.length == 2){
        alert("This Row Cannot Be Deleted");
    return;
        }

        var btnIdx = btn.id.replaceAll('btnremove-', '');
        var idofIsDeleted = btnIdx + "___IsDeleted";

        var hidIsDelId = document.querySelector("[id$='" + idofIsDeleted + "']").id;

        document.getElementById(hidIsDelId).value = "true";

   // $(btn).closest('tr').remove();

        $(btn).closest('tr').hide();

    CalcTotalExperience();
        }



    function AddItem(btn) {

            var table = document.getElementById('ExpTable');
    var rows = table.getElementsByTagName('tr');

    var rowOuterHtml = rows[rows.length - 1].outerHTML;

    var lastrowIdx = rows.length - 1;

    var nextrowIdx = eval(lastrowIdx) + 1;

    rowOuterHtml = rowOuterHtml.replaceAll('_' + lastrowIdx + '_', '_' + nextrowIdx + '_');
    rowOuterHtml = rowOuterHtml.replaceAll('[' + lastrowIdx + ']', '[' + nextrowIdx + ']');
    rowOuterHtml = rowOuterHtml.replaceAll('-' + lastrowIdx, '-' + nextrowIdx);


    var newRow = table.insertRow();
    newRow.innerHTML = rowOuterHtml;

        var x = document.getElementsByTagName("INPUT");

        for (var cnt = 0; cnt < x.length; cnt++) {
            if (x[cnt].type == "text" && x[cnt].id.indexOf('_' + nextrowIdx + '_') > 0)
                x[cnt].value = '';
           else if (x[cnt].type == "number" && x[cnt].id.indexOf('_' + nextrowIdx + '_') > 0)
                x[cnt].value = 0;
        }

    rebindvalidators();

        }

    function rebindvalidators(){
            var $form = $("#ApplicantForm");

    $form.unbind();

    $form.data("validator", null);

    $.validator.unobtrusive.parse($form);

    $form.validate($form.data("unobtrusiveValidation").options);
        }

