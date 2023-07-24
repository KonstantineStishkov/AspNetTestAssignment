function InitializeCompaniesTable(table_type) {
  let table = $("." + table_type)[0];
  let url = table_type == 'Companies' ? "Companies/GetColumns" : 'GetColumns';
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
    $('.' + table_type + '-add').unbind();
    $('.' + table_type + '-add').on("click", function (e) {
      OpenForm(form, table_type, function () {
        FillTable(table, table_type, columns);
      });
    });
  
    $('.' + table_type + '-refresh').unbind();
    $('.' + table_type + '-refresh').on("click", function (e) {
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
  let url = table_type == 'Companies' ? "Companies/Get" + table_type : 'Get' + table_type;
  let tbody = $("tbody", table)[0];
  let company_id;

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

    console.log(columns);

    $.each(data, function (id, company) {
      let tr = document.createElement("tr");
      $(tr).addClass("table-line");
      $.each(columns, function (id, column) {
        let td = document.createElement("td");
        $(td).addClass("table-cell");
        $(td).addClass("cell-" + column.name);
        $(td).attr("width", column.width);
        $(td).html(company[column.name.toLowerCase()]);
        tr.appendChild(td);
      });

      $(tr).unbind();
      $(tr).on("click", function () {
        window.location.href = "/Companies/Details?id=" + company.id;
      });

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
    $(form).html(result);
    $('.' + table_type + '-submit', form).unbind();
    $('.' + table_type + '-submit', form).on("click", function (e) {
      SubmitForm(form[0], table_type, on_form_close_action);
    });

    $('.' + table_type + '-cancel', form).unbind();
    $('.' + table_type + '-cancel', form).on("click", function (e) {
      CloseForm(form[0]);
    });
  });
}

function FillForm(form, model) {}

function CloseForm(form) {
  form.style.display = "none";
}

function SubmitForm(form, table_type, on_form_close_action) {
  console.log($('input[name="Name"]', form).val());

  let data = {
    method: "POST",
    Name: $('input[name="Name"]', form).val(),
    Address: $('textarea[name="Address"]', form).val(),
    City: $('input[name="City"]', form).val(),
    State: $('input[name="State"]', form).val(),
    Phone: $('input[name="Phone"]', form).val(),
  };

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
