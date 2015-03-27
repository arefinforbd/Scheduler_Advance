﻿! function (a) {
    "use strict";

    function b(a) {
        return ko.isObservable(a) && !(void 0 === a.destroyAll)
    }

    function c(a, b) {
        for (var c = 0; c < a.length; ++c) b(a[c])
    }

    function d(b, c) {
        this.$select = a(b), this.options = this.mergeOptions(a.extend({}, c, this.$select.data())), this.originalOptions = this.$select.clone()[0].options, this.query = "", this.searchTimeout = null, this.options.multiple = "multiple" === this.$select.attr("multiple"), this.options.onChange = a.proxy(this.options.onChange, this), this.options.onDropdownShow = a.proxy(this.options.onDropdownShow, this), this.options.onDropdownHide = a.proxy(this.options.onDropdownHide, this), this.options.onDropdownShown = a.proxy(this.options.onDropdownShown, this), this.options.onDropdownHidden = a.proxy(this.options.onDropdownHidden, this), this.buildContainer(), this.buildButton(), this.buildDropdown(), this.buildSelectAll(), this.buildDropdownOptions(), this.buildFilter(), this.updateButtonText(), this.updateSelectAll(), this.options.disableIfEmpty && a("option", this.$select).length <= 0 && this.disable(), this.$select.hide().after(this.$container)
    }
    "undefined" != typeof ko && ko.bindingHandlers && !ko.bindingHandlers.multiselect && (ko.bindingHandlers.multiselect = {
        init: function (d, e, f) {
            var g = f().selectedOptions,
                h = ko.utils.unwrapObservable(e());
            a(d).multiselect(h), b(g) && (a(d).multiselect("select", ko.utils.unwrapObservable(g)), g.subscribe(function (b) {
                var e = [],
                    f = [];
                c(b, function (a) {
                    switch (a.status) {
                        case "added":
                            e.push(a.value);
                            break;
                        case "deleted":
                            f.push(a.value)
                    }
                }), e.length > 0 && a(d).multiselect("select", e), f.length > 0 && a(d).multiselect("deselect", f)
            }, null, "arrayChange"))
        },
        update: function (c, d, e) {
            var f = e().options,
                g = a(c).data("multiselect"),
                h = ko.utils.unwrapObservable(d());
            b(f) && f.subscribe(function () {
                a(c).multiselect("rebuild")
            }), g ? g.updateOriginalOptions() : a(c).multiselect(h)
        }
    }), d.prototype = {
        defaults: {
            buttonText: function (b, c) {
                if (0 === b.length) return this.nonSelectedText + ' <b class="fa fa-caret-down"></b>';
                if (b.length == a("option", a(c)).length) return this.allSelectedText + ' <b class="fa fa-caret-down"></b>';
                if (b.length > this.numberDisplayed) return b.length + " " + this.nSelectedText + ' <b class="fa fa-caret-down"></b>';
                var d = "";
                return b.each(function () {
                    var b = void 0 !== a(this).attr("label") ? a(this).attr("label") : a(this).html();
                    d += b + ", "
                }), d.substr(0, d.length - 2) + ' <b class="fa fa-caret-down"></b>'
            },
            buttonTitle: function (b) {
                if (0 === b.length) return this.nonSelectedText;
                var c = "";
                return b.each(function () {
                    c += a(this).text() + ", "
                }), c.substr(0, c.length - 2)
            },
            label: function (b) {
                return a(b).attr("label") || a(b).html()
            },
            onChange: function () { },
            onDropdownShow: function () { },
            onDropdownHide: function () { },
            onDropdownShown: function () { },
            onDropdownHidden: function () { },
            buttonClass: "btn btn-default",
            buttonWidth: "auto",
            buttonContainer: '<div class="btn-group" />',
            dropRight: !1,
            selectedClass: "active",
            maxHeight: !1,
            checkboxName: !1,
            includeSelectAllOption: !1,
            includeSelectAllIfMoreThan: 0,
            selectAllText: " Select all",
            selectAllValue: "multiselect-all",
            selectAllName: !1,
            enableFiltering: !1,
            enableCaseInsensitiveFiltering: !1,
            enableClickableOptGroups: !1,
            filterPlaceholder: "Search",
            filterBehavior: "text",
            includeFilterClearBtn: !0,
            preventInputChangeEvent: !1,
            nonSelectedText: "Select Invoice",
            nSelectedText: "selected",
            allSelectedText: "[ALL]",
            numberDisplayed: 30,
            disableIfEmpty: !1,
            templates: {
                button: '<button type="button" class="multiselect dropdown-toggle" data-toggle="dropdown"></button>',
                ul: '<ul class="multiselect-container dropdown-menu"></ul>',
                filter: '<li class="multiselect-item filter"><div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-search"></i></span><input class="form-control multiselect-search" type="text"></div></li>',
                filterClearBtn: '<span class="input-group-btn"><button class="btn btn-default multiselect-clear-filter" type="button"><i class="glyphicon glyphicon-remove-circle"></i></button></span>',
                li: '<li><a href="javascript:void(0);"><label></label></a></li>',
                divider: '<li class="multiselect-item divider"></li>',
                liGroup: '<li class="multiselect-item multiselect-group"><label></label></li>'
            }
        },
        constructor: d,
        buildContainer: function () {
            this.$container = a(this.options.buttonContainer), this.$container.on("show.bs.dropdown", this.options.onDropdownShow), this.$container.on("hide.bs.dropdown", this.options.onDropdownHide), this.$container.on("shown.bs.dropdown", this.options.onDropdownShown), this.$container.on("hidden.bs.dropdown", this.options.onDropdownHidden)
        },
        buildButton: function () {
            this.$button = a(this.options.templates.button).addClass(this.options.buttonClass), this.$select.prop("disabled") ? this.disable() : this.enable(), this.options.buttonWidth && "auto" !== this.options.buttonWidth && (this.$button.css({
                width: this.options.buttonWidth
            }), this.$container.css({
                width: this.options.buttonWidth
            }));
            var b = this.$select.attr("tabindex");
            b && this.$button.attr("tabindex", b), this.$container.prepend(this.$button)
        },
        buildDropdown: function () {
            this.$ul = a(this.options.templates.ul), this.options.dropRight && this.$ul.addClass("pull-right"), this.options.maxHeight && this.$ul.css({
                "max-height": this.options.maxHeight + "px",
                "overflow-y": "auto",
                "overflow-x": "hidden"
            }), this.$container.append(this.$ul)
        },
        buildDropdownOptions: function () {
            this.$select.children().each(a.proxy(function (b, c) {
                var d = a(c),
                    e = d.prop("tagName").toLowerCase();
                d.prop("value") !== this.options.selectAllValue && ("optgroup" === e ? this.createOptgroup(c) : "option" === e && ("divider" === d.data("role") ? this.createDivider() : this.createOptionValue(c)))
            }, this)), a("li input", this.$ul).on("change", a.proxy(function (b) {
                var c = a(b.target),
                    d = c.prop("checked") || !1,
                    e = c.val() === this.options.selectAllValue;
                this.options.selectedClass && (d ? c.closest("li").addClass(this.options.selectedClass) : c.closest("li").removeClass(this.options.selectedClass));
                var f = c.val(),
                    g = this.getOptionByValue(f),
                    h = a("option", this.$select).not(g),
                    i = a("input", this.$container).not(c);
                return e && (d ? this.selectAll() : this.deselectAll()), e || (d ? (g.prop("selected", !0), this.options.multiple ? g.prop("selected", !0) : (this.options.selectedClass && a(i).closest("li").removeClass(this.options.selectedClass), a(i).prop("checked", !1), h.prop("selected", !1), this.$button.click()), "active" === this.options.selectedClass && h.closest("a").css("outline", "")) : g.prop("selected", !1)), this.$select.change(), this.updateButtonText(), this.updateSelectAll(), this.options.onChange(g, d), this.options.preventInputChangeEvent ? !1 : void 0
            }, this)), a("li a", this.$ul).on("touchstart click", function (b) {
                b.stopPropagation();
                var c = a(b.target);
                try {
                    if ("Range" === document.getSelection().type) {
                        var d = a(this).find("input:first");
                        d.prop("checked", !d.prop("checked")).trigger("change")
                    }
                } catch (e) { }
                if (b.shiftKey) {
                    var f = c.prop("checked") || !1;
                    if (f) {
                        var g = c.closest("li").siblings('li[class="active"]:first'),
                            h = c.closest("li").index(),
                            i = g.index();
                        h > i ? c.closest("li").prevUntil(g).each(function () {
                            a(this).find("input:first").prop("checked", !0).trigger("change")
                        }) : c.closest("li").nextUntil(g).each(function () {
                            a(this).find("input:first").prop("checked", !0).trigger("change")
                        })
                    }
                }
                c.blur()
            }), this.$container.off("keydown.multiselect").on("keydown.multiselect", a.proxy(function (b) {
                if (!a('input[type="text"]', this.$container).is(":focus"))
                    if (9 === b.keyCode && this.$container.hasClass("open")) this.$button.click();
                    else {
                        var c = a(this.$container).find("li:not(.divider):not(.disabled) a").filter(":visible");
                        if (!c.length) return;
                        var d = c.index(c.filter(":focus"));
                        38 === b.keyCode && d > 0 ? d-- : 40 === b.keyCode && d < c.length - 1 ? d++ : ~d || (d = 0);
                        var e = c.eq(d);
                        if (e.focus(), 32 === b.keyCode || 13 === b.keyCode) {
                            var f = e.find("input");
                            f.prop("checked", !f.prop("checked")), f.change()
                        }
                        b.stopPropagation(), b.preventDefault()
                    }
            }, this)), this.options.enableClickableOptGroups && this.options.multiple && a("li.multiselect-group", this.$ul).on("click", a.proxy(function (b) {
                b.stopPropagation();
                var c = a(b.target).parent(),
                    d = c.nextUntil("li.multiselect-group"),
                    e = !0,
                    f = d.find("input");
                f.each(function () {
                    e = e && a(this).prop("checked")
                }), f.prop("checked", !e).trigger("change")
            }, this))
        },
        createOptionValue: function (b) {
            var c = a(b);
            c.is(":selected") && c.prop("selected", !0);
            var d = this.options.label(b),
                e = c.val(),
                f = this.options.multiple ? "checkbox" : "radio",
                g = a(this.options.templates.li),
                h = a("label", g);
            h.addClass(f);
            var i = a("<input/>").attr("type", f).addClass("ace");
            this.options.checkboxName && i.attr("name", this.options.checkboxName), h.append(i), i.after('<span class="lbl" />');
            var j = c.prop("selected") || !1;
            i.val(e), e === this.options.selectAllValue && (g.addClass("multiselect-item multiselect-all"), i.parent().parent().addClass("multiselect-all")), h.append(" " + d), h.attr("title", c.attr("title")), this.$ul.append(g), c.is(":disabled") && i.attr("disabled", "disabled").prop("disabled", !0).closest("a").attr("tabindex", "-1").closest("li").addClass("disabled"), i.prop("checked", j), j && this.options.selectedClass && i.closest("li").addClass(this.options.selectedClass)
        },
        createDivider: function () {
            var b = a(this.options.templates.divider);
            this.$ul.append(b)
        },
        createOptgroup: function (b) {
            var c = a(b).prop("label"),
                d = a(this.options.templates.liGroup);
            a("label", d).text(c), this.options.enableClickableOptGroups && d.addClass("multiselect-group-clickable"), this.$ul.append(d), a(b).is(":disabled") && d.addClass("disabled"), a("option", b).each(a.proxy(function (a, b) {
                this.createOptionValue(b)
            }, this))
        },
        buildSelectAll: function () {
            "number" == typeof this.options.selectAllValue && (this.options.selectAllValue = this.options.selectAllValue.toString());
            var b = this.hasSelectAll();
            if (!b && this.options.includeSelectAllOption && this.options.multiple && a("option", this.$select).length > this.options.includeSelectAllIfMoreThan) {
                this.options.includeSelectAllDivider && this.$ul.prepend(a(this.options.templates.divider));
                var c = a(this.options.templates.li);
                a("label", c).addClass("checkbox"), a("label", c).append(this.options.selectAllName ? '<input type="checkbox" name="' + this.options.selectAllName + '" />' : '<input style="margin-left: 0px; margin-right: 10px;" type="checkbox" class="ace" /><span class="lbl"></span>');
                var d = a("input", c);
                d.val(this.options.selectAllValue), c.addClass("multiselect-item multiselect-all"), d.parent().parent().addClass("multiselect-all"), a("label", c).append(" " + this.options.selectAllText), this.$ul.prepend(c), d.prop("checked", !1)
            }
        },
        buildFilter: function () {
            if (this.options.enableFiltering || this.options.enableCaseInsensitiveFiltering) {
                var b = Math.max(this.options.enableFiltering, this.options.enableCaseInsensitiveFiltering);
                if (this.$select.find("option").length >= b) {
                    if (this.$filter = a(this.options.templates.filter), a("input", this.$filter).attr("placeholder", this.options.filterPlaceholder), this.options.includeFilterClearBtn) {
                        var c = a(this.options.templates.filterClearBtn);
                        c.on("click", a.proxy(function () {
                            clearTimeout(this.searchTimeout), this.$filter.find(".multiselect-search").val(""), a("li", this.$ul).show().removeClass("filter-hidden"), this.updateSelectAll()
                        }, this)), this.$filter.find(".input-group").append(c)
                    }
                    this.$ul.prepend(this.$filter), this.$filter.val(this.query).on("click", function (a) {
                        a.stopPropagation()
                    }).on("input keydown", a.proxy(function (b) {
                        13 === b.which && b.preventDefault(), clearTimeout(this.searchTimeout), this.searchTimeout = this.asyncFunction(a.proxy(function () {
                            if (this.query !== b.target.value) {
                                this.query = b.target.value;
                                var c, d;
                                a.each(a("li", this.$ul), a.proxy(function (b, e) {
                                    var f = a("input", e).val(),
                                        g = a("label", e).text(),
                                        h = "";
                                    if ("text" === this.options.filterBehavior ? h = g : "value" === this.options.filterBehavior ? h = f : "both" === this.options.filterBehavior && (h = g + "\n" + f), f !== this.options.selectAllValue && g) {
                                        var i = !1;
                                        this.options.enableCaseInsensitiveFiltering && h.toLowerCase().indexOf(this.query.toLowerCase()) > -1 ? i = !0 : h.indexOf(this.query) > -1 && (i = !0), a(e).toggle(i).toggleClass("filter-hidden", !i), a(e).hasClass("multiselect-group") ? (c = e, d = i) : (i && a(c).show().removeClass("filter-hidden"), !i && d && a(e).show().removeClass("filter-hidden"))
                                    }
                                }, this))
                            }
                            this.updateSelectAll()
                        }, this), 300, this)
                    }, this))
                }
            }
        },
        destroy: function () {
            this.$container.remove(), this.$select.show(), this.$select.data("multiselect", null)
        },
        refresh: function () {
            a("option", this.$select).each(a.proxy(function (b, c) {
                var d = a("li input", this.$ul).filter(function () {
                    return a(this).val() === a(c).val()
                });
                a(c).is(":selected") ? (d.prop("checked", !0), this.options.selectedClass && d.closest("li").addClass(this.options.selectedClass)) : (d.prop("checked", !1), this.options.selectedClass && d.closest("li").removeClass(this.options.selectedClass)), a(c).is(":disabled") ? d.attr("disabled", "disabled").prop("disabled", !0).closest("li").addClass("disabled") : d.prop("disabled", !1).closest("li").removeClass("disabled")
            }, this)), this.updateButtonText(), this.updateSelectAll()
        },
        select: function (b, c) {
            a.isArray(b) || (b = [b]);
            for (var d = 0; d < b.length; d++) {
                var e = b[d];
                if (null !== e && void 0 !== e) {
                    var f = this.getOptionByValue(e),
                        g = this.getInputByValue(e);
                    void 0 !== f && void 0 !== g && (this.options.multiple || this.deselectAll(!1), this.options.selectedClass && g.closest("li").addClass(this.options.selectedClass), g.prop("checked", !0), f.prop("selected", !0))
                }
            }
            this.updateButtonText(), this.updateSelectAll(), c && 1 === b.length && this.options.onChange(f, !0)
        },
        clearSelection: function () {
            this.deselectAll(!1), this.updateButtonText(), this.updateSelectAll()
        },
        deselect: function (b, c) {
            a.isArray(b) || (b = [b]);
            for (var d = 0; d < b.length; d++) {
                var e = b[d];
                if (null !== e && void 0 !== e) {
                    var f = this.getOptionByValue(e),
                        g = this.getInputByValue(e);
                    void 0 !== f && void 0 !== g && (this.options.selectedClass && g.closest("li").removeClass(this.options.selectedClass), g.prop("checked", !1), f.prop("selected", !1))
                }
            }
            this.updateButtonText(), this.updateSelectAll(), c && 1 === b.length && this.options.onChange(f, !1)
        },
        selectAll: function (b) {
            var b = "undefined" == typeof b ? !0 : b,
                c = a("li input[type='checkbox']:enabled", this.$ul),
                d = c.filter(":visible"),
                e = c.length,
                f = d.length;
            if (b ? (d.prop("checked", !0), a("li:not(.divider):not(.disabled)", this.$ul).filter(":visible").addClass(this.options.selectedClass)) : (c.prop("checked", !0), a("li:not(.divider):not(.disabled)", this.$ul).addClass(this.options.selectedClass)), e === f || b === !1) a("option:enabled", this.$select).prop("selected", !0);
            else {
                var g = d.map(function () {
                    return a(this).val()
                }).get();
                a("option:enabled", this.$select).filter(function () {
                    return -1 !== a.inArray(a(this).val(), g)
                }).prop("selected", !0)
            }
        },
        deselectAll: function (b) {
            var b = "undefined" == typeof b ? !0 : b;
            if (b) {
                var c = a("li input[type='checkbox']:enabled", this.$ul).filter(":visible");
                c.prop("checked", !1);
                var d = c.map(function () {
                    return a(this).val()
                }).get();
                a("option:enabled", this.$select).filter(function () {
                    return -1 !== a.inArray(a(this).val(), d)
                }).prop("selected", !1), this.options.selectedClass && a("li:not(.divider):not(.disabled)", this.$ul).filter(":visible").removeClass(this.options.selectedClass)
            } else a("li input[type='checkbox']:enabled", this.$ul).prop("checked", !1), a("option:enabled", this.$select).prop("selected", !1), this.options.selectedClass && a("li:not(.divider):not(.disabled)", this.$ul).removeClass(this.options.selectedClass)
        },
        rebuild: function () {
            this.$ul.html(""), this.options.multiple = "multiple" === this.$select.attr("multiple"), this.buildSelectAll(), this.buildDropdownOptions(), this.buildFilter(), this.updateButtonText(), this.updateSelectAll(), this.options.disableIfEmpty && a("option", this.$select).length <= 0 && this.disable(), this.options.dropRight && this.$ul.addClass("pull-right")
        },
        dataprovider: function (b) {
            var d = "",
                e = 0,
                f = a("");
            a.each(b, function (b, g) {
                var h;
                a.isArray(g.children) ? (e++, h = a("<optgroup/>").attr({
                    label: g.label || "Group " + e
                }), c(g.children, function (b) {
                    h.append(a("<option/>").attr({
                        value: b.value,
                        label: b.label || b.value,
                        title: b.title,
                        selected: !!b.selected
                    }))
                }), d += "</optgroup>") : h = a("<option/>").attr({
                    value: g.value,
                    label: g.label || g.value,
                    title: g.title,
                    selected: !!g.selected
                }), f = f.add(h)
            }), this.$select.empty().append(f), this.rebuild()
        },
        enable: function () {
            this.$select.prop("disabled", !1), this.$button.prop("disabled", !1).removeClass("disabled")
        },
        disable: function () {
            this.$select.prop("disabled", !0), this.$button.prop("disabled", !0).addClass("disabled")
        },
        setOptions: function (a) {
            this.options = this.mergeOptions(a)
        },
        mergeOptions: function (b) {
            return a.extend(!0, {}, this.defaults, b)
        },
        hasSelectAll: function () {
            return a("li." + this.options.selectAllValue, this.$ul).length > 0
        },
        updateSelectAll: function () {
            if (this.hasSelectAll()) {
                var b = a("li:not(.multiselect-item):not(.filter-hidden) input:enabled", this.$ul),
                    c = b.length,
                    d = b.filter(":checked").length,
                    e = a("li." + this.options.selectAllValue, this.$ul),
                    f = e.find("input");
                d > 0 && d === c ? (f.prop("checked", !0), e.addClass(this.options.selectedClass)) : (f.prop("checked", !1), e.removeClass(this.options.selectedClass))
            }
        },
        updateButtonText: function () {
            var b = this.getSelected();
            a(".multiselect", this.$container).html(this.options.buttonText(b, this.$select)), a(".multiselect", this.$container).attr("title", this.options.buttonTitle(b, this.$select))
        },
        getSelected: function () {
            return a("option", this.$select).filter(":selected")
        },
        getOptionByValue: function (b) {
            for (var c = a("option", this.$select), d = b.toString(), e = 0; e < c.length; e += 1) {
                var f = c[e];
                if (f.value === d) return a(f)
            }
        },
        getInputByValue: function (b) {
            for (var c = a("li input", this.$ul), d = b.toString(), e = 0; e < c.length; e += 1) {
                var f = c[e];
                if (f.value === d) return a(f)
            }
        },
        updateOriginalOptions: function () {
            this.originalOptions = this.$select.clone()[0].options
        },
        asyncFunction: function (a, b, c) {
            var d = Array.prototype.slice.call(arguments, 3);
            return setTimeout(function () {
                a.apply(c || window, d)
            }, b)
        }
    }, a.fn.multiselect = function (b, c, e) {
        return this.each(function () {
            var f = a(this).data("multiselect"),
                g = "object" == typeof b && b;
            f || (f = new d(this, g), a(this).data("multiselect", f)), "string" == typeof b && (f[b](c, e), "destroy" === b && a(this).data("multiselect", !1))
        })
    }, a.fn.multiselect.Constructor = d, a(function () {
        a("select[data-role=multiselect]").multiselect()
    })
}(window.jQuery);