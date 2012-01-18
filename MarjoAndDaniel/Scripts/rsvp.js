ko.bindingHandlers.readonly = {
    update: function (element, valueAccessor) {
        var el = $(element);
        var value = valueAccessor();
        if (value) {
            el.attr("readonly", "readonly")
                .bind("focus.readonly", function () { $(this).blur() })
                .toggleClass("readonly", value)
        } else {
            el.removeAttr("readonly")
                .unbind('.readonly')
                .toggleClass("readonly", value);
        }
    }
};

var viewModel = {

    Primary: {
        Name: ko.observable(""),
        IsEditable: ko.observable(true),
        IsAttending: ko.observable(null)
    },

    PlusOne: {
        Name: ko.observable(""),
        IsEditable: ko.observable(true),
        IsAttending: ko.observable(null)
    },

    Id: ko.observable(null),

    IsAllowedPlusOne: ko.observable(false),

    IsGuestPrefilled: ko.observable(false),

    Message: ko.observable(""),

    bringingGuest: ko.observable(false),

    isRecordSelected: ko.observable(false),

    startOver: function () {
        this.Primary.Name("");
        this.Primary.IsEditable(true);
        this.Primary.IsAttending(null);
        this.PlusOne.Name("");
        this.PlusOne.IsEditable(true);
        this.PlusOne.IsAttending(null);
        this.Id(null);
        this.IsAllowedPlusOne(false);
        this.IsGuestPrefilled(false);
        this.Message("");
        this.isRecordSelected(false);

        $("#searchbox").focus();
    },

    map: function (data) {
        if (!data) return;

        this.Primary.Name(data.Primary.Name);
        this.Primary.IsEditable(data.Primary.IsEditable);
        this.Primary.IsAttending(data.Primary.IsAttending);
        this.PlusOne.Name(data.PlusOne.Name);
        this.PlusOne.IsEditable(data.PlusOne.IsEditable);
        this.PlusOne.IsAttending(data.PlusOne.IsAttending);
        this.Id(data.Id);
        this.IsAllowedPlusOne(data.IsAllowedPlusOne);
        this.IsGuestPrefilled(data.IsGuestPrefilled);
        this.Message(data.Message);
        this.isRecordSelected(true);

        $("#searchbox").blur();
    }
};

viewModel.Primary.IsAttending.subscribe(function (value) {
    if (value !== 'Accept')
        viewModel.bringingGuest(null);
});

viewModel.bringingGuest.subscribe(function (value) {
    if (viewModel.IsGuestPrefilled()) return;

    if (value === 'Yes')
        viewModel.PlusOne.IsAttending('Accept');
    else
        viewModel.PlusOne.IsAttending(null);
});

viewModel.map(data);

ko.applyBindings(viewModel);

$("#searchbox").autocomplete({
    minLength: 3,
    source: dataUrl || "/Rsvp/Search",
    focus: function (event, ui) {
        $("#searchbox").val(ui.item.label);
        return false;
    },
    select: function (event, ui) {
        viewModel.map(ui.item.value);
        $("#searchbox").val("");
        return false;
    }
})