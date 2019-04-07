define("dominant-color/ColorPalette", [
        "dojo/_base/declare",
        "dijit/_Widget",
        "dijit/_TemplatedMixin",
        "dijit/_PaletteMixin",
        "dojo/_base/Color",
        "dojo/dom-construct",
        "dojo/string"
    ], function (
        declare,
        _Widget,
        _TemplatedMixin,
        _PaletteMixin,
        Color,
        domConstruct,
        string

    ) {

        return declare([_Widget, _TemplatedMixin, _PaletteMixin],
            {
                templateString: '<div class="dijitInline"><div class="dijitInline dijitColorPalette" role="grid"><table dojoAttachPoint="paletteTableNode" class= "dijitPaletteTable" cellSpacing="0" cellPadding="0" role="presentation" ><tbody data-dojo-attach-point="gridNode"></tbody></table></div><div>',

                _palette: null,

                buildRendering: function () {
                    this.inherited(arguments);
                    this._preparePalette(this.dominantColors, this.dominantColors.flat());
                },

                baseClass: "dijitColorPalette",

                dyeClass: declare([Color], {
                    template:
                        "<span class='dijitInline dijitPaletteImg'>" +
                            "<img src='${blankGif}'  alt='${alt}' title='${title}' class='dijitColorPaletteSwatch' style='background-color: ${color}'/>" +
                            "</span>",

                    constructor: function (hex, row, col) {
                        this._row = row;
                        this._col = col;
                        this.setColor(hex);
                    },

                    getValue: function () {
                        return this.toHex();
                    },

                    fillCell: function (cell, blankGif) {
                        var hex = this.toHex();
                        var html = string.substitute(this.template, {
                            color: hex,
                            blankGif: blankGif,
                            alt: hex,
                            title: hex
                        });
                        domConstruct.place(html, cell);
                    }
                }),

                isValid: function () {
                    return true;
                }
            });
    }
);