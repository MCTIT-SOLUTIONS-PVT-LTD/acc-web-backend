/**
 * Theme: Frogetor - Responsive Bootstrap 4 Admin Dashboard
 * Author: Mannatthemes
 * Footable Js
 */

$(function () {
    "use strict";

    /* Init FooTable */
    $('#footable-1,#footable-2').footable();

    /* Editing FooTable */
    var $modal = $('#editor-modal'),
        $editor = $('#editor'),
        $editorTitle = $('#editor-title'),
        ft = FooTable.init('#footable-3', {
            editing: {
                enabled: true,
                addRow: function () {
                    $modal.removeData('row');
                    $editor[0].reset();
                    $editorTitle.text('Add a new row');
                    $modal.modal('show');
                },
                editRow: function (row) {
                    var values = row.val();
                    $editor.find('#id').val(values.id);
                    $editor.find('#Name').val(values.Name);
                    $editor.find('#Email').val(values.Email);

                    // Set checkboxes for Transfer Method
                    $("#transferEmail").prop("checked", values.TransferMethod.includes("Email"));
                    $("#transferPhone").prop("checked", values.TransferMethod.includes("Mobile phone"));

                    // Set radio buttons for Preferred Language
                    if (values.preferredLangauge === "English") {
                        $("#langEnglish").prop("checked", true);
                    } else {
                        $("#langFrench").prop("checked", true);
                    }

                    $editor.find('#SecurityQuestion').val(values.SecurityQuestion);

                    $modal.data('row', row);
                    $editorTitle.text('Edit row #' + values.id);
                    $modal.modal('show');
                },
                deleteRow: function (row) {
                    if (confirm('Are you sure you want to delete the row?')) {
                        row.delete();
                    }
                }
            }
        }),
        uid = 10;

    $editor.on("submit", function (e) {
        if (this.checkValidity && !this.checkValidity()) return;
        e.preventDefault();
        var row = $modal.data("row"),
            values = {
                id: $editor.find("#id").val(),
                Name: $editor.find("#Name").val(),
                Email: $editor.find("#Email").val(),
                TransferMethod: [
                    $("#transferEmail").is(":checked") ? "Email" : "",
                    $("#transferPhone").is(":checked") ? "Mobile phone" : ""
                ].filter(Boolean).join(", "), // Store as a comma-separated value
                preferredLangauge: $("input[name='preferredLangauge']:checked").val(),
                SecurityQuestion: $editor.find("#SecurityQuestion").val(),
            };

        if (row instanceof FooTable.Row) {
            row.val(values);
        } else {
            values.id = uid++;
            ft.rows.add(values);
        }
        $modal.modal("hide");
    });
});
