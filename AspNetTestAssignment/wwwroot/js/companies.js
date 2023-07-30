function InitializeCompaniesTable(table_type) {
  let table = $("." + table_type)[0];
  let url = table_type == "Companies" ? "Companies/GetColumns" : "GetColumns";
  let form = $(".form-popup");
  let columns;

  console.log(url);

  $.get(url, { tableType: table_type }, function (data) {
    columns = data;
    ComposeTable(table, table_type, columns);
    FillTable(table, table_type, columns);
  });

  SetEventHandlers();

  function SetEventHandlers() {
    $("." + table_type + "-add").unbind();
    $("." + table_type + "-add").on("click", function (e) {
      OpenForm(form, table_type, function () {
        FillTable(table, table_type, columns);
      });
    });

    $("." + table_type + "-edit").unbind();
    $("." + table_type + "-edit").on("click", function (e) {
      console.log("click edit");
      if (table_type == "Companies") {
        let is_edit = $(table).attr("data-edit-mode");

        if (is_edit == "true") {
          $(table).attr("data-edit-mode", "false");
          $(table).addClass("is-edit-false");
          $(table).removeClass("is-edit-true");
        } else {
          $(table).attr("data-edit-mode", "true");
          $(table).addClass("is-edit-true");
          $(table).removeClass("is-edit-false");
        }
      }

      if (table_type == "Employees") {
        let container = $(".employee-summary-container");
        let form = $("form", container)[0];
        let id_input = form.elements["Id"];

        if (!id_input) {
          return;
        }

        let employee_id = id_input.value;
        let is_edit = $(table).attr("data-edit-mode");

        if (is_edit == "true") {
          $(table).attr("data-edit-mode", "false");
          $(table).addClass("is-edit-false");
          $(table).removeClass("is-edit-true");

          let data = GetSubmitData(form, table_type);

          $.post("/Companies/_EmployeeSummary", data, function (result) {
            $(container).empty();
            $(container).html(result);
            FillTable(table, table_type, columns);
          });
        } else {
          $(table).attr("data-edit-mode", "true");
          $(table).addClass("is-edit-true");
          $(table).removeClass("is-edit-false");

          $.get(
            "/Companies/_EmployeeSummary",
            { method: "GET", id: employee_id, isEdit: true },
            function (result) {
              $(container).empty();
              $(container).html(result);
            }
          );
        }
      }
    });

    $("." + table_type + "-refresh").unbind();
    $("." + table_type + "-refresh").on("click", function (e) {
      FillTable(table, table_type, columns);
    });
  }
}
function ComposeTable(table, table_type, columns) {
  ComposeHead(table, columns);
  let tbody = document.createElement("tbody");
  $(tbody).addClass("table-body");
  table.appendChild(tbody);
}

function ComposeHead(table, columns) {
  let tr = document.createElement("tr");
  $(tr).addClass("table-header-line");

  $.each(columns, function (id, column) {
    let th = document.createElement("th");
    $(th).attr("width", column.width);
    $(th).html(column.showName);
    tr.appendChild(th);
  });
  table.appendChild(tr);
}

