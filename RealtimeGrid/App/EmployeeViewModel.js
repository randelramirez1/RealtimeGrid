var EmployeeViewModel = function (employeeSignalR) {

    var self = this;
    self.employees = ko.observableArray();
    self.loading = ko.observable(true);

    self.displayMode = function (employee) {
        console.log("display mode");
        if (employee.Locked()) {
            return 'lock-template';
        } else {
            return employee.Edit() ? 'edit-template' : 'read-template'
        }
    }

    self.edit = function (employee) {
        employee.Edit(true);
        employeeSignalR.server.lock(employee.Id,$.connection.hub.id)
    }

    self.done = function (employee) {
        employee.Edit(false);
        employeeSignalR.server.unlock(employee.Id)
    }


    self.watchModel = function (model, callback) {
        for (var key in model) {
            if (model.hasOwnProperty(key) && ko.isObservable(model[key]) && key != 'Edit' && key != 'Locked') {
                self.subscribeToProperty(model, key, function (key, val) {
                    callback(model, key, val);
                });
            }
        }
    }

    self.subscribeToProperty = function (model, key, callback) {
        model[key].subscribe(function (val) {
            callback(key, val);
        });
    }

    self.modelChanged = function (model, key, val) {
  
        // must use JSON.stringify otherwise it will be sent as a form encode and odata will not recognize it because it's not in json
        var payload = {};
        payload[key] = val;
        $.ajax({
            //url: '/odata/EmployeesOData(' + model.Id + ')',
            url: '/odata/EmployeesSignalR(' + model.Id + ')',
            type: 'PATCH',
            data: JSON.stringify(payload),
            contentType: 'application/json',
            dataType: 'json'
        });
    }


    //$.getJSON('api/Employees', function (data) {

    //    self.employees(ko.utils.arrayMap(data, function (employee) {
    //        var obsEmployee = {
    //            Id: employee.Id,
    //            Name: ko.observable(employee.Name),
    //            Email: ko.observable(employee.Email),
    //            Salary: ko.observable(employee.Salary),
    //        };
    //        self.watchModel(obsEmployee, self.modelChanged)
    //        return obsEmployee;
    //    }));

    //    self.loading(false);
    //});

    //for odata without SignalR url is = 'EmployeesOData' , this is case sensitive
    $.getJSON('/odata/EmployeesSignalR', function (data) {

        self.employees(ko.utils.arrayMap(data.value, function (employee) {
            var obsEmployee = {
                Id: employee.Id,
                Name: ko.observable(employee.Name),
                Email: ko.observable(employee.Email),
                Salary: ko.observable(employee.Salary),
                Edit: ko.observable(false),
                Locked: ko.observable(employee.Locked)
            };
            self.watchModel(obsEmployee, self.modelChanged)
            return obsEmployee;
        }));

        self.loading(false);
    });
}


$(function () {
    var employeesSignalR = $.connection.employee
    var viewModel = new EmployeeViewModel(employeesSignalR);
    window.x = viewModel;
    var findEmployee = function (id) {
        return ko.utils.arrayFirst(viewModel.employees(), function (item) {
            if (item.Id == id) {
                return item;
            }
        });
    }

    employeesSignalR.client.updateEmployee = function (id, key, value) {
        var employee = findEmployee(id);
        employee[key](value);
      
    }

    employeesSignalR.client.unlockEmployee = function (id) {
        var employee = findEmployee(id);
        employee.Locked(false);
    }

    employeesSignalR.client.lockEmployee = function (id, from) {
        var employee = findEmployee(id);
        console.log("client-lockEmployee");
        console.log($.connection.hub.id);
        console.log(from);
        toastr.success('Edit from connection: ' + from);
        employee.Locked(true);
    }

    $.connection.hub.start().done(function () {
        ko.applyBindings(viewModel);
    })
    //ko.applyBindings();
})