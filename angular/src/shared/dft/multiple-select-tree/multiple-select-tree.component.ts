import { Component, Injector, Input, ViewChild, OnChanges, SimpleChanges, EventEmitter, Output, HostListener, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { isNil } from 'lodash';
import { CheckBoxTreeComponent } from './lib/checkbox-tree-component';
import { CheckboxTreeEditModel } from './lib/checkbox-tree-edit.model';

@Component({
  selector: 'app-multiple-select-tree',
  templateUrl: './multiple-select-tree.component.html',
  styleUrls: ['./multiple-select-tree.component.scss']
})
export class MultipleSelectTreeComponent extends AppComponentBase implements OnChanges {

  @Output() onSelect = new EventEmitter<number[]>();
  @ViewChild('checkboxTree', { static: true }) checkboxTree: CheckBoxTreeComponent;
  @ViewChild('treeItem', { static: true }) treeItem: ElementRef;
  @ViewChild('btn', { static: true }) btn: ElementRef;
  @Input() dataEdit: CheckboxTreeEditModel;
  show = true;
  selectedValue = 'Chọn';
  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.show && !this.btn.nativeElement.contains(event.target)) {
      this.show = !this._isEventFromToggle(event);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.dataEdit.currentValue !== changes.dataEdit.previousValue) {
      this.checkboxTree.editData = changes.dataEdit.currentValue;
    }
  }

  onSelectedChange(event: any[]) {
    this.selectedValue = event.length > 0 ? event.map(e => e.displayName).join() : 'Chọn';
    this.onSelect.emit(event.map(e => e.id));
  }

  private _isEventFromToggle(event: MouseEvent): boolean {
    return !isNil(this.treeItem) && this.treeItem.nativeElement.contains(event.target);
  }
}
