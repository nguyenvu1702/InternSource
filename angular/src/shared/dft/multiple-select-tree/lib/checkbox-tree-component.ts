/* tslint:disable */
import { AfterViewChecked, AfterViewInit, Component, ElementRef, Injector, OnInit, Output } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { CheckboxTreeEditModel } from './checkbox-tree-edit.model';
import * as _ from 'lodash';
import { EventEmitter } from '@angular/core';

@Component({
    selector: 'checkbox-tree',
    template: `
            <div class="permission-tree"></div>
   `
})
export class CheckBoxTreeComponent implements AfterViewInit {

    @Output() onSelectedChange = new EventEmitter<any[]>();
    permissionNames: any[] = [];

    set editData(val: CheckboxTreeEditModel) {
        this._editData = val;
        this.refreshTree();
    }

    private _$tree: JQuery;
    private _editData: CheckboxTreeEditModel;
    private _createdTreeBefore;
    constructor(private _element: ElementRef) { }

    ngAfterViewInit(): void {

        this._$tree = $(this._element.nativeElement);

        this._$tree.on('select_node.jstree', function (e, data) {
            if (data.event) { data.instance.select_node(data.node.children_d); }
        }).on('deselect_node.jstree', function (e, data) {
            if (data.event) {
                data.instance.deselect_node(data.node.children_d);
            }
        });

        this.refreshTree();
    }

    getGrantedPermissionNames(): any[] {
        if (!this._$tree || !this._createdTreeBefore) {
            return [];
        }

        this.permissionNames = [];
        let selectedPermissions = this._$tree.jstree('get_selected', true);
        for (let i = 0; i < selectedPermissions.length; i++) {
            this.permissionNames.push({
                id: selectedPermissions[i].original.id,
                parentId: selectedPermissions[i].original.parent,
                displayName: selectedPermissions[i].original.text,
            });
        }
        this.onSelectedChange.emit(this.permissionNames)
        return this.permissionNames;
    }

    refreshTree(): void {
        let self = this;

        if (this._createdTreeBefore) {
            this._$tree.jstree('destroy');
        }

        this._createdTreeBefore = false;

        if (!this._editData || !this._$tree) {
            return;
        }

        let treeData = _.map(this._editData.data, function (item) {
            return {
                id: item.id,
                parent: item.parentId ? item.parentId : '#',
                text: item.displayName,
                state: {
                    opened: true,
                    selected: _.includes(self._editData.selected, item.id),
                    disabled: self._editData.disabled,
                }
            };
        });

        this._$tree.jstree({
            'core': {
                data: treeData,
                "themes": {
                    "icons": false
                }
            },
            'types': {
                'default': {
                    'icon': 'fas fa-folder m--font-warning'
                },
                'file': {
                    'icon': 'fa fa-file m--font-warning'
                }
            },
            'checkbox': {
                keep_selected_style: false,
                three_state: false,
                cascade: ''
            },
            plugins: ['checkbox', 'types']
        });

        this._createdTreeBefore = true;

        let inTreeChangeEvent = false;

        function selectNodeAndAllParents(node) {
            self._$tree.jstree('select_node', node, true);
            let parent = self._$tree.jstree('get_parent', node);
            if (parent) {
                selectNodeAndAllParents(parent);
            }
        }

        this._$tree.on('changed.jstree', function (e, data) {
            self.getGrantedPermissionNames();
            if (!data.node || !inTreeChangeEvent) {
                return;
            }

            let wasInTreeChangeEvent = inTreeChangeEvent;
            if (!wasInTreeChangeEvent) {
                inTreeChangeEvent = true;
            }

            let childrenNodes;

            if (data.node.state.selected) {
                selectNodeAndAllParents(self._$tree.jstree('get_parent', data.node));
                childrenNodes = $.makeArray(self._$tree.jstree('get_children_dom', data.node));
                self._$tree.jstree('select_node', childrenNodes);

            } else {
                childrenNodes = $.makeArray(self._$tree.jstree('get_children_dom', data.node));
                self._$tree.jstree('deselect_node', childrenNodes);
            }

            if (!wasInTreeChangeEvent) {
                inTreeChangeEvent = false;
            }
        });
    }
}
