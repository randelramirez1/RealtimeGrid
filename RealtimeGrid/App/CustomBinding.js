ko.bindingHandlers.displayMoney = {

    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        // This will be called when the binding is first applied to an element
        // Setup up any initial state, event handlers, etc. here
        $(element).html('$' + valueAccessor()().toFixed(2));

    },

    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        // This will be called once when the binding is first applied to an element,
        // and again whenever the associated observable changes value.
        // Update the DOM element based on the supplied values here.
        $(element).html('$' + valueAccessor()().toFixed(2));
    }

};