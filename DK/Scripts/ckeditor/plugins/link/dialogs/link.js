﻿/*
Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.dialog.add('link', function(a) {
    var b = CKEDITOR.plugins.link, c = function() {
        var F = this.getDialog(), G = F.getContentElement('target', 'popupFeatures'), H = F.getContentElement('target', 'linkTargetName'), I = this.getValue();
        if (!G || !H) return;
        G = G.getElement();
        G.hide();
        H.setValue('');
        switch (I) {
        case 'frame':
            H.setLabel(a.lang.link.targetFrameName);
            H.getElement().show();
            break;
        case 'popup':
            G.show();
            H.setLabel(a.lang.link.targetPopupName);
            H.getElement().show();
            break;
        default:
            H.setValue(I);
            H.getElement().hide();
            break;
        }
    }, d = function() {
        var F = this.getDialog(), G = ['urlOptions', 'anchorOptions', 'emailOptions'], H = this.getValue(), I = F.definition.getContents('upload'), J = I && I.hidden;
        if (H == 'url') {
            if (a.config.linkShowTargetTab) F.showPage('target');
            if (!J) F.showPage('upload');
        } else {
            F.hidePage('target');
            if (!J) F.hidePage('upload');
        }
        for (var K = 0; K < G.length; K++) {
            var L = F.getContentElement('info', G[K]);
            if (!L) continue;
            L = L.getElement().getParent().getParent();
            if (G[K] == H + 'Options') L.show();
            else L.hide();
        }
        F.layout();
    }, e = /^javascript:/, f = /^mailto:([^?]+)(?:\?(.+))?$/, g = /subject=([^;?:@&=$,\/]*)/, h = /body=([^;?:@&=$,\/]*)/, i = /^#(.*)$/, j = /^((?:http|https|ftp|news):\/\/)?(.*)$/, k = /^(_(?:self|top|parent|blank))$/, l = /^javascript:void\(location\.href='mailto:'\+String\.fromCharCode\(([^)]+)\)(?:\+'(.*)')?\)$/, m = /^javascript:([^(]+)\(([^)]+)\)$/, n = /\s*window.open\(\s*this\.href\s*,\s*(?:'([^']*)'|null)\s*,\s*'([^']*)'\s*\)\s*;\s*return\s*false;*\s*/, o = /(?:^|,)([^=]+)=(\d+|yes|no)/gi, p = function(F, G) {
        var H = G && (G.data('cke-saved-href') || G.getAttribute('href')) || '', I, J, K, L, M = {};
        if (I = H.match(e))
            if (y == 'encode') H = H.replace(l, function(ae, af, ag) { return 'mailto:' + String.fromCharCode.apply(String, af.split(',')) + (ag && w(ag)); });
            else if (y)
                H.replace(m, function(ae, af, ag) {
                    if (af == z.name) {
                        M.type = 'email';
                        var ah = M.email = {}, ai = /[^,\s]+/g, aj = /(^')|('$)/g, ak = ag.match(ai), al = ak.length, am, an;
                        for (var ao = 0; ao < al; ao++) {
                            an = decodeURIComponent(w(ak[ao].replace(aj, '')));
                            am = z.params[ao].toLowerCase();
                            ah[am] = an;
                        }
                        ah.address = [ah.name, ah.domain].join('@');
                    }
                });
        if (!M.type)
            if (K = H.match(i)) {
                M.type = 'anchor';
                M.anchor = {};
                M.anchor.name = M.anchor.id = K[1];
            } else if (J = H.match(f)) {
                var N = H.match(g), O = H.match(h);
                M.type = 'email';
                var P = M.email = {};
                P.address = J[1];
                N && (P.subject = decodeURIComponent(N[1]));
                O && (P.body = decodeURIComponent(O[1]));
            } else if (H && (L = H.match(j))) {
                M.type = 'url';
                M.url = {};
                M.url.protocol = L[1];
                M.url.url = L[2];
            } else M.type = 'url';
        if (G) {
            var Q = G.getAttribute('target');
            M.target = {};
            M.adv = {};
            if (!Q) {
                var R = G.data('cke-pa-onclick') || G.getAttribute('onclick'), S = R && R.match(n);
                if (S) {
                    M.target.type = 'popup';
                    M.target.name = S[1];
                    var T;
                    while (T = o.exec(S[2])) {
                        if ((T[2] == 'yes' || T[2] == '1') && !(T[1] in { height: 1, width: 1, top: 1, left: 1 })) M.target[T[1]] = true;
                        else if (isFinite(T[2])) M.target[T[1]] = T[2];
                    }
                }
            } else {
                var U = Q.match(k);
                if (U) M.target.type = M.target.name = Q;
                else {
                    M.target.type = 'frame';
                    M.target.name = Q;
                }
            }
            var V = this, W = function(ae, af) {
                var ag = G.getAttribute(af);
                if (ag !== null) M.adv[ae] = ag || '';
            };
            W('advId', 'id');
            W('advLangDir', 'dir');
            W('advAccessKey', 'accessKey');
            M.adv.advName = G.data('cke-saved-name') || G.getAttribute('name') || '';
            W('advLangCode', 'lang');
            W('advTabIndex', 'tabindex');
            W('advTitle', 'title');
            W('advContentType', 'type');
            CKEDITOR.plugins.link.synAnchorSelector ? M.adv.advCSSClasses = C(G) : W('advCSSClasses', 'class');
            W('advCharset', 'charset');
            W('advStyles', 'style');
            W('advRel', 'rel');
        }
        var X = M.anchors = [], Y, Z, aa;
        if (CKEDITOR.plugins.link.emptyAnchorFix) {
            var ab = F.document.getElementsByTag('a');
            for (Y = 0, Z = ab.count(); Y < Z; Y++) {
                aa = ab.getItem(Y);
                if (aa.data('cke-saved-name') || aa.hasAttribute('name')) X.push({ name: aa.data('cke-saved-name') || aa.getAttribute('name'), id: aa.getAttribute('id') });
            }
        } else {
            var ac = new CKEDITOR.dom.nodeList(F.document.$.anchors);
            for (Y = 0, Z = ac.count(); Y < Z; Y++) {
                aa = ac.getItem(Y);
                X[Y] = { name: aa.getAttribute('name'), id: aa.getAttribute('id') };
            }
        }
        if (CKEDITOR.plugins.link.fakeAnchor) {
            var ad = F.document.getElementsByTag('img');
            for (Y = 0, Z = ad.count(); Y < Z; Y++) {
                if (aa = CKEDITOR.plugins.link.tryRestoreFakeAnchor(F, ad.getItem(Y))) X.push({ name: aa.getAttribute('name'), id: aa.getAttribute('id') });
            }
        }
        this._.selectedElement = G;
        return M;
    }, q = function(F, G) { if (G[F]) this.setValue(G[F][this.id] || ''); }, r = function(F) { return q.call(this, 'target', F); }, s = function(F) { return q.call(this, 'adv', F); }, t = function(F, G) {
        if (!G[F]) G[F] = {};
        G[F][this.id] = this.getValue() || '';
    }, u = function(F) { return t.call(this, 'target', F); }, v = function(F) { return t.call(this, 'adv', F); };

    function w(F) { return F.replace(/\\'/g, "'"); }

    ;

    function x(F) { return F.replace(/'/g, '\\$&'); }

    ;
    var y = a.config.emailProtection || '';
    if (y && y != 'encode') {
        var z = {};
        y.replace(/^([^(]+)\(([^)]+)\)$/, function(F, G, H) {
            z.name = G;
            z.params = [];
            H.replace(/[^,\s]+/g, function(I) { z.params.push(I); });
        });
    }

    function A(F) {
        var G, H = z.name, I = z.params, J, K;
        G = [H, '('];
        for (var L = 0; L < I.length; L++) {
            J = I[L].toLowerCase();
            K = F[J];
            L > 0 && G.push(',');
            G.push("'", K ? x(encodeURIComponent(F[J])) : '', "'");
        }
        G.push(')');
        return G.join('');
    }

    ;

    function B(F) {
        var G, H = F.length, I = [];
        for (var J = 0; J < H; J++) {
            G = F.charCodeAt(J);
            I.push(G);
        }
        return 'String.fromCharCode(' + I.join(',') + ')';
    }

    ;

    function C(F) {
        var G = F.getAttribute('class');
        return G ? G.replace(/\s*(?:cke_anchor_empty|cke_anchor)(?:\s*$)?/g, '') : '';
    }

    ;
    var D = a.lang.common, E = a.lang.link;
    return {
        title: E.title,
        minWidth: 350,
        minHeight: 230,
        contents: [{
                id: 'info',
                label: E.info,
                title: E.info,
                elements: [{ id: 'linkType', type: 'select', label: E.type, 'default': 'url', items: [[E.toUrl, 'url'], [E.toAnchor, 'anchor'], [E.toEmail, 'email']], onChange: d, setup: function(F) { if (F.type) this.setValue(F.type); }, commit: function(F) { F.type = this.getValue(); } }, {
                        type: 'vbox', id: 'urlOptions',
                        children: [{
                            type: 'hbox', widths: ['25%', '75%'],
                            children: [{
                                    id: 'protocol', type: 'select', label: D.protocol, 'default': 'http://', items: [['http://‎', 'http://'], ['https://‎', 'https://'], ['ftp://‎', 'ftp://'], ['news://‎', 'news://'], [E.other, '']], setup: function(F) { if (F.url) this.setValue(F.url.protocol || ''); },
                                    commit: function(F) {
                                        if (!F.url) F.url = {};
                                        F.url.protocol = this.getValue();
                                    }
                                }, {
                                    type: 'text', id: 'url', label: D.url, required: true, onLoad: function() { this.allowOnChange = true; },
                                    onKeyUp: function() {
                                        var K = this;
                                        K.allowOnChange = false;
                                        var F = K.getDialog().getContentElement('info', 'protocol'), G = K.getValue(), H = /^(http|https|ftp|news):\/\/(?=.)/i, I = /^((javascript:)|[#\/\.\?])/i, J = H.exec(G);
                                        if (J) {
                                            K.setValue(G.substr(J[0].length));
                                            F.setValue(J[0].toLowerCase());
                                        } else if (I.test(G)) F.setValue('');
                                        K.allowOnChange = true;
                                    },
                                    onChange: function() { if (this.allowOnChange) this.onKeyUp(); },
                                    validate: function() {
                                        var F = this.getDialog();
                                        if (F.getContentElement('info', 'linkType') && F.getValueOf('info', 'linkType') != 'url') return true;
                                        if (this.getDialog().fakeObj) return true;
                                        var G = CKEDITOR.dialog.validate.notEmpty(E.noUrl);
                                        return G.apply(this);
                                    },
                                    setup: function(F) {
                                        this.allowOnChange = false;
                                        if (F.url) this.setValue(F.url.url);
                                        this.allowOnChange = true;
                                    },
                                    commit: function(F) {
                                        this.onChange();
                                        if (!F.url) F.url = {};
                                        F.url.url = this.getValue();
                                        this.allowOnChange = false;
                                    }
                                }],
                            setup: function(F) { if (!this.getDialog().getContentElement('info', 'linkType')) this.getElement().show(); }
                        }, { type: 'button', id: 'browse', hidden: 'true', filebrowser: 'info:url', label: D.browseServer }]
                    }, {
                        type: 'vbox',
                        id: 'anchorOptions',
                        width: 260,
                        align: 'center',
                        padding: 0,
                        children: [{
                                type: 'fieldset',
                                id: 'selectAnchorText',
                                label: E.selectAnchor,
                                setup: function(F) {
                                    if (F.anchors.length > 0) this.getElement().show();
                                    else this.getElement().hide();
                                },
                                children: [{
                                    type: 'hbox',
                                    id: 'selectAnchor',
                                    children: [{
                                            type: 'select',
                                            id: 'anchorName',
                                            'default': '',
                                            label: E.anchorName,
                                            style: 'width: 100%;',
                                            items: [['']],
                                            setup: function(F) {
                                                var I = this;
                                                I.clear();
                                                I.add('');
                                                for (var G = 0; G < F.anchors.length; G++) {
                                                    if (F.anchors[G].name) I.add(F.anchors[G].name);
                                                }
                                                if (F.anchor) I.setValue(F.anchor.name);
                                                var H = I.getDialog().getContentElement('info', 'linkType');
                                                if (H && H.getValue() == 'email') I.focus();
                                            },
                                            commit: function(F) {
                                                if (!F.anchor) F.anchor = {};
                                                F.anchor.name = this.getValue();
                                            }
                                        }, {
                                            type: 'select', id: 'anchorId', 'default': '', label: E.anchorId, style: 'width: 100%;', items: [['']],
                                            setup: function(F) {
                                                var H = this;
                                                H.clear();
                                                H.add('');
                                                for (var G = 0; G < F.anchors.length; G++) {
                                                    if (F.anchors[G].id) H.add(F.anchors[G].id);
                                                }
                                                if (F.anchor) H.setValue(F.anchor.id);
                                            },
                                            commit: function(F) {
                                                if (!F.anchor) F.anchor = {};
                                                F.anchor.id = this.getValue();
                                            }
                                        }],
                                    setup: function(F) {
                                        if (F.anchors.length > 0) this.getElement().show();
                                        else this.getElement().hide();
                                    }
                                }]
                            }, {
                                type: 'html', id: 'noAnchors', style: 'text-align: center;', html: '<div role="note" tabIndex="-1">' + CKEDITOR.tools.htmlEncode(E.noAnchors) + '</div>', focus: true,
                                setup: function(F) {
                                    if (F.anchors.length < 1) this.getElement().show();
                                    else this.getElement().hide();
                                }
                            }],
                        setup: function(F) { if (!this.getDialog().getContentElement('info', 'linkType')) this.getElement().hide(); }
                    }, {
                        type: 'vbox', id: 'emailOptions', padding: 1,
                        children: [{
                                type: 'text', id: 'emailAddress', label: E.emailAddress, required: true,
                                validate: function() {
                                    var F = this.getDialog();
                                    if (!F.getContentElement('info', 'linkType') || F.getValueOf('info', 'linkType') != 'email') return true;
                                    var G = CKEDITOR.dialog.validate.notEmpty(E.noEmail);
                                    return G.apply(this);
                                },
                                setup: function(F) {
                                    if (F.email) this.setValue(F.email.address);
                                    var G = this.getDialog().getContentElement('info', 'linkType');
                                    if (G && G.getValue() == 'email') this.select();
                                },
                                commit: function(F) {
                                    if (!F.email) F.email = {};
                                    F.email.address = this.getValue();
                                }
                            }, {
                                type: 'text', id: 'emailSubject', label: E.emailSubject, setup: function(F) { if (F.email) this.setValue(F.email.subject); },
                                commit: function(F) {
                                    if (!F.email) F.email = {};
                                    F.email.subject = this.getValue();
                                }
                            }, {
                                type: 'textarea', id: 'emailBody', label: E.emailBody, rows: 3, 'default': '', setup: function(F) { if (F.email) this.setValue(F.email.body); },
                                commit: function(F) {
                                    if (!F.email) F.email = {};
                                    F.email.body = this.getValue();
                                }
                            }],
                        setup: function(F) { if (!this.getDialog().getContentElement('info', 'linkType')) this.getElement().hide(); }
                    }]
            }, {
                id: 'target',
                label: E.target,
                title: E.target,
                elements: [{
                    type: 'hbox',
                    widths: ['50%', '50%'],
                    children: [{
                            type: 'select',
                            id: 'linkTargetType',
                            label: D.target,
                            'default': 'notSet',
                            style: 'width : 100%;',
                            items: [[D.notSet, 'notSet'], [E.targetFrame, 'frame'], [E.targetPopup, 'popup'], [D.targetNew, '_blank'], [D.targetTop, '_top'], [D.targetSelf, '_self'], [D.targetParent, '_parent']],
                            onChange: c,
                            setup: function(F) {
                                if (F.target) this.setValue(F.target.type || 'notSet');
                                c.call(this);
                            },
                            commit: function(F) {
                                if (!F.target) F.target = {};
                                F.target.type = this.getValue();
                            }
                        }, {
                            type: 'text', id: 'linkTargetName', label: E.targetFrameName, 'default': '', setup: function(F) { if (F.target) this.setValue(F.target.name); },
                            commit: function(F) {
                                if (!F.target) F.target = {};
                                F.target.name = this.getValue().replace(/\W/gi, '');
                            }
                        }]
                }, { type: 'vbox', width: '100%', align: 'center', padding: 2, id: 'popupFeatures', children: [{ type: 'fieldset', label: E.popupFeatures, children: [{ type: 'hbox', children: [{ type: 'checkbox', id: 'resizable', label: E.popupResizable, setup: r, commit: u }, { type: 'checkbox', id: 'status', label: E.popupStatusBar, setup: r, commit: u }] }, { type: 'hbox', children: [{ type: 'checkbox', id: 'location', label: E.popupLocationBar, setup: r, commit: u }, { type: 'checkbox', id: 'toolbar', label: E.popupToolbar, setup: r, commit: u }] }, { type: 'hbox', children: [{ type: 'checkbox', id: 'menubar', label: E.popupMenuBar, setup: r, commit: u }, { type: 'checkbox', id: 'fullscreen', label: E.popupFullScreen, setup: r, commit: u }] }, { type: 'hbox', children: [{ type: 'checkbox', id: 'scrollbars', label: E.popupScrollBars, setup: r, commit: u }, { type: 'checkbox', id: 'dependent', label: E.popupDependent, setup: r, commit: u }] }, { type: 'hbox', children: [{ type: 'text', widths: ['50%', '50%'], labelLayout: 'horizontal', label: D.width, id: 'width', setup: r, commit: u }, { type: 'text', labelLayout: 'horizontal', widths: ['50%', '50%'], label: E.popupLeft, id: 'left', setup: r, commit: u }] }, { type: 'hbox', children: [{ type: 'text', labelLayout: 'horizontal', widths: ['50%', '50%'], label: D.height, id: 'height', setup: r, commit: u }, { type: 'text', labelLayout: 'horizontal', label: E.popupTop, widths: ['50%', '50%'], id: 'top', setup: r, commit: u }] }] }] }]
            }, { id: 'upload', label: E.upload, title: E.upload, hidden: true, filebrowser: 'uploadButton', elements: [{ type: 'file', id: 'upload', label: D.upload, style: 'height:40px', size: 29 }, { type: 'fileButton', id: 'uploadButton', label: D.uploadSubmit, filebrowser: 'info:url', 'for': ['upload', 'upload'] }] }, { id: 'advanced', label: E.advanced, title: E.advanced, elements: [{ type: 'vbox', padding: 1, children: [{ type: 'hbox', widths: ['45%', '35%', '20%'], children: [{ type: 'text', id: 'advId', label: E.id, setup: s, commit: v }, { type: 'select', id: 'advLangDir', label: E.langDir, 'default': '', style: 'width:110px', items: [[D.notSet, ''], [E.langDirLTR, 'ltr'], [E.langDirRTL, 'rtl']], setup: s, commit: v }, { type: 'text', id: 'advAccessKey', width: '80px', label: E.acccessKey, maxLength: 1, setup: s, commit: v }] }, { type: 'hbox', widths: ['45%', '35%', '20%'], children: [{ type: 'text', label: E.name, id: 'advName', setup: s, commit: v }, { type: 'text', label: E.langCode, id: 'advLangCode', width: '110px', 'default': '', setup: s, commit: v }, { type: 'text', label: E.tabIndex, id: 'advTabIndex', width: '80px', maxLength: 5, setup: s, commit: v }] }] }, { type: 'vbox', padding: 1, children: [{ type: 'hbox', widths: ['45%', '55%'], children: [{ type: 'text', label: E.advisoryTitle, 'default': '', id: 'advTitle', setup: s, commit: v }, { type: 'text', label: E.advisoryContentType, 'default': '', id: 'advContentType', setup: s, commit: v }] }, { type: 'hbox', widths: ['45%', '55%'], children: [{ type: 'text', label: E.cssClasses, 'default': '', id: 'advCSSClasses', setup: s, commit: v }, { type: 'text', label: E.charset, 'default': '', id: 'advCharset', setup: s, commit: v }] }, { type: 'hbox', widths: ['45%', '55%'], children: [{ type: 'text', label: E.rel, 'default': '', id: 'advRel', setup: s, commit: v }, { type: 'text', label: E.styles, 'default': '', id: 'advStyles', validate: CKEDITOR.dialog.validate.inlineStyle(a.lang.common.invalidInlineStyle), setup: s, commit: v }] }] }] }],
        onShow: function() {
            var F = this.getParentEditor(), G = F.getSelection(), H = null;
            if ((H = b.getSelectedLink(F)) && H.hasAttribute('href')) G.selectElement(H);
            else H = null;
            this.setupContent(p.apply(this, [F, H]));
        },
        onOk: function() {
            var F = {}, G = [], H = {}, I = this, J = this.getParentEditor();
            this.commitContent(H);
            switch (H.type || 'url') {
            case 'url':
                var K = H.url && H.url.protocol != undefined ? H.url.protocol : 'http://', L = H.url && CKEDITOR.tools.trim(H.url.url) || '';
                F['data-cke-saved-href'] = L.indexOf('/') === 0 ? L : K + L;
                break;
            case 'anchor':
                var M = H.anchor && H.anchor.name, N = H.anchor && H.anchor.id;
                F['data-cke-saved-href'] = '#' + (M || N || '');
                break;
            case 'email':
                var O, P = H.email, Q = P.address;
                switch (y) {
                case '':
                case 'encode':
                    var R = encodeURIComponent(P.subject || ''), S = encodeURIComponent(P.body || ''), T = [];
                    R && T.push('subject=' + R);
                    S && T.push('body=' + S);
                    T = T.length ? '?' + T.join('&') : '';
                    if (y == 'encode') {
                        O = ["javascript:void(location.href='mailto:'+", B(Q)];
                        T && O.push("+'", x(T), "'");
                        O.push(')');
                    } else O = ['mailto:', Q, T];
                    break;
                default:
                    var U = Q.split('@', 2);
                    P.name = U[0];
                    P.domain = U[1];
                    O = ['javascript:', A(P)];
                }
                F['data-cke-saved-href'] = O.join('');
                break;
            }
            if (H.target)
                if (H.target.type == 'popup') {
                    var V = ["window.open(this.href, '", H.target.name || '', "', '"], W = ['resizable', 'status', 'location', 'toolbar', 'menubar', 'fullscreen', 'scrollbars', 'dependent'], X = W.length, Y = function(ai) { if (H.target[ai]) W.push(ai + '=' + H.target[ai]); };
                    for (var Z = 0; Z < X; Z++) W[Z] = W[Z] + (H.target[W[Z]] ? '=yes' : '=no');
                    Y('width');
                    Y('left');
                    Y('height');
                    Y('top');
                    V.push(W.join(','), "'); return false;");
                    F['data-cke-pa-onclick'] = V.join('');
                    G.push('target');
                } else {
                    if (H.target.type != 'notSet' && H.target.name) F.target = H.target.name;
                    else G.push('target');
                    G.push('data-cke-pa-onclick', 'onclick');
                }
            if (H.adv) {
                var aa = function(ai, aj) {
                    var ak = H.adv[ai];
                    if (ak) F[aj] = ak;
                    else G.push(aj);
                };
                aa('advId', 'id');
                aa('advLangDir', 'dir');
                aa('advAccessKey', 'accessKey');
                if (H.adv.advName) F.name = F['data-cke-saved-name'] = H.adv.advName;
                else G = G.concat(['data-cke-saved-name', 'name']);
                aa('advLangCode', 'lang');
                aa('advTabIndex', 'tabindex');
                aa('advTitle', 'title');
                aa('advContentType', 'type');
                aa('advCSSClasses', 'class');
                aa('advCharset', 'charset');
                aa('advStyles', 'style');
                aa('advRel', 'rel');
            }
            var ab = J.getSelection();
            F.href = F['data-cke-saved-href'];
            if (!this._.selectedElement) {
                var ac = ab.getRanges(true);
                if (ac.length == 1 && ac[0].collapsed) {
                    var ad = new CKEDITOR.dom.text(H.type == 'email' ? H.email.address : F['data-cke-saved-href'], J.document);
                    ac[0].insertNode(ad);
                    ac[0].selectNodeContents(ad);
                    ab.selectRanges(ac);
                }
                var ae = new CKEDITOR.style({ element: 'a', attributes: F });
                ae.type = CKEDITOR.STYLE_INLINE;
                ae.apply(J.document);
            } else {
                var af = this._.selectedElement, ag = af.data('cke-saved-href'), ah = af.getHtml();
                af.setAttributes(F);
                af.removeAttributes(G);
                if (H.adv && H.adv.advName && CKEDITOR.plugins.link.synAnchorSelector) af.addClass(af.getChildCount() ? 'cke_anchor' : 'cke_anchor_empty');
                if (ag == ah || H.type == 'email' && ah.indexOf('@') != -1) af.setHtml(H.type == 'email' ? H.email.address : F['data-cke-saved-href']);
                ab.selectElement(af);
                delete this._.selectedElement;
            }
        },
        onLoad: function() {
            if (!a.config.linkShowAdvancedTab) this.hidePage('advanced');
            if (!a.config.linkShowTargetTab) this.hidePage('target');
        },
        onFocus: function() {
            var F = this.getContentElement('info', 'linkType'), G;
            if (F && F.getValue() == 'url') {
                G = this.getContentElement('info', 'url');
                G.select();
            }
        }
    };
});