function FillTable(table, table_type, columns) {
  let url =
    table_type == "Companies"
      ? "Companies/Get" + table_type
      : "Get" + table_type;
  let tbody = $("tbody", table)[0];
  let company_id;
  let selected;

  if (table_type != "Companies") {
    company_id = $(".details-container").attr("data-company-id");
  }

  $(tbody).empty();

  $.get(url, { tableType: table_type, id: company_id }, function (data) {
    if (data == false) {
      let tr = document.createElement("tr");
      $(tr).addClass("table-line");
      $(tr).html("No data");
      tbody.appendChild(tr);
    }

    let edit_button = $("." + table_type + "-edit")[0];

    if (edit_button) {
      edit_button.style.color = "lightgrey";
    }

    let remove_button = $("." + table_type + "-remove")[0];

    if (remove_button) {
      remove_button.style.color = "lightgrey";
    }

    $(remove_button).on("click", function () {
      if (!selected) {
        return;
      }

      $.get(
        "/Companies/Delete",
        { method: "GET", id: selected, tableType: table_type },
        function (result) {
          if (result == true) {
            FillTable(table, table_type, columns);
          }
        }
      );
    });

    $.each(data, function (id, element) {
      let tr = document.createElement("tr");
      $(tr).addClass("table-line");
      $(tr).attr("data-id", element.id);
      $.each(columns, function (id, column) {
        let key = column.name.charAt(0).toLowerCase() + column.name.slice(1);
        let td = document.createElement("td");
        $(td).addClass("table-cell");
        $(td).addClass("cell-" + column.name);
        $(td).attr("width", column.width);
        $(td).html(element[key]);
        tr.appendChild(td);
      });

      $(tr).unbind();

      if (table_type == "Companies") {
        let company = element;
        $(tr).on("click", function () {
          window.location.href = "/Companies/Details?id=" + company.id;
        });
      }

      if (table_type == "Notes") {
        $(tr).on("click", function () {
          remove_button.style.color = "black";
          selected = $(this).attr("data-id");
          console.log(selected);
        });
      }

      if (table_type == "Employees") {
        let url = "/Companies/_EmployeeSummary";
        let employee = element;

        $(tr).on("click", function () {
          selected = $(this).attr("data-id");
          edit_button.style.color = "black";
          remove_button.style.color = "black";
          $.get(
            url,
            {
              id: employee.id,
              isEdit: $(table).attr("data-edit-mode"),
            },
            function (result) {
              let container = $(".employee-summary-container");
              console.log(container);
              $(container).empty();
              $(container).html(result);
            }
          );
        });
      }

      tbody.appendChild(tr);
    });
  });
}

function OpenForm(form, table_type, on_form_close_action) {
  form[0].style.display = "block";
  let url = "/Companies/Add" + table_type;

  let company_id;

  if (table_type != "Companies") {
    company_id = $(".details-container").attr("data-company-id");
  }

  let data = {
    method: "GET",
    id: company_id,
  };

  console.log(data);

  $.get(url, data, function (result) {
    console.log(result);
    $(form).html(result);
    $("." + table_type + "-submit", form).unbind();
    $("." + table_type + "-submit", form).on("click", function (e) {
      SubmitForm(form[0], table_type, on_form_close_action);
    });

    $("." + table_type + "-cancel", form).unbind();
    $("." + table_type + "-cancel", form).on("click", function (e) {
      CloseForm(form[0]);
    });
  });
}

function CloseForm(form) {
  form.style.display = "none";
}

function SubmitForm(form, table_type, on_form_close_action) {
  console.log($('input[name="Name"]', form).val());

  let data = GetSubmitData(form, table_type);

  console.log(data);

  $.post("/Companies/Add" + table_type, data, function (result) {
    if (result == true) {
      CloseForm(form);
      on_form_close_action();
    } else {
      $.each(result, function (field_name, error_message) {
        console.log(field_name);
        let message_place = $(
          'span[data-valmsg-for="' + field_name + '"]',
          form
        );
        $(message_place).html(error_message);
      });
    }
  });
}

function GetSubmitData(form, table_type) {
  switch (table_type) {
    case "Companies":
      return {
        method: "POST",
        Name: $('input[name="Name"]', form).val(),
        Address: $('textarea[name="Address"]', form).val(),
        City: $('input[name="City"]', form).val(),
        State: $('input[name="State"]', form).val(),
        Phone: $('input[name="Phone"]', form).val(),
      };
    case "Notes":
      return {
        method: "POST",
        Id: $('input[name="Id"]', form).val(),
        CompanyId: $('input[name="CompanyId"]', form).val(),
        InvoiceNumber: $('input[name="InvoiceNumber"]', form).val(),
        EmployeeId: $('select[name="Employee"]', form).val(),
      };
    case "Employees":
      return {
        method: "POST",
        Id: $('input[name="Id"]', form).val(),
        CompanyId: $('input[name="CompanyId"]', form).val(),
        FirstName: $('input[name="FirstName"]', form).val(),
        LastName: $('input[name="LastName"]', form).val(),
        Title: $('input[name="Title"]', form).val(),
        BirthDate: $('input[name="BirthDate"]', form).val(),
        Position: $('input[name="Position"]', form).val(),
      };
  }
}